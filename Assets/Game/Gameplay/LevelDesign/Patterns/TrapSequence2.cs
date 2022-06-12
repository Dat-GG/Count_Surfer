using UnityEngine;

namespace Funzilla
{
	internal class TrapSequence2 : Pattern
	{
		[SerializeField] private int count;

		internal override void Populate(Level level)
		{
		}

		internal override void Randomize(int current, int expect, int seed, int maxRaise)
		{
		}

		internal override int EstimateBest(int current)
		{
			return 0;
		}
		internal override float Length => 0.0f;
	}
}