using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DG.Tweening;
namespace Funzilla
{
    internal class ChestRoom : Scene
    {
        [SerializeField] private Button openAgainButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button noButton;
        [SerializeField] private Transform chestPanel;
        [SerializeField] private Text txtGem;
        
        private bool _canOpeChests = true;
        private int _remainingTimes = 3;
        private int _remainingChest = 9;
        private Chest[] _chests;

        private void Awake()
        {
            openAgainButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            SetGemInChest();
        }

        private void Update()
        {
            UpdateButton();
            txtGem.text = Profile.Instance.CoinAmount.ToString();
            UpdateChest();
        }

        private void UpdateChest()
        {
            if(_remainingChest <= 0) return;
            if (_remainingTimes <= 0 && _canOpeChests)
            {
                _canOpeChests = false;
                foreach (var chest in _chests)
                {
                    chest.chestButton.interactable = false;
                }
                openAgainButton.gameObject.SetActive(true);
                Invoke("ShowNoButton",2);
            }
        }

        private void ShowNoButton()
        {
            noButton.gameObject.SetActive(true);
        }

        internal void OnChestPressed()
        {
            _remainingTimes--;
            _remainingChest--;
        }

        internal void OnOpenAgainButtonPressed()
        {
            openAgainButton.gameObject.SetActive(false);
            if(_remainingChest <= 0) return;
            Ads.Instance.ShowRewardedVideo("openChest", (state) =>
            {
                if (state == RewardedVideoState.NotReady || state == RewardedVideoState.Failed || state == RewardedVideoState.Closed)
                {
                    openAgainButton.gameObject.SetActive(true);
                }

                if (state == RewardedVideoState.Watched)
                {
                    _remainingTimes = 3;
                    _canOpeChests = true;
                    foreach (var chest in _chests)
                    {
                        if(!chest.isOpened)  chest.chestButton.interactable = true;
                    }
                    openAgainButton.gameObject.SetActive(false);
                    noButton.gameObject.SetActive(false);
                }
            });
        }

        internal void OnContinueButtonPressed()
        {
            ReloadGame();
        }

        internal void OnNoButtonPressed()
        {
            ReloadGame();
        }

        private void ReloadGame()
        {
            SceneManager.Instance.ShowLoading(false, 1f, () =>
            {
                SceneManager.Instance.CloseScene(SceneID.ChestRoom);
                SceneManager.Instance.OpenScene(SceneID.Gameplay);
            });
        }

        private void UpdateButton()
        {
            if (_remainingChest > 0) return;
            continueButton.gameObject.SetActive(true);
            openAgainButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
        }

        private void SetGemInChest()
        {
            _chests = chestPanel.GetComponentsInChildren<Chest>();
            foreach (var chest in _chests)
            {
                chest.TxtGem.text = RandomGemNumber().ToString();
            }
        }

        private int RandomGemNumber()
        {
            int r = Random.Range(1,4);
            switch (r)
            {
                case 1:
                    return 5;
                case 2:
                    return 10;
                default: return 25;

            }
           
        }

        private void OnEnable()
        {
            SceneManager.Instance.HideLoading();
        }

        private void OnDisable()
        {
            DOTween.KillAll();
        }
    }
}
