using UnityEngine;

namespace Funzilla
{
	internal class PitRandom : Pattern
	{
		[SerializeField] private int length = 5;

		internal override void Populate(Level level)
		{
			var hole = LevelGenerator.NewHole(level.transform);
			hole.length = length;
			hole.spacing = Spacing;
			var diamondSequence = LevelGenerator.NewDiamondSequence(hole.transform);
			diamondSequence.amount = length / DiamondSequence.Spacing;
			diamondSequence.spacing = 0;
			if (Random.Range(0, 2) == 0)
			{
				hole.laneL = 1;
				hole.laneR = 4;
				diamondSequence.lane = 5;
			}
			else
			{
				hole.laneL = 2;
				hole.laneR = 5;
				diamondSequence.lane = 1;
			}
		}

		internal override void Randomize(int current, int expect, int seed, int maxRaise)
		{
			length = Random.Range(7, 14);
		}
		internal override float Length => length + Spacing;
	}
}