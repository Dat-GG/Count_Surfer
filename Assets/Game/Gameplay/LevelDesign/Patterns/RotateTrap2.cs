using UnityEngine;

namespace Funzilla
{
	internal class RotateTrap2 : Pattern
	{
		private const int DiamondSpacing = 5;
		[SerializeField] private int height = 1;
		[SerializeField] private int coinAmount = 5;

		internal override void Populate(Level level)
		{
			var sequence = LevelGenerator.NewDiamondSequence(level.transform);
			sequence.amount = coinAmount;
			sequence.spacing = DiamondSpacing;
			sequence.lane = Random.Range(1, Gameplay.LaneCount + 1);
			var rotate = LevelGenerator.NewRotate(level.transform);
			rotate.spacing = Spacing + Wall.CalculateSpacing(height);

			var parent = rotate.transform;
			for (var i = 0; i < height; i++)
			{
				var trap = LevelGenerator.NewLyingTrap(parent);
				trap.height = i;
				trap.laneL = 1;
				trap.laneR = Gameplay.LaneCount - 1;
				trap.spacing = 0;
				parent = trap.transform;
			}
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			height = Random.Range(2, 5);
			coinAmount = Random.Range(5, 10);
		}

		internal override float Length =>
			Spacing + Wall.CalculateSpacing(height) +
			DiamondSequence.CalculateLength(coinAmount, DiamondSpacing);
	}
}