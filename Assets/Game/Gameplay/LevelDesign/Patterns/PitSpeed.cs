using UnityEngine;

namespace Funzilla
{
	internal class PitSpeed : Gate2Pattern
	{
		[SerializeField] private int length = 5;

		internal override void Populate(Level level)
		{
			base.Populate(level);
			var speedup = LevelGenerator.NewSpeedup(level.transform);
			speedup.spacing = 0;
			speedup.x = Random.Range(0, 3) switch
			{
				0 => -1.5f,
				1 => -0,
				_ => +1.5f
			};

			var hole = LevelGenerator.NewHole(level.transform);
			hole.length = length;
			hole.spacing = PitSpacing;
			hole.laneL = 1;
			hole.laneR = Gameplay.LaneCount;

			var diamondSequence = LevelGenerator.NewDiamondSequence(hole.transform);
			diamondSequence.lane = Random.Range(1, Gameplay.LaneCount + 1);
			diamondSequence.amount = length / DiamondSequence.Spacing;
			diamondSequence.spacing = 0;
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			length = Random.Range(9, 15);
			while (true)
			{
				var loss = EditHole.EstimateLossSpeed(length);
				var best = Mathf.Min(
					expect - input + loss,
					maxRaise);
				RandomizeMath(input, best);
				if (EstimateBest(input) > 0)
				{
					break;
				}
				length--;
			}
		}

		internal override int EstimateBest(int input)
		{
			return EstimateBestMath(input) - EditHole.EstimateLossSpeed(length);
		}
		internal override float Length => length + Speedup.Length + PitSpacing + GateSpacing;
	}
}