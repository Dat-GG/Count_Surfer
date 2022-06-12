using UnityEngine;

namespace Funzilla
{
	internal interface ITrapTriggerListener
	{
		void OnCollided(Tablet tablet);
	}
	internal class TrapTrigger : MonoBehaviour
	{
		internal ITrapTriggerListener Listener;

		private void OnCollisionEnter(Collision other)
		{
			OnTriggerEnter(other.collider);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!Gameplay.Instance.Playing) return;
			if (other.gameObject.layer != Layers.Tablet) return;
			var tablet = other.gameObject.GetComponent<Tablet>();
			if (!tablet) return;
			Listener?.OnCollided(tablet);
		}
	}
}