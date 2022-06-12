using UnityEngine;

namespace Funzilla
{
	public class GateTrigger : MonoBehaviour
	{
		[SerializeField] private Gate1 gate1;
		private void OnTriggerEnter(Collider other)
		{
			SoundManager.Instance.PlaySfx("CubeCollect3");
			if (!enabled) return;
			if (other.gameObject.layer != 7) return;
			if (!Gameplay.Instance.Playing) return;
			enabled = false;
			gate1.Activate();
		}
	}
}