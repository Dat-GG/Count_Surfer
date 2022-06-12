using System;
using UnityEngine;

namespace Funzilla
{
	internal class Trap : Obstacle, ITrapTriggerListener
	{
		[SerializeField] private TrapTrigger trigger;
		internal void Init(float begin, float end)
		{
			Begin = begin;
			End = end;
			trigger.Listener = this;
		}

		public void OnCollided(Tablet tablet)
		{
			Gameplay.Instance.Player.Stack.BreakTablet(tablet);
		}
	}
}