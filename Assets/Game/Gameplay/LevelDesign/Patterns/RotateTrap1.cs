using UnityEngine;

namespace Funzilla
{
	internal class RotateTrap1 : Pattern
	{
		private const int DiamondSpacing = 5;
		[SerializeField] private int height = 2;
		[SerializeField] private int coinAmount = 5;

		internal override void Populate(Level level)
		{
			var sequence = LevelGenerator.NewDiamondSequence(level.transform);
			sequence.amount = coinAmount;
			sequence.spacing = DiamondSpacing;
			sequence.lane = Random.Range(1, Gameplay.LaneCount + 1);
			var rotate = LevelGenerator.NewRotate(level.transform);
			rotate.spacing = Spacing + Wall.CalculateSpacing(height);
			var trapL = LevelGenerator.NewTallTrap(rotate.transform);
			trapL.lane = 1;
			trapL.tall = height;
			trapL.spacing = 0;
			var trapR = LevelGenerator.NewTallTrap(rotate.transform);
			trapR.lane = Gameplay.LaneCount - 1;
			trapR.tall = height;
			trapR.spacing = 2;
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