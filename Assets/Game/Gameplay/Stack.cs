using System.Collections.Generic;
using UnityEngine;

namespace Funzilla
{
	internal class Stack : MonoBehaviour
	{
		[SerializeField] private Tablet tabletPrefab;
		[SerializeField] private Rigidbody physics;
		private readonly Stack<Tablet> _pool = new Stack<Tablet>();
		private static readonly int ColorID = Shader.PropertyToID("_Color");
		internal static MaterialPropertyBlock DeadColor { get; private set; }
		internal static MaterialPropertyBlock LiveColor { get; private set; }

		private readonly LinkedList<Tablet> _tablets = new LinkedList<Tablet>();
		private int _height;

		private float _timerForSfx;

		internal int TabletCount => _tablets.Count;

		internal void Init()
		{
			Add(Profile.Instance.TabletAmount);
			DeadColor = new MaterialPropertyBlock();
			DeadColor.SetColor(ColorID, new Color32(162, 100, 0, 255));

			LiveColor = new MaterialPropertyBlock();
			LiveColor.SetColor(ColorID, new Color32(255, 184, 69, 255));
		}

		internal void Add(int n)
		{
			Vector3 p, v;
			if (_tablets.Count <= 0)
			{
				p = new Vector3(0, Tablet.Thickness, 0);
				v = Vector3.zero;
			}
			else
			{
				var last = _tablets.Last.Value;
				p = last.transform.localPosition + new Vector3(0, Tablet.Thickness * 0.5f, 0);
				v = last.Velocity;
			}
			for (var i = 0; i < n; i++)
			{
				p.y += Tablet.Thickness;
				var tablet = NewTablet(transform);
				tablet.transform.localPosition = p;
				tablet.Velocity = v;
				_tablets.AddLast(tablet);
			}
			Gameplay.Instance.Player.Character.Jump();
			Gameplay.Instance.Player.CharacterTransform.localPosition =
				_tablets.Last.Value.transform.localPosition + new Vector3(0, Tablet.Thickness * 0.5f, 0);
		}

		private void SetBasePosition(Vector3 position)
		{
			var v = physics.velocity;
			physics.transform.localPosition = position;
			physics.velocity = v;
		}

		internal void Subtract(int n)
		{
			Gameplay.Instance.Player.Character.Balance();
			var v = Gameplay.Instance.Player.Velocity;
			for (var p = _tablets.First; p != null & n > 0; n--)
			{
				var q = p.Next;
				p.Value.Discard(v);
				_tablets.Remove(p);
				p = q;
			}

			if (_tablets.Count <= 0)
			{
				Gameplay.Instance.Lose();
			}
			else
			{
				SetBasePosition(_tablets.First.Value.transform.localPosition);
			}
		}

		internal void Fall()
		{
			Subtract(_tablets.Count);
		}

		internal void Multiply(int n)
		{
			switch (n)
			{
				case 1:
					return;
				case 0:
					Subtract(_tablets.Count);
					break;
				default:
					Add(_tablets.Count * (n - 1));
					break;
			}
		}

		internal void ChangeHeight(int height, bool finishing)
		{
			var v = new Vector3(0, _height - height, 0);
			var vel = Gameplay.Instance.Player.Velocity;
			for (var p = _tablets.First; p != null;)
			{
				var q = p.Next;
				p.Value.transform.localPosition += v;
				if (p.Value.transform.position.y < height)
				{
					p.Value.Discard(vel, finishing);
					_tablets.Remove(p);
				}
				p = q;
			}

			_height = height;

			if (_tablets.Count > 0)
			{
				SetBasePosition(_tablets.First.Value.transform.localPosition);
				return;
			}

			if (finishing)
			{
				physics.gameObject.layer = Layers.Tablet;
				enabled = false;
				Gameplay.Instance.Win();
			}
			else
			{
				Gameplay.Instance.Lose();
			}
		}

		internal void Jump(Vector3 v)
		{
			physics.velocity = v;
		}

		internal void Discard(Tablet tablet)
		{
			if (tablet.gameObject.layer != Layers.Tablet) return;
			tablet.Discard(Gameplay.Instance.Player.Velocity);
			_tablets.Remove(tablet);
			if (_tablets.Count > 0)
			{
				SetBasePosition(_tablets.First.Value.transform.localPosition);
				return;
			}
			SetBasePosition(new Vector3(0, _height + Tablet.Thickness * 0.5f, 0));
			Gameplay.Instance.Lose();
		}

		internal void Recycle(Tablet tablet)
		{
			tablet.gameObject.SetActive(false);
			_pool.Push(tablet);
		}

		private Tablet NewTablet(Transform parent)
		{
			if (_pool.Count <= 0) return Instantiate(tabletPrefab, parent);
			var tablet = _pool.Pop();
			tablet.transform.SetParent(parent);
			tablet.Recyle();
			return tablet;
		}

		internal void BreakTablet(Tablet tablet)
		{
			Discard(tablet);
			if (_timerForSfx < 0.5f) return;
			_timerForSfx = 0;
			SoundManager.Instance.PlaySfx("WallBricks");
		}

		private void Update()
		{
			_timerForSfx += Time.deltaTime;
		}

		private void FixedUpdate()
		{
			var p = physics.transform.localPosition;
			var v = physics.velocity;

			for (var node = _tablets.First; node != null; node = node.Next)
			{
				var tablet = node.Value;
				tablet.Velocity += Physics.gravity * Time.fixedDeltaTime;
				tablet.transform.localPosition += tablet.Velocity * Time.fixedDeltaTime;
				if (tablet.transform.localPosition.y < p.y)
				{
					tablet.transform.localPosition = p;
					tablet.Velocity = v;
				}

				v = tablet.Velocity;
				p = tablet.transform.localPosition;
				p.y += Tablet.Thickness;
			}

			p.y -= Tablet.Thickness * 0.5f;
			Gameplay.Instance.Player.CharacterTransform.localPosition = p;
		}
	}
}