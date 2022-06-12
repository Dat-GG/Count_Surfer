using UnityEngine;

namespace Funzilla
{
	internal class Wall32 : Gate2Pattern
	{
		[SerializeField] private int hmin;
		[SerializeField] private int hmax;

		internal override void Populate(Level level)
		{
			base.Populate(level);
			EditObject trap;
			if (Random.Range(0, 2) == 0)
			{
				trap = LeftOrRight.SpawnTrap(level.transform, hmin, 1, Gameplay.LaneCount);
				int laneL, laneR;

				if (Random.Range(0, 2) == 0)
				{
					laneL = 1;
					laneR = 3;
				}
				else
				{
					laneL = 3;
					laneR = 5;
				}

				var parent = trap.transform;
				for (var i = hmin + 1; i <= hmax; i++)
				{
					var t = LevelGenerator.NewLyingTrap(parent);
					t.height = i - 1;
					t.laneL = laneL;
					t.laneR = laneR;
					t.spacing = 0;
					parent = t.transform;
				}
			}
			else
			{
				if (Random.Range(0, 2) == 0)
				{
					trap = LeftOrRight.SpawnTrap(level.transform, hmin, 1, 2);
					trap = LeftOrRight.SpawnTrap(trap.transform, hmax, 3, 5);
				}
				else
				{
					trap = LeftOrRight.SpawnTrap(level.transform, hmax, 1, 3);
					trap = LeftOrRight.SpawnTrap(trap.transform, hmin, 4, 5);
				}
			}

			trap.spacing = Spacing + Wall.CalculateSpacing(hmax);
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			var min = Mathf.CeilToInt(seed * 0.01f + 2);
			var max = Mathf.CeilToInt(seed * 0.01f + 4);
			hmin = Random.Range(min, max + 1);
			hmax = hmin + Random.Range(1, 4);
			while (true)
			{
				var best = Mathf.Min(
					expect - input + EstimateLoss(),
					maxRaise);
				RandomizeMath(input, best);
				if (EstimateBest(input) > 2)
				{
					break;
				}

				maxRaise += TabletsPerUnit;
			}
		}

		private int EstimateLoss()
		{
			return hmin * TabletsPerUnit + 1;
		}

		internal override int EstimateBest(int input)
		{
			return EstimateBestMath(input) - hmin * TabletsPerUnit;
		}
		internal override float Length => GateSpacing + Spacing + Wall.CalculateSpacing(hmax);
	}
}