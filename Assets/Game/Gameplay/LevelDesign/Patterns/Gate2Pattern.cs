using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Funzilla
{
	internal class Gate2Pattern : Pattern
	{
		private const int MathTolerence = 4;

		[SerializeField] internal MathType typeL = MathType.Add;
		[SerializeField] internal int numberL = 5;
		[SerializeField] internal MathType typeR = MathType.Multiply;
		[SerializeField] internal int numberR = 2;

		internal override void Populate(Level level)
		{
			if (numberL == 0 || (typeL == MathType.Multiply && numberL == 1))
			{
				SpawnGate1(level.transform, typeR, numberR, -1);
				return;
			}
			if (numberR == 0 || (typeR == MathType.Multiply && numberR == 1))
			{
				SpawnGate1(level.transform, typeL, numberL, 1);
				return;
			}
			var gate = LevelGenerator.NewGate2(level.transform);
			gate.typeL = typeL;
			gate.numberL = numberL;
			gate.typeR = typeR;
			gate.numberR = numberR;
			gate.spacing = GateSpacing;
		}

		protected void RandomizeMath(int input, int best)
		{
			var worst = Mathf.Max(1, Random.Range(best / 4, 3 * best / 4));
			if (worst == best) worst--;
			if (Random.Range(0, 2) == 0)
			{
				RandomizeMath(input, worst, out typeL, out numberL);
				RandomizeMath(input, best, out typeR, out numberR);
			}
			else
			{
				RandomizeMath(input, worst, out typeR, out numberR);
				RandomizeMath(input, best, out typeL, out numberL);
			}
		}

		protected int EstimateBestMath(int current)
		{
			return Mathf.Max(
				EstimateOutcome(current, typeL, numberL),
				EstimateOutcome(current, typeR, numberR));
		}

		private static void SpawnGate1(Transform parent, MathType type, int number, float x)
		{
			var gate = LevelGenerator.NewGate1(parent);
			gate.type = type;
			gate.number = number;
			gate.spacing = 10;
			gate.x = x;
		}

		internal static int EstimateOutcome(int current, MathType type, int number)
		{
			return type switch
			{
				MathType.Add => current + number,
				MathType.Substract => current - number,
				MathType.Multiply => current * number,
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
		}

		internal static void RandomizeMath(int input, int raise, out MathType type, out int number)
		{
			if (raise < 0)
			{
				type = MathType.Substract;
				number = -raise;
				return;
			}

			if (raise == 0)
			{
				type = MathType.Multiply;
				number = 1;
				return;
			}

			var x = raise / input;
			if (Random.Range(0, 2) == 1 && x > 1 && raise - input * x <= MathTolerence)
			{
				type = MathType.Multiply;
				number = x;
				return;
			}

			type = MathType.Add;
			number = raise;
		}
	}
}