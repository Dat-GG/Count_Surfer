using UnityEngine;

namespace Funzilla
{
	internal class Pattern : MonoBehaviour
	{
		internal const int TabletsPerUnit = 5;
		internal const int GateSpacing = 8;
		internal const int Spacing = 12;
		internal const int PitSpacing = 5;

		internal virtual void Populate(Level level)
		{
		}

		internal virtual void Randomize(int input, int expect, int seed, int maxRaise)
		{
		}

		internal virtual int EstimateBest(int current)
		{
			return current;
		}

		internal virtual float Length => Spacing;
	}
}