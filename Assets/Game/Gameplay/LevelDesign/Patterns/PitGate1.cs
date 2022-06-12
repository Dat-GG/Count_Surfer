using UnityEngine;

namespace Funzilla
{
	internal class PitGate1 : Pattern
	{
		[SerializeField] private int length = 10;
		[SerializeField] private bool left;
		[SerializeField] private MathType type;
		[SerializeField] private int number;
		internal override void Populate(Level level)
		{
			var hole = LevelGenerator.NewHole(level.transform);
			hole.length = length;
			if (left)
			{
				hole.laneL = 1;
				hole.laneR = 3;
			}
			else
			{
				hole.laneL = 3;
				hole.laneR = Gameplay.LaneCount;
			}

			if (Gate2Pattern.EstimateOutcome(1, type, number) != 1)
			{
				var gate = LevelGenerator.NewGate1(hole.transform);
				gate.number = number;
				gate.type = type;
				gate.spacing = 5;
				gate.x = Gameplay.CalculatePosition(hole.laneL, hole.laneR);
			}
			hole.spacing = PitSpacing;
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			length = Random.Range(7, 13);
			left = Random.Range(0, 2) == 1;
			var best = Mathf.Min(expect - input, maxRaise);
			Gate2Pattern.RandomizeMath(input, best, out type, out number);
		}

		internal override int EstimateBest(int input)
		{
			return Gate2Pattern.EstimateOutcome(input, type, number);
		}

		internal override float Length => length + GateSpacing + PitSpacing;
	}
}