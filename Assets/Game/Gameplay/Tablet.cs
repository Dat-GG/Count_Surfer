using UnityEngine;

namespace Funzilla
{
	internal class Tablet : MonoBehaviour
	{
		internal const float Thickness = 0.2f;
		[SerializeField] private Rigidbody body;

		internal Vector3 Velocity;
		private Color _originColor;
		private MeshRenderer _meshRenderer;

		private void Awake()
		{
			_meshRenderer = GetComponent<MeshRenderer>();
			_originColor = _meshRenderer.material.color;
		}

		internal void Discard(Vector3 velocity, bool finishing = false)
		{
			enabled = false;
			transform.SetParent(Gameplay.Instance.Level.transform);

			if (finishing)
			{
				SoundManager.Instance.PlaySfx("CubeCollect2");
			}
			else
			{
				gameObject.layer = Layers.DeadTablet;
				body.constraints = RigidbodyConstraints.None;
				body.velocity = velocity + Velocity;
				_meshRenderer.SetPropertyBlock(Stack.DeadColor);
			}
		}

		internal void Recyle()
		{
			enabled = true;
			gameObject.layer = Layers.Tablet;
			gameObject.SetActive(true);
			body.constraints = RigidbodyConstraints.FreezeAll;
			body.velocity = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			_meshRenderer.SetPropertyBlock(Stack.LiveColor);
		}

		private void OnDisable()
		{
			_meshRenderer.material.color = _originColor;
		}
	}
}