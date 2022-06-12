using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Funzilla
{
	internal class LevelGenerator : MonoBehaviour
	{
		private const int MaxRaise = 40;
		[SerializeField] private Pattern[] patterns;
		[SerializeField] private Pattern turnPattern;

#if UNITY_EDITOR
		[ContextMenu("Generate Levels")]
		private void GenerateLevels()
		{
			for (var i = 9; i <= 100; i++)
			{
				var levelName = i.ToString("000");
				var level = GeneratePatternedLevel(levelName);
				PrefabUtility.SaveAsPrefabAsset(level.gameObject,
					$"Assets/Game/Gameplay/Resources/Levels/Generated/{levelName}.prefab");
				DestroyImmediate(level.gameObject);
			}
		}
#endif

		private static LevelGenerator _instance;

		private static LevelGenerator Instance
		{
			get
			{
				if (_instance) return _instance;
				_instance = FindObjectOfType<LevelGenerator>();
				return _instance;
			}
		}

		private void Awake()
		{
			_instance = this;
		}

		internal Level GenerateLevel()
		{
			var patternedLevel = GeneratePatternedLevel("PatternedLevel");
			var level = patternedLevel.Make();
			patternedLevel.transform.SetParent(level.transform, false);
			return level;
		}

		private PatternedLevel GeneratePatternedLevel(string levelName)
		{
			var obj = new GameObject(levelName);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.localScale = Vector3.one;
			var level = obj.AddComponent<PatternedLevel>();

			var duration = Random.Range(22.0f, 28.0f);
			var length = duration * Gameplay.NormalSpeed;
			var best = Random.Range(40, 55);
			var current = 1;
			var travel = 0.0f;
			var nTurns = 0;
			var maxRise = Random.Range(10, MaxRaise / 2);

			while (travel < length)
			{
				var pattern = CreatePattern(
					level,
					RandomPattern(patterns),
					current,
					best,
					0,
					maxRise);
				travel += pattern.Length;
				current = pattern.EstimateBest(current);
				if (travel * 4.0f > length && Random.value < 0.5f && nTurns == 0 ||
					travel * 1.5f > length && travel * 1.333f < length && Random.value < 0.3f && nTurns == 1)
				{
					CreatePattern(level, turnPattern, current, best, 0, 0);
					travel += Gameplay.TurnRadius * Mathf.PI * 0.5f;
					nTurns++;
				}

				maxRise = Mathf.Min(maxRise + Random.Range(10, MaxRaise * 2 / 3), MaxRaise);
			}
			return level;
		}

		private Pattern _lastPattern;
		private Pattern RandomPattern(IReadOnlyList<Pattern> list)
		{
			while (true)
			{
				var pattern = list[Random.Range(0, list.Count)];
				if (pattern == _lastPattern && list.Count > 1) continue;
				_lastPattern = pattern;
				return pattern;
			}
		}

		private static Pattern CreatePattern(Component level, Pattern pattern, int current, int expect, int seed, int maxRaise)
		{
			var copy = Instantiate(pattern, level.transform);
			copy.name = pattern.name;
			copy.Randomize(current, expect, seed, maxRaise);
			return copy;
		}

		[SerializeField] private EditTrapSphere ballTrap;
		[SerializeField] private EditDiamond diamonSequence;
		[SerializeField] private EditGate1 gate1;
		[SerializeField] private EditGate2 gate2;
		[SerializeField] private EditHeight heightChange;
		[SerializeField] private EditJump jump;
		[SerializeField] private EditLava lava;
		[SerializeField] private EditTrapLying lyingTrap;
		[SerializeField] private EditPit pit;
		[SerializeField] private EditRotate rotate;
		[SerializeField] private EditTrapSaw saw;
		[SerializeField] private EditSpeedup speedup;
		[SerializeField] private EditSwing swing;
		[SerializeField] private EditTrapStanding tallTrap;
		[SerializeField] private EditTurn turn;
		[SerializeField] private EditWall wall;

		private static T NewObject<T>(T prefab, string name, Transform t) where T : Object
		{
#if UNITY_EDITOR
			var p = EditorApplication.isPlayingOrWillChangePlaymode ? prefab :
				AssetDatabase.LoadAssetAtPath<T>($"Assets/Game/Gameplay/LevelDesign/{name}.prefab");
			return Instantiate(p, t);
#else
			return Instantiate(prefab, t);
#endif
		}

		internal static EditTrapSphere NewBallTrap(Transform t)
		{
			return NewObject(Instance.ballTrap, "BallTrap", t);
		}

		internal static EditDiamond NewDiamondSequence(Transform t)
		{
			return NewObject(Instance.diamonSequence, "Diamond", t);
		}

		internal static EditGate1 NewGate1(Transform t)
		{
			return NewObject(Instance.gate1, "Gate1", t);
		}

		internal static EditGate2 NewGate2(Transform t)
		{
			return NewObject(Instance.gate2, "Gate2", t);
		}

		internal static EditHeight NewHeightChange(Transform t)
		{
			return NewObject(Instance.heightChange, "Height", t);
		}

		internal static EditJump NewJump(Transform t)
		{
			return NewObject(Instance.jump, "Jump", t);
		}

		internal static EditHole NewLava(Transform t)
		{
			return NewObject(Instance.lava, "Lava", t);
		}

		internal static EditTrapLying NewLyingTrap(Transform t)
		{
			return NewObject(Instance.lyingTrap, "LyingTrap", t);
		}

		internal static EditHole NewPit(Transform t)
		{
			return NewObject(Instance.pit, "Pit", t);
		}

		internal static EditHole NewHole(Transform t)
		{
			return Random.Range(0, 2) == 0 ? NewLava(t)  : NewPit(t);
		}

		internal static EditRotate NewRotate(Transform t)
		{
			return NewObject(Instance.rotate, "Rotate", t);
		}

		internal static EditTrapSaw NewSaw(Transform t)
		{
			return NewObject(Instance.saw, "SawTrap", t);
		}

		internal static EditSpeedup NewSpeedup(Transform t)
		{
			return NewObject(Instance.speedup, "Speedup", t);
		}

		internal static EditSwing NewSwing(Transform t)
		{
			return NewObject(Instance.swing, "Swing", t);
		}

		internal static EditTrapStanding NewTallTrap(Transform t)
		{
			return NewObject(Instance.tallTrap, "TallTrap", t);
		}

		internal static EditTurn NewTurn(Transform t)
		{
			return NewObject(Instance.turn, "Turn", t);
		}

		internal static EditWall NewWall(Transform t)
		{
			return NewObject(Instance.wall, "Wall", t);
		}
	}
}