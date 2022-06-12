using UnityEngine;

namespace Funzilla
{
	internal class PitSide : Pattern
	{
		[SerializeField] private int length = 5;
		internal override void Populate(Level level)
		{
			var hole = LevelGenerator.NewHole(level.transform);
			hole.length = length;
			hole.laneL = 1;
			hole.laneR = 2;
			hole.spacing = 0;
			hole = LevelGenerator.NewHole(hole.transform);
			hole.length = length;
			hole.laneL = 4;
			hole.laneR = 5;
			hole.spacing = 0;
			var diamondSequence = LevelGenerator.NewDiamondSequence(hole.transform);
			diamondSequence.lane = 3;
			diamondSequence.amount = length / DiamondSequence.Spacing;
			diamondSequence.spacing = Spacing;
		}

		internal override void Randomize(int current, int expect, int seed, int maxRaise)
		{
			length = Random.Range(7, 13);
		}
		internal override float Length => length + Spacing;
	}
}