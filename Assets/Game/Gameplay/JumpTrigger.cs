using UnityEngine;

namespace Funzilla
{
	public class JumpTrigger : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (!enabled) return;
			if (other.gameObject.layer != 7) return;
			if (!Gameplay.Instance.Playing) return;
			enabled = false;
			Gameplay.Instance.Player.Jump();
		}
	}
}