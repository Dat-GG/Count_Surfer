using UnityEngine;

namespace Funzilla
{
	internal class Player : MonoBehaviour
	{
		[SerializeField] private Stack stack;
		[SerializeField] private Rigidbody characterRigidbody;
		internal Character Character { get; private set; }
		internal Transform CharacterTransform => characterRigidbody.transform;
		internal Stack Stack => stack;
		private float _speed;
		internal Vector3 Velocity => transform.forward * _speed;

		private bool _touched;
		private Vector3 _touchPos;
		private float _position;
		private Path _path;

		internal void Init()
		{
			LoadCharacter();
			stack.Init();
			_speed = Gameplay.NormalSpeed;
			_path = Gameplay.Instance.Level.Path;
		}

		private void LoadCharacter()
		{
			if (Character) Destroy(Character.gameObject);
			var prefab = Resources.Load<Character>($"Characters/{Profile.Instance.CurrentSkin}");
			Character = Instantiate(prefab, characterRigidbody.transform);
		}

		internal void SwapCharacter()
		{
			var profile = Profile.Instance;
			if (profile.Skins.Count <= 0) return;
			profile.CurrentSkinIndex = (profile.CurrentSkinIndex + 1) % profile.Skins.Count;
			LoadCharacter();
		}

		internal void Die()
		{
			characterRigidbody.isKinematic = true;
			Stack.Fall();
		}

		internal void Speedup()
		{
			_speed = Gameplay.FastSpeed;
		}

		internal void Jump()
		{
			var v = Vector3.up * 10;
			characterRigidbody.velocity = v;
			stack.Jump(v);
		}

		internal void DoFrame()
		{
			if (_speed > Gameplay.NormalSpeed)
			{
				_speed -= Time.smoothDeltaTime * Gameplay.Deacceleration;
				if (_speed < Gameplay.NormalSpeed)
				{
					_speed = Gameplay.NormalSpeed;
				}
			}
			_position += _speed * Time.smoothDeltaTime;
			var progress = Mathf.Min(_position / Gameplay.Instance.Level.Length, 1.0f);
			Gameplay.Instance.UpdateProgress(progress);
			var path = _path.Place(transform, _position);
			if (path == null)
			{
				Gameplay.Instance.Win();
				return;
			}

			if (path.Height != _path.Height)
			{
				stack.ChangeHeight(path.Height, path.Finishing);
				var dv = new Vector3(0, _path.Height - path.Height, 0);
				if (!Character.Dead)
				{
					characterRigidbody.transform.localPosition += dv;
				}
				else
				{
					Character.transform.localPosition += dv;
				}
			}

			_path = path;
			if (Input.GetMouseButtonDown(0))
			{
				_touched = true;
				_touchPos = Input.mousePosition;
			}

			if (Input.GetMouseButtonUp(0))
			{
				_touched = false;
			}

			if (!_touched) return;

			var v = Input.mousePosition - _touchPos;
			_touchPos = Input.mousePosition;
			var t = stack.transform;
			var p = t.localPosition;

			const float w = (Gameplay.RoadWidth - Gameplay.LaneWidth) * 0.5f;
			p.x = Mathf.Clamp(p.x + v.x * Screen.width / 1080.0f * Gameplay.ControlSensivity, -w, w);
			t.localPosition = p;
		}
	}
}