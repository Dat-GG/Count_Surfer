using UnityEngine;

namespace Funzilla
{
	internal class TrapSpeed : Pattern
	{
		[SerializeField] private int lane;

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