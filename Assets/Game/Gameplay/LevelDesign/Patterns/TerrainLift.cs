using UnityEngine;

namespace Funzilla
{
	internal class TerrainLift : Gate2Pattern
	{
		private const int HeightSpacing = 2;
		[SerializeField] private int height = 1;
		[SerializeField] private int coinAmount = 5;

		internal override void Populate(Level level)
		{
			base.Populate(level);
			var h1 = LevelGenerator.NewHeightChange(level.transform);
			h1.height = height;
			h1.spacing = HeightSpacing;
			var sequence = LevelGenerator.NewDiamondSequence(level.transform);
			sequence.amount = coinAmount;
			sequence.spacing = 0;
			var h2 = LevelGenerator.NewHeightChange(level.transform);
			h2.height = -height;
			h2.spacing = HeightSpacing + Spacing;
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			coinAmount = Random.Range(4, 10);
			var min = Mathf.CeilToInt(seed * 0.01f + 2);
			var max = Mathf.CeilToInt(seed * 0.01f + 5);

			height = Random.Range(min, max + 1);
			var best = Mathf.Min(
				expect - input + height * TabletsPerUnit,
				maxRaise);
			RandomizeMath(input, best);

			while (EstimateBest(input) < 2 && height > 1)
			{
				height--;
			}
		}

		internal override int EstimateBest(int current)
		{
			return EstimateBestMath(current) - height * TabletsPerUnit;
		}

		internal override float Length =>
			GateSpacing + Spacing + HeightSpacing * 2 +
			DiamondSequence.CalculateLength(coinAmount, 0);
	}
}