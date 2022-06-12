
using System;
using UnityEngine;

namespace Funzilla
{
	internal class DiamondTrigger : MonoBehaviour
	{
		[SerializeField] private MeshRenderer diamondRenderer;
		[SerializeField] private GameObject diamondFx;
		private void OnTriggerEnter(Collider other)
		{
			if (!enabled) return;
			if (other.gameObject.layer != 7) return;
			if (!Gameplay.Instance.Playing) return;
			enabled = false;
			SoundManager.Instance.PlaySfx("GemCollect2");
			Gameplay.Instance.AddCoins(1);
			diamondRenderer.enabled = false;
			diamondFx.SetActive(true);
		}
	}
}