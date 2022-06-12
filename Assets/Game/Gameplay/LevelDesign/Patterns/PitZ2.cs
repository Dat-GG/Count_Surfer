using UnityEngine;

namespace Funzilla
{
	internal class PitZ2 : Gate2Pattern
	{
		private const int MiddleLength = 5;
		private const int MinLength = MiddleLength * 3 + MiddleLength * 2;
		private const int MaxLength = MinLength;

		[SerializeField] private int length;

		internal override void Populate(Level level)
		{
			base.Populate(level);
			var type = Random.Range(0, 2);
			var side = Random.Range(0, 2);
			var l = Mathf.Clamp(length, MinLength, MaxLength);
			var l1 = (l - MiddleLength * 2) / 3;
			var l3 = (l - MiddleLength * 2 - l1) / 2;
			var l5 = l - l3 - l1 - MiddleLength * 2;

			var hole1 = NewHole(type, l1, 0, level.transform);
			var hole2 = NewHole(type, MiddleLength, 0, level.transform);
			var hole3 = NewHole(type, l3, 0, level.transform);
			var hole4 = NewHole(type, MiddleLength, 0, level.transform);
			var hole5 = NewHole(type, l5, PitSpacing, level.transform);
			hole2.laneL = hole4.laneL = 1;
			hole2.laneR = hole4.laneR = 5;
			if (side == 0)
			{
				hole1.laneL = hole5.laneL = 1;
				hole1.laneR = hole5.laneR = 3;
				hole3.laneL = 3;
				hole3.laneR = 5;
			}
			else
			{
				hole1.laneL = hole5.laneL = 3;
				hole1.laneR = hole5.laneR = 5;
				hole3.laneL = 1;
				hole3.laneR = 3;
			}

			var diamondSequence = LevelGenerator.NewDiamondSequence(hole1.transform);
			diamondSequence.lane = side == 0 ? 5 : 1;
			diamondSequence.amount = l1 / DiamondSequence.Spacing;
			diamondSequence.spacing = 0;

			diamondSequence = LevelGenerator.NewDiamondSequence(hole3.transform);
			diamondSequence.lane = side == 0 ? 1 : 5;
			diamondSequence.amount = l3 / DiamondSequence.Spacing;
			diamondSequence.spacing = 0;

			diamondSequence = LevelGenerator.NewDiamondSequence(hole5.transform);
			diamondSequence.lane = side == 0 ? 5 : 1;
			diamondSequence.amount = l5 / DiamondSequence.Spacing;
			diamondSequence.spacing = 0;
		}

		private static EditHole NewHole(int type, int length, int spacing, Transform parent)
		{
			var hole = type == 0 ? LevelGenerator.NewLava(parent) : LevelGenerator.NewPit(parent);
			hole.length = length;
			hole.spacing = spacing;
			return hole;
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			length = Random.Range(MinLength, MaxLength + 1);
			while (true)
			{
				var loss = EstimateLoss();
				var best = Mathf.Min(
					expect - input + loss,
					maxRaise);
				RandomizeMath(input, best);
				if (EstimateBest(input) > 2)
				{
					break;
				}
				maxRaise += TabletsPerUnit;
			}
		}

		private static int EstimateLoss()
		{
			return EditHole.EstimateLoss(MiddleLength + 1) * 2;
		}

		internal override int EstimateBest(int input)
		{
			return EstimateBestMath(input) - EstimateLoss();
		}
		internal override float Length => length + PitSpacing + GateSpacing;
	}
}