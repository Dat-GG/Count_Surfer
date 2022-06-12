using UnityEngine;

namespace Funzilla
{
	internal class SplitTrap1 : Gate2Pattern
	{
		[SerializeField] private int count = 2;

		internal override void Populate(Level level)
		{
			base.Populate(level);
			var parent = level.transform;
			var spacing = Wall.CalculateSpacing(count * 2) + Spacing;
			for (var i = 0; i < count; i++)
			{
				var trap = LevelGenerator.NewLyingTrap(parent);
				trap.spacing = spacing;
				trap.height = i * 2;
				trap.laneL = 1;
				trap.laneR = Gameplay.LaneCount;
				parent = trap.transform;
			}
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			count = Random.Range(2, 4);
			while (true)
			{
				var best = Mathf.Min(
					expect - input + (count * 2 - 1) * (TabletsPerUnit + 1),
					maxRaise);
				RandomizeMath(input, best);
				if (EstimateBest(input) - (count - 1) * (TabletsPerUnit + 1) > 2)
				{
					break;
				}
				maxRaise += TabletsPerUnit;
			}
		}

		internal override int EstimateBest(int input)
		{
			return EstimateBestMath(input) - count * (TabletsPerUnit + 1);
		}
		internal override float Length => GateSpacing + Spacing + Wall.CalculateSpacing(count * 2);
	}
}