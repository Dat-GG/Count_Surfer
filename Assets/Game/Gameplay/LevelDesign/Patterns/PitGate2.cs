using UnityEngine;

namespace Funzilla
{
	internal class PitGate2 : Gate2Pattern
	{
		[SerializeField] private int length = 10;
		internal override void Populate(Level level)
		{
			base.Populate(level);
			var hole = LevelGenerator.NewHole(level.transform);
			hole.length = length;
			hole.laneL = 1;
			hole.laneR = Gameplay.LaneCount;
			hole.spacing = PitSpacing;
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			length = Random.Range(7, 13);
			while (true)
			{
				var best = Mathf.Min(
					expect - input + EditHole.EstimateLoss(length),
					maxRaise);
				RandomizeMath(input, best);
				if (EstimateBest(input) > 2)
				{
					break;
				}
				length--;
			}
		}

		internal override int EstimateBest(int input)
		{
			return EstimateBestMath(input) - EditHole.EstimateLoss(length);
		}

		internal override float Length => length + PitSpacing + GateSpacing;
	}
}