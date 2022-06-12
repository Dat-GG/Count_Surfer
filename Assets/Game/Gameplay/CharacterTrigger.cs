using System;
using UnityEngine;

namespace Funzilla
{
	public class CharacterTrigger : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (!Gameplay.Instance.Playing || !enabled || other.gameObject.layer != Layers.Trap) return;
			Gameplay.Instance.Player.Die();
			enabled = false;
		}
	}
}