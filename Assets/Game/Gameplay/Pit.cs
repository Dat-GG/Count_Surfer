using UnityEngine;

namespace Funzilla
{
	internal class Pit : Obstacle, ITrapTriggerListener
	{
		[SerializeField] private TrapTrigger trigger;
		internal void Init(EditPit info, float begin)
		{
			Begin = begin;
			End = begin + info.length + info.spacing;
			var x = Gameplay.CalculatePosition(info.laneL, info.laneR);
			var w = Gameplay.CalculateWidth(info.laneL, info.laneR);
			var t = trigger.transform;
			t.localPosition = new Vector3(x, -Gameplay.RoadHeight * 0.5f, info.length * 0.5f);
			t.localScale = new Vector3(
				w - Gameplay.TrapTriggerSpacing,
				Gameplay.RoadHeight + 0.01f,
				info.length - Gameplay.TrapTriggerSpacing);
			trigger.Listener = this;
		}

		public void OnCollided(Tablet tablet)
		{
			Gameplay.Instance.Player.Stack.Discard(tablet);
			SoundManager.Instance.PlaySfx("CubeCollect1");
		}
	}
}