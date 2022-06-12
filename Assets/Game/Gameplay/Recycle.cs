using UnityEngine;

namespace Funzilla
{
	internal class Recycle : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (!other.gameObject.activeSelf) return;
			var tablet = other.GetComponent<Tablet>();
			if (tablet) Gameplay.Instance.Player.Stack.Recycle(tablet);
		}
	}
}