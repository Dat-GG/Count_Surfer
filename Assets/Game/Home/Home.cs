using System;
using UnityEngine;
using UnityEngine.UI;

namespace Funzilla
{
	internal class Home : Scene
	{
		[SerializeField] private Text levelText;
		[SerializeField] private Button characterButton;
		[SerializeField] private Button themeButton;
		[SerializeField] private Image[] images;
		private int _level;
		private int _milestone;
		private bool _updated = false;

		private void Start()
		{
			levelText.text = $"LEVEL {Profile.Instance.Level}";
			characterButton.onClick.AddListener(() => Gameplay.Instance.Player.SwapCharacter());
			themeButton.onClick.AddListener(() => Gameplay.Instance.SwitchTheme());
			_level = Profile.Instance.Level;
		}

		private void Update()
		{
			if(_updated) return;
			UpdateMilestone();
		}

		private void UpdateMilestone()
		{
			_updated = true;
			_milestone = _level % 5;
			if (_milestone == 0)
			{
				images[4].color = Color.green;
			}
			else
			{
				images[_milestone-1].color = Color.green;
				for (int i = 1; i <= images.Length; i++)
				{
					if (_milestone < i)
					{
						images[i-1].gameObject.SetActive(false);
					}
				}
			}
			
		}
	}
}