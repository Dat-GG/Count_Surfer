using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Funzilla
{
	internal class Path
	{
		private Path _next;
		internal int Height { get; private protected set; }
		internal float Begin { get; private protected set; }
		internal float End { get; private protected set; }
		internal float Length { get; private protected set; }
		internal bool Finishing { get; set; }

		internal void Append(Path path)
		{
			_next = path;
		}

		internal Path Place(Transform obj, float position)
		{
			var p = position - Begin;
			if (p > Length)
			{
				if (_next != null)
				{
					return _next.Place(obj, position);
				}
				else
				{
					PlaceInside(obj, p);
					return null;
				}
			}

			PlaceInside(obj, p);
			return this;
		}

		protected virtual void PlaceInside(Transform obj, float position) { }
	}

	internal class PathStraight : Path
	{
		internal Vector2 Entry { get; private set; }
		internal Vector2 Exit { get; private set; }
		internal Vector2 Direction { get; private set; }
		private readonly List<EditHole> _holes;
		private readonly Vector3 _position;
		private readonly Vector3 _forward;

		internal PathStraight(float begin, Vector2 entry, Vector2 exit, int height, List<EditHole> holes)
		{
			Begin = begin;
			Entry = entry;
			Exit = exit;
			Height = height;
			Direction = exit - entry;
			Length = Direction.magnitude;
			Direction /= Length;
			_holes = holes;
			_position = new Vector3(entry.x, height, entry.y);
			_forward = new Vector3(Direction.x, 0, Direction.y);
			End = begin + Length;
			Debug.DrawLine(_position, _position + _forward * Length, Color.red, 5);
		}

		protected override void PlaceInside(Transform obj, float position)
		{
			obj.forward = new Vector3(Direction.x, 0, Direction.y);
			var p = Entry + Direction * position;
			obj.position = new Vector3(p.x, Height, p.y);
		}

		private void MakeBridge(Component level, int lane, float begin, float end)
		{
			var length = end - begin;
			if (length < 0.01f)
			{
				return;
			}

			var bridge = Gameplay.NewBridge(level.transform);
			bridge.Init(lane, length, Height);
			PlaceInside(bridge.transform, begin - Begin);
			bridge.transform.position = _position + _forward * (begin - Begin);
		}

		private class Cluster
		{
			private Node _head;
			private Node _tail;
			internal float Min;
			internal float Max;
			private int _count;
			internal List<EditHole> Holes;

			internal void MakeList()
			{
				Holes = new List<EditHole>(_count);
				for (var node = _head; node != null; node = node.Next)
				{
					Holes.Add(node.Hole);
					node.Cluster = null;
				}
				Holes.Sort((a, b) => (a.Begin < b.Begin) ? -1 : 1);
			}

			internal void Join(Node node)
			{
				if (node.Cluster != null)
				{
					if (_head == null)
					{
						_head = node.Cluster._head;
						Min = node.Cluster.Min;
						Max = node.Cluster.Max;
						_count = node.Cluster._count;
					}
					else
					{
						if (Min > node.Cluster.Max) return;
						if (Max < node.Cluster.Min) return;
						if (Min > node.Cluster.Min) Min = node.Cluster.Min;
						if (Max < node.Cluster.Max) Max = node.Cluster.Max;
						_count += node.Cluster._count;
						_tail.Next = node;
					}

					_tail = node.Cluster._tail;
					node.Cluster._head = null;
					node.Cluster._tail = null;
					for (var p = node; p != null; p = p.Next)
					{
						p.Cluster = this;
					}
				}
				else
				{
					if (_head == null)
					{
						_head = node;
						Min = node.Hole.Begin;
						Max = node.Hole.End;
						_count = 1;
					}
					else
					{
						if (Min > node.Hole.End) return;
						if (Max < node.Hole.Begin) return;
						if (Min > node.Hole.Begin) Min = node.Hole.Begin;
						if (Max < node.Hole.End) Max = node.Hole.End;
						_tail.Next = node;
						_count++;
					}
					_tail = node;
					_tail.Cluster = this;
				}
			}
		}

		private class Node
		{
			internal readonly EditHole Hole;
			internal Node Next;
			internal Cluster Cluster;

			internal Node(EditHole hole)
			{
				Hole = hole;
				var cluster = new Cluster();
				cluster.Join(this);
			}

			internal void Join(Node other)
			{
				if (Cluster != other.Cluster)
				{
					Cluster.Join(other);
				}
			}
		}

		private float Mend(Component level, Cluster cluster, float position)
		{
			var length = cluster.Min - position;
			if (length > 0.01f)
			{
				var straight = Gameplay.NewStraight(level.transform);
				straight.Setup(Gameplay.RoadWidth, length, Height);
				PlaceInside(straight.transform, position - Begin);
			}

			var begin = new float[Gameplay.LaneCount];

			for (var i = 0; i < Gameplay.LaneCount; i++)
			{
				begin[i] = cluster.Min;
				var lane = i + 1;
				foreach (var hole in cluster.Holes.Where(hole => lane >= hole.laneL && lane <= hole.laneR))
				{
					MakeBridge(level, lane, begin[i], hole.Begin);
					begin[i] = hole.End;
				}
			}

			for (var i = 0; i < Gameplay.LaneCount; i++)
			{
				MakeBridge(level, i + 1, begin[i], cluster.Max);
			}
			return cluster.Max;
		}

		internal void Spawn(Level level)
		{
			var nodes = new Node[_holes.Count];

			for (var i = 0; i < _holes.Count; i++)
			{
				nodes[i] = new Node(_holes[i]);
			}
			for (var i = 0; i < nodes.Length; i++)
			{
				for (var j = i + 1; j < nodes.Length; j++)
				{
					nodes[i].Join(nodes[j]);
				}
			}

			var clusters = new List<Cluster>();
			foreach (var node in nodes)
			{
				if (node.Cluster == null) continue;
				clusters.Add(node.Cluster);
				node.Cluster.MakeList();
			}

			clusters.Sort((a,b)=>a.Min < b.Min ? -1 : 1);
			var position = clusters.Aggregate(Begin, (current, cluster) => Mend(level, cluster, current));
			var length = End - position;
			if (!(length > 0.01f)) return;
			var straight = Gameplay.NewStraight(level.transform);
			straight.Setup(Gameplay.RoadWidth, length, Height);
			PlaceInside(straight.transform, position - Begin);
		}
	}

	internal class PathTurn : Path
	{
		private readonly Vector2 _vx;
		private readonly Vector2 _vy;
		private readonly Vector2 _center;
		private readonly float _angle;
		internal Vector2 Exit { get; private set; }
		private readonly float _sign;

		internal PathTurn(
			float begin,
			Vector2 directionIn,
			Vector2 directionOut,
			Vector2 entry,
			int height)
		{
			Begin = begin;
			_vy = directionIn;
			_sign = _vy.x * directionOut.y - _vy.y * directionOut.x > 0 ? 1.0f : -1.0f;
			_vx = new Vector2(-_vy.y * _sign, _vy.x * _sign);
			_center = entry + _vx * Gameplay.TurnRadius;
			Height = height;
			_angle = Mathf.Acos(Vector2.Dot(directionIn, directionOut));
			Length = Gameplay.TurnRadius * _angle;
			End = begin + Length;
			Exit = _center - _sign * new Vector2(-directionOut.y, directionOut.x) * Gameplay.TurnRadius;

			Debug.DrawLine(
				new Vector3(entry.x, 0, entry.y),
				new Vector3(Exit.x, 0, Exit.y), Color.magenta, 5);
		}

		protected override void PlaceInside(Transform obj, float position)
		{
			var angle = position / Length * _angle;
			var v = Mathf.Cos(angle) * _vy + Mathf.Sin(angle) * _vx;
			var p = _center - new Vector2(-v.y, v.x) * (_sign * Gameplay.TurnRadius);

			obj.position = new Vector3(p.x, Height, p.y);
			obj.forward = new Vector3(v.x, 0, v.y).normalized;
		}
	}
}