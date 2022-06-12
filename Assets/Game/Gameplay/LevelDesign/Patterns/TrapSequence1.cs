using UnityEngine;

namespace Funzilla
{
	internal class TrapSequence1 : Gate2Pattern
	{
		private const int TrapSpacing = 7;
		[SerializeField] private int count;

		internal override void Populate(Level level)
		{
			base.Populate(level);
			EditTrapLying trap = null;
			for (var i = 0; i < count; i++)
			{
				trap = LevelGenerator.NewLyingTrap(level.transform);
				trap.laneL = 1;
				trap.laneR = Gameplay.LaneCount;
				trap.height = 0;
				trap.spacing = TrapSpacing;
			}

			if (trap) trap.spacing = Spacing;
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			count = Random.Range(4, 8);
			var best = Mathf.Min(
				expect - input + count * (TabletsPerUnit + 1),
				maxRaise);
			RandomizeMath(input, best);
			while (EstimateBest(input) < 2 && count > 1)
			{
				count--;
			}
		}

		internal override int EstimateBest(int input)
		{
			return EstimateBestMath(input) - count * (TabletsPerUnit + 1);
		}
		internal override float Length => Spacing + GateSpacing + TrapSpacing * count;
	}
}