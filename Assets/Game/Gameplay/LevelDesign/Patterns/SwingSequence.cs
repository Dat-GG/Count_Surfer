using UnityEngine;

namespace Funzilla
{
	internal class SwingSequence : Pattern
	{
		private const int Gap = 3;
		[SerializeField] private int height = 4;
		[SerializeField] private int count = 3;

		internal override void Populate(Level level)
		{
			var side = Random.Range(0, 2) == 0;
			EditSwing swing = null;
			for (var i = 0; i < count; i++)
			{
				swing = LevelGenerator.NewSwing(level.transform);
				swing.spacing = Gap;
				swing.left = side;
				var trap = LevelGenerator.NewTallTrap(swing.transform);
				trap.spacing = 0;
				trap.lane = 3;
				trap.tall = height;
				side = !side;
			}

			if (swing is { }) swing.spacing = Spacing;
		}

		internal override void Randomize(int current, int expect, int seed, int maxRaise)
		{
			count = Random.Range(3, 5);
			height = Random.Range(2, 5);
		}
		internal override float Length => count * Gap + Spacing;
	}
}