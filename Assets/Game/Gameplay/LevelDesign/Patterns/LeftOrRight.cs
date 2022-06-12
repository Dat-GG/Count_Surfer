using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Funzilla
{
	internal class LeftOrRight : Gate2Pattern
	{
		private const int TrapSpacing = 7;

		[SerializeField] private int count;
		[SerializeField] private int hmin;
		[SerializeField] private int hmax;
		[SerializeField] private int difficulty;

		internal override void Populate(Level level)
		{
			base.Populate(level);
			var side = Random.Range(0, 2);
			EditObject obj = null;
			for (var i = 0; i < count; i++)
			{
				if (side == 0)
				{
					obj = SpawnTrap(level.transform, hmax, 1, difficulty);
					obj.spacing = TrapSpacing;
					if (hmin > 0)
					{
						SpawnTrap(obj.transform, hmin, difficulty + 1, Gameplay.LaneCount);
					}
				}
				else
				{
					var laneR = Gameplay.LaneCount - difficulty;
					obj = SpawnTrap(level.transform, hmax, laneR + 1, Gameplay.LaneCount);
					obj.spacing = TrapSpacing;
					if (hmin > 0)
					{
						SpawnTrap(obj.transform, hmin, 1, laneR);
					}
				}

				side = (side + 1) % 2;
			}

			if (obj is { }) obj.spacing = Spacing;
		}

		internal override void Randomize(int input, int expect, int seed, int maxRaise)
		{
			count = Random.Range(3, 8);
			hmin = Random.Range(0, 2);
			hmax = Random.Range(hmin + 2, hmin + 4);
			var best = Mathf.Min(
				expect - input + EstimateLoss(),
				maxRaise);
			RandomizeMath(input, best);
			while (EstimateBest(input) < 2 && count > 1)
			{
				count--;
			}
			while (EstimateBest(input) < 2 && hmin > 0)
			{
				hmin--;
			}
			difficulty = 3;
		}

		internal override int EstimateBest(int input)
		{
			return EstimateBestMath(input) - EstimateLoss();
		}

		private int EstimateLoss()
		{
			return hmin <= 0 ? 0 : count * (hmin * TabletsPerUnit + 1);
		}

		internal override float Length =>
			GateSpacing + count * TrapSpacing + Spacing;

		private enum TrapType
		{
			Lying, Wall, Standing, Saw, Sphere
		}

		private static readonly TrapType[] Traps = new[]
		{
			TrapType.Lying, TrapType.Wall, TrapType.Standing
		};

		private static readonly TrapType[] DynamicTraps = new[]
		{
			TrapType.Lying, TrapType.Standing
		};

		internal static EditObject SpawnTrap(Transform parent, int height, int laneL, int laneR, bool dynamic = false)
		{
			var traps = dynamic ? DynamicTraps : Traps;
			var type = traps[Random.Range(0, traps.Length)];
			switch (type)
			{
				case TrapType.Lying:
				{
					EditTrapLying trap = null;
					for (var i = 0; i < height; i++)
					{
						trap = LevelGenerator.NewLyingTrap(parent);
						trap.height = i;
						trap.laneL = laneL;
						trap.laneR = laneR;
						trap.spacing = 0;
						parent = trap.transform;
					}
					return trap;
				}
				case TrapType.Wall:
				{
					var wall = LevelGenerator.NewWall(parent);
					wall.height = height;
					wall.laneL = laneL;
					wall.laneR = laneR;
					wall.spacing = 0;
					return wall;
				}
				case TrapType.Standing:
				{
					EditTrapStanding trap = null;
					for (var i = laneL; i <= laneR; i++)
					{
						trap = LevelGenerator.NewTallTrap(parent);
						trap.tall = height;
						trap.lane = i;
						trap.spacing = 0;
						parent = trap.transform;
					}
					return trap;
				}
				case TrapType.Saw:
				{
					var x = Gameplay.CalculatePosition(laneL, laneR);
					EditTrapSaw trap = null;
					for (var i = 0; i < height; i++)
					{
						trap = LevelGenerator.NewSaw(parent);
						trap.height = i;
						trap.x = x;
						trap.spacing = 0;
						parent = trap.transform;
					}
					return trap;
				}
				case TrapType.Sphere:
				{
					var ball = LevelGenerator.NewBallTrap(parent);
					ball.x = Gameplay.CalculatePosition(laneL, laneR);
					ball.spacing = 0;
					return ball;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}