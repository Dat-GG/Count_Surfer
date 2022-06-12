using UnityEngine;

namespace Funzilla
{
	internal class WallRandom : Gate2Pattern
	{
		[SerializeField] private int height;

		internal override void Populate(Level level)
		{
			base.Populate(level);
			var trap = LeftOrRight.SpawnTrap(level.transform, height, 1, Gameplay.LaneCount);
			trap.spacing = Spacing + Wall.CalculateSpacing(height);
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			var min = Mathf.CeilToInt(seed * 0.01f + 2);
			var max = Mathf.CeilToInt(seed * 0.01f + 4);
			height = Random.Range(min, max + 1);
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
			return height * TabletsPerUnit + 1;
		}

		internal override int EstimateBest(int current)
		{
			return EstimateBestMath(current) - EstimateLoss();
		}
		internal override float Length =>
			GateSpacing + Spacing +
			Wall.CalculateSpacing(height);
	}
}