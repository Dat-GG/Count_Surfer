using System.Collections.Generic;
using UnityEngine;

namespace Funzilla
{
	internal class Wall : Obstacle, ITrapTriggerListener
	{
		private const int Spacing = 1;
		[SerializeField] internal Brick[] fullBricks;
		[SerializeField] internal Brick[] halfBricks;
		[SerializeField] internal BoxCollider boxCollider;
		[SerializeField] internal TrapTrigger trigger;
		private List<Brick> _bricks;

		private Transform CreateRow(string rowName, float y)
		{
			var row = new GameObject(rowName).transform;
			row.SetParent(transform.GetChild(0));
			row.localPosition = new Vector3(0, y, 0);
			row.localRotation = Quaternion.identity;
			return row;
		}
		internal void Init(EditWall info, float begin)
		{
			Begin = begin;
			End = begin + info.spacing;
			trigger.Listener = this;

			_bricks = new List<Brick>(info.height * 3 * Gameplay.LaneCount);
			var t = transform.GetChild(0);
			t.localPosition = new Vector3(
				Gameplay.CalculatePosition(info.laneL, info.laneR), 0, 0);

			var w = Gameplay.CalculateWidth(info.laneL, info.laneR);
			boxCollider.center = new Vector3(0, info.height * 0.5f, 0.4f);
			boxCollider.size = new Vector3(w, info.height, 0.2f);

			var n = info.height * 2;
			for (var i = 0; i < n; i++)
			{
				if (i % 2 == 0)
				{
					var even = CreateRow("BrickRowEven", 0.25f);
					var x = (-w + Gameplay.LaneWidth) * 0.5f;
					for (var j = info.laneL; j <= info.laneR; j++)
					{
						var brick = Instantiate(fullBricks[Random.Range(0, fullBricks.Length)], even);
						brick.transform.localPosition = new Vector3(x, 0, 0);
						x += Gameplay.LaneWidth;
						_bricks.Add(brick);
					}
					even.localPosition = new Vector3(0, Gameplay.LaneWidth * (i * 0.5f + 0.25f), 0);
				}
				else
				{
					var odd = CreateRow("BrickRowOdd", 0.75f);
					var x = (-w + Gameplay.LaneWidth + 1) * 0.5f;
					var left = Instantiate(halfBricks[Random.Range(0, halfBricks.Length)], odd);
					left.transform.localPosition = new Vector3(x - 0.75f * Gameplay.LaneWidth, 0, 0);
					_bricks.Add(left);
					for (var j = info.laneL; j < info.laneR; j++)
					{
						var brick = Instantiate(fullBricks[Random.Range(0, fullBricks.Length)], odd);
						brick.transform.localPosition = new Vector3(x, 0, 0);
						x += Gameplay.LaneWidth;
						_bricks.Add(brick);
					}
					var right = Instantiate(halfBricks[Random.Range(0, halfBricks.Length)], odd);
					right.transform.localPosition = new Vector3(x - 0.25f * Gameplay.LaneWidth, 0, 0);
					_bricks.Add(right);
					odd.localPosition = new Vector3(0, Gameplay.LaneWidth * (i * 0.5f + 0.25f), 0);
				}
			}
		}

		internal static int CalculateSpacing(int height)
		{
			return height * Spacing;
		}

		private bool _broken;

		public void OnCollided(Tablet tablet)
		{
			Gameplay.Instance.Player.Stack.BreakTablet(tablet);
			if (_broken) return;
			_broken = true;
			foreach (var brick in _bricks) brick.WakeUp();
		}
	}
}