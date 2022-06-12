using UnityEngine;

namespace Funzilla
{
	internal class SplitTrap2 : Gate2Pattern
	{
		[SerializeField] private int count;

		internal override void Populate(Level level)
		{
			base.Populate(level);
			var parent = level.transform;
			var spacing = Wall.CalculateSpacing(count * 2) + Spacing;
			for (var i = 0; i < 2; i++)
			{
				var trap = LevelGenerator.NewTallTrap(parent);
				trap.spacing = spacing;
				trap.tall = count * 2 - 1;
				trap.lane = i == 0 ? 1 : Gameplay.LaneCount;
				parent = trap.transform;
			}
			for (var i = 0; i < count; i++)
			{
				var trap = LevelGenerator.NewLyingTrap(parent);
				trap.spacing = spacing;
				trap.height = i * 2;
				trap.laneL = 2;
				trap.laneR = Gameplay.LaneCount - 1;
				parent = trap.transform;
			}
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			count = Random.Range(2, 4);
			while (true)
			{
				var best = Mathf.Min(
					expect - input + (count * 2 - 1) * TabletsPerUnit,
					maxRaise);
				RandomizeMath(input, best);
				if (EstimateBest(input) - (count - 1) * TabletsPerUnit > 2)
				{
					break;
				}
				maxRaise += TabletsPerUnit;
			}
		}

		internal override int EstimateBest(int input)
		{
			return EstimateBestMath(input) - count * TabletsPerUnit;
		}
		internal override float Length => GateSpacing + Spacing + Wall.CalculateSpacing(count * 2);
	}
}