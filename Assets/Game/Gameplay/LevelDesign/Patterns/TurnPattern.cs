
using UnityEngine;

namespace Funzilla
{
	internal class TurnPattern : Pattern
	{
		[SerializeField] internal bool left;
		internal override void Populate(Level level)
		{
			var turn = LevelGenerator.NewTurn(level.transform);
			turn.left = left;

			var sequence = LevelGenerator.NewDiamondSequence(level.transform);
			sequence.amount = Random.Range(5, 10);
			sequence.lane = Random.Range(1, Gameplay.LaneCount + 1);
			var spacing = Gameplay.TurnRadius * Mathf.PI * 0.5f - DiamondSequence.CalculateLength(sequence.amount, 0);
			sequence.spacing = Mathf.Max(0, Mathf.FloorToInt(spacing));
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			left = Random.Range(0, 2) == 0;
		}
	}
}