using System;
using UnityEngine;

namespace Funzilla
{
	public class SpeedupTrigger : MonoBehaviour
	{
		[SerializeField] private MeshRenderer model;
		private void OnTriggerEnter(Collider other)
		{
			if (!enabled) return;
			if (other.gameObject.layer != Layers.Sensor) return;
			if (!Gameplay.Instance.Playing) return;
			enabled = false;
			Gameplay.Instance.Player.Speedup();
		}
	}
}