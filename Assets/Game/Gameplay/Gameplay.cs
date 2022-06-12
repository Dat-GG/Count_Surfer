
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Funzilla
{
	internal class Gameplay : Scene
	{
		[SerializeField] private Player player;
		[SerializeField] private Text moneyText;
		[SerializeField] private Text levelText;
		[SerializeField] private Text tabletNumberText;
		[SerializeField] private Image moneyImage;
		[SerializeField] private Button restartButton;
		[SerializeField] private Button prevButton;
		[SerializeField] private Button nextButton;
		[SerializeField] private Slider progressLevelSlider;
		[SerializeField] private Finish finish;
		[SerializeField] private Theme[] themes;
		[SerializeField] private GameObject winCamera;
		[SerializeField] private GameObject mainCamera;
		[SerializeField] private GameObject cameraController;
		[SerializeField] private GameObject confettiBlastRainbow;
		[SerializeField] private LevelGenerator levelGenerator;
		[SerializeField] private string[] levels;

		internal static Gameplay Instance;
		internal Player Player => player;
		internal Finish Finish => finish;
		internal Level Level { get; private set; }
		internal static int CoinAmountEarned;

		private enum State
		{
			None, Initializing, Initialized, Playing, Win, Lost
		}

		private State _state = State.None;
		internal bool Playing => _state == State.Playing;

		private void Awake()
		{
			Instance = this;
			restartButton.onClick.AddListener(() =>
			{
				SceneManager.Instance.CloseScene(SceneID.Home);
				SceneManager.Instance.ReloadScene(SceneID.Gameplay);
			});
			prevButton.onClick.AddListener(() =>
			{
				Profile.Instance.Level--;
				SceneManager.Instance.CloseScene(SceneID.Home);
				SceneManager.Instance.ReloadScene(SceneID.Gameplay);
			});
			nextButton.onClick.AddListener(() =>
			{
				Profile.Instance.Level++;
				SceneManager.Instance.CloseScene(SceneID.Home);
				SceneManager.Instance.ReloadScene(SceneID.Gameplay);
			});
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (_state == State.Initializing) return;
			_state = State.Initializing;
			SceneManager.Instance.OpenScene(SceneID.Home);
			SceneManager.Instance.HideSplash();
			SceneManager.Instance.HideLoading();
			if (Profile.Instance.Level <= 8)
			{
				var levelIndex = (Profile.Instance.Level - 1) % levels.Length;
				Level = Instantiate(Resources.Load<Level>($"Levels/{levels[levelIndex]}"), transform);
			}
			else if (Profile.Instance.Level > 100)
			{
				Level = levelGenerator.GenerateLevel();
				Level.transform.SetParent(transform, false);
			}
			else
			{
				var levelName = Profile.Instance.Level.ToString("000");
				var patternedLevel = Instantiate(Resources.Load<PatternedLevel>($"Levels/Generated/{levelName}"), transform);
				Level = patternedLevel.Make();
				patternedLevel.transform.SetParent(Level.transform, false);
				Level.transform.SetParent(transform, false);
			}

			Level.Init();
			player.Init();
			_state = State.Initialized;
			Adjust.TrackEvent(Adjust.LevelStart);

			CoinAmountEarned = 0;
			moneyText.text = Profile.Instance.CoinAmount.ToString();
			levelText.text = Profile.Instance.Level.ToString();
			moneyImage.gameObject.SetActive(false);
			
			UpdateProgress(0);
			progressLevelSlider.gameObject.SetActive(false);

			for (var i = 0; i < themes.Length; i++)
			{
				if (themes[i].gameObject.activeSelf) _theme = i;
			}
		}

		internal void Play()
		{
			if (_state == State.Playing) return;
			moneyImage.gameObject.SetActive(true);
			progressLevelSlider.gameObject.SetActive(true);
			player.DoFrame();
			_state = State.Playing;
			Analytics.Instance.LogEvent($"level_{Profile.Instance.Level}_start");
		}

		internal void Win()
		{
			if (_state == State.Win) return;
			_state = State.Win;

			Profile.Instance.Level++;
			Player.Character.Cheer();
			moneyImage.gameObject.SetActive(false);
			progressLevelSlider.gameObject.SetActive(false);
			cameraController.SetActive(false);
			winCamera.SetActive(true);
			mainCamera.transform.SetParent(winCamera.transform);
			confettiBlastRainbow.SetActive(true);
			Analytics.Instance.LogEvent($"level_{Profile.Instance.Level}_win");
			SoundManager.Instance.PlaySfx("LevelSuccess");
			SceneManager.Instance.OpenScene(SceneID.UIWin);
		}

		internal void Lose()
		{
			if (_state == State.Lost) return;
			_state = State.Lost;
			player.Character.Fall();
			moneyImage.gameObject.SetActive(false);
			progressLevelSlider.gameObject.SetActive(false);
			Analytics.Instance.LogEvent($"level_{Profile.Instance.Level}_lose");
			SceneManager.Instance.OpenScene(SceneID.UILose);
		}

		internal void AddCoins(int nCoints)
		{
			CoinAmountEarned += nCoints;
			moneyText.text = (Profile.Instance.CoinAmount + CoinAmountEarned).ToString();
		}

		internal void UpdateProgress(float progress)
		{
			progressLevelSlider.value = progress;
			tabletNumberText.text = Player.Stack.TabletCount.ToString();
		}

		private int _theme;
		internal void SwitchTheme()
		{
			themes[_theme].gameObject.SetActive(false);
			_theme = (_theme + 1) % themes.Length;
			themes[_theme].gameObject.SetActive(true);
		}

		private void Update()
		{
			switch (_state)
			{
				case State.None:
					break;
				case State.Initializing:
					break;
				case State.Initialized:
					break;
				case State.Playing:
					player.DoFrame();
					break;
				case State.Win:
					winCamera.transform.Rotate(0, 0.5f, 0);
					break;
				case State.Lost:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#region Gameplay configurations

		internal const int LaneCount = 5;
		internal const float LaneWidth = 1.0f;
		internal const float RoadWidth = LaneCount * LaneWidth;
		internal const float TurnRadius = RoadWidth * 1.5f;
		internal const float InteractableGap = 2.0f;
		internal const float TrapTriggerSpacing = 0.6f;
		internal const float RoadHeight = 10.0f;
		internal const float NormalSpeed = 12.0f;
		internal const float FastSpeed = 24.0f;
		internal const float ControlSensivity = 0.02f;
		internal const float Deacceleration = 5.0f;

		internal static float CalculateWidth(int laneL, int laneR)
		{
			return (laneR - laneL + 1) * LaneWidth;
		}

		internal static float CalculatePosition(int laneL, int laneR)
		{
			return (laneR + laneL - LaneCount - 1) * 0.5f * LaneWidth;
		}

		internal static float CalculatePosition(int lane)
		{
			return (lane * 2 - LaneCount - 1) * 0.5f * LaneWidth;
		}

		internal static float CalculateLeft(int lane)
		{
			return (lane * 2 - LaneCount - 1) * 0.5f * LaneWidth;
		}

		internal static float CalculateRight(int lane)
		{
			return (lane * 2 - LaneCount + 1) * 0.5f * LaneWidth;
		}

		#endregion


		#region Game Assets
		[SerializeField] private Gate1 gate1Prefab;
		[SerializeField] private Gate2 gate2Prefab;
		[SerializeField] private Lava lavaPrefab;
		[SerializeField] private Ramp rampPrefab;
		[SerializeField] private Rotate rotatePrefab;
		[SerializeField] private Swing swingPrefab;
		[SerializeField] private TrapLying trapLyingPrefab;
		[SerializeField] private TrapStanding trapStandingPrefab;
		[SerializeField] private Trap trapSawPrefab;
		[SerializeField] private Trap trapSpherePrefab;
		[SerializeField] private Wall wallPrefab;
		[SerializeField] private Pit pitPrefab;
		[SerializeField] private Straight straightPrefab;
		[SerializeField] private Turn turnPrefab;
		[SerializeField] private Bridge bridgePrefab;
		[SerializeField] private HeightChange heightPrefab;
		[SerializeField] private Speedup speedupPrefab;
		[SerializeField] private DiamondSequence diamondSequencePrefab;

		private static T NewObject<T>(T prefab, string name, Transform t) where T : Object
		{
#if UNITY_EDITOR
			var p = EditorApplication.isPlayingOrWillChangePlaymode ? prefab :
				AssetDatabase.LoadAssetAtPath<T>($"Assets/Game/Gameplay/Models/{name}.prefab");
			return Instantiate(p, t);
#else
			return Instantiate(prefab, t);
#endif
		}

		internal static Gate1 NewGate1(Transform t)
		{
			return NewObject(Instance.gate1Prefab, "Gate", t);
		}

		internal static Gate2 NewGate2(Transform t)
		{
			return NewObject(Instance.gate2Prefab, "Gate", t);
		}

		internal static Lava NewLava(Transform t)
		{
			return NewObject(Instance.lavaPrefab, "Lava", t);
		}

		internal static Ramp NewRamp(Transform t)
		{
			return NewObject(Instance.rampPrefab, "Ramp", t);
		}

		internal static Rotate NewRotate(Transform t)
		{
			return NewObject(Instance.rotatePrefab, "Rotate", t);
		}

		internal static Swing NewSwing(Transform t)
		{
			return NewObject(Instance.swingPrefab, "Swing", t);
		}

		internal static TrapLying NewLyingTrap(Transform t)
		{
			return NewObject(Instance.trapLyingPrefab, "TrapLying", t);
		}

		internal static TrapStanding NewStandingTrap(Transform t)
		{
			return NewObject(Instance.trapStandingPrefab, "TrapStanding", t);
		}

		internal static Trap NewSawTrap(Transform t)
		{
			return NewObject(Instance.trapSawPrefab, "TrapSaw", t);
		}

		internal static Trap NewSphereTrap(Transform t)
		{
			return NewObject(Instance.trapSpherePrefab, "TrapSphere", t);
		}

		internal static Wall NewWall(Transform t)
		{
			return NewObject(Instance.wallPrefab, "Wall", t);
		}

		internal static Pit NewPit(Transform t)
		{
			return NewObject(Instance.pitPrefab, "Pit", t);
		}

		internal static Straight NewStraight(Transform t)
		{
			return NewObject(Instance.straightPrefab, "Straight", t);
		}

		internal static Turn NewTurn(Transform t)
		{
			return NewObject(Instance.turnPrefab, "Turn", t);
		}

		internal static Bridge NewBridge(Transform t)
		{
			return NewObject(Instance.bridgePrefab, "Bridge", t);
		}

		internal static HeightChange NewHeightChange(Transform t)
		{
			return NewObject(Instance.heightPrefab, "HeightChange", t);
		}

		internal static Speedup NewSpeedup(Transform t)
		{
			return NewObject(Instance.speedupPrefab, "Speedup", t);
		}

		internal static DiamondSequence NewDiamondSequence(Transform t)
		{
			return NewObject(Instance.diamondSequencePrefab, "DiamondSequence", t);
		}
		#endregion
	}
}