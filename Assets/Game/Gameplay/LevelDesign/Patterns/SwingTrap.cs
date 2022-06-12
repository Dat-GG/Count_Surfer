using UnityEngine;

namespace Funzilla
{
	internal class SwingTrap : Pattern
	{
		private const int DiamondSpacing = 5;
		[SerializeField] private int height = 2;
		[SerializeField] private int width = 2;
		[SerializeField] private int coinAmount = 5;

		internal override void Populate(Level level)
		{
			var sequence = LevelGenerator.NewDiamondSequence(level.transform);
			sequence.amount = coinAmount;
			sequence.spacing = DiamondSpacing;
			var swing = LevelGenerator.NewSwing(level.transform);
			swing.spacing = Spacing;
			swing.laneL = 2;
			swing.laneR = 4;
			var parent = swing.transform;
			for (var i = 2; i <= 4; i++)
			{
				var trap = LevelGenerator.NewTallTrap(parent);
				trap.tall = height;
				trap.lane = i;
				trap.spacing = 0;
			}
		}

		internal override void Randomize(int current, int expect, int seed, int maxRaise)
		{
			height = Random.Range(2, 7);
			coinAmount = Random.Range(5, 10);
		}

		internal override float Length =>
			Spacing + DiamondSequence.CalculateLength(coinAmount, DiamondSpacing);
	}
}