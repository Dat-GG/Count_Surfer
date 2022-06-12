using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Funzilla
{
    internal class UIWin : Scene
    {
        [SerializeField] private Button watchAdsButton;
        [SerializeField] private Button noButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button getSkinButton;
        [SerializeField] private Button loseSkinButton;
        [SerializeField] private Text txtGem;
        [SerializeField] private Text txtGemCollected;
        [SerializeField] private Text txtGemReward;
        [SerializeField] private Text txtLevel;
        [SerializeField] private Transform gemEarn;
        [SerializeField] private Transform endFlyGem;
        [SerializeField] private Transform startFlyGem;
        [SerializeField] private Slider progessSlider;
        [SerializeField] private GameObject flyGem;
        private float _currentProgess = 0f;
        private float _currentLevel = 1;
        private String _nextSkin = "Humanoid";
        [Tooltip("Character in 3DView")] [SerializeField] private Transform characterContain;
        [SerializeField] private Material originMaterial;
        [SerializeField] private Material blackMaterial;
        
        private void Awake()
        {
            watchAdsButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(false);
            getSkinButton.gameObject.SetActive(false);
            loseSkinButton.gameObject.SetActive(false);
            Profile.Instance.CoinAmount += Gameplay.CoinAmountEarned;
            txtGemCollected.text = Gameplay.CoinAmountEarned.ToString();
            txtGemReward.text = "+" + Gameplay.CoinAmountEarned;
            txtLevel.text = "LEVEL " + (Profile.Instance.Level-1).ToString();
            _currentLevel = Profile.Instance.Level - 1;
            CheckUnlockSkinProgress();
            CheckNextSkin();
            LoadSkin();
        }

        private void Start()
        {
            progessSlider.DOValue(_currentProgess, 1.9f);
            Invoke("ShowSkinAndButton",2f);

        }

        private void Update()
        {
            txtGem.text = Profile.Instance.CoinAmount.ToString();
        }

        internal void OnWatchAdsButtonPressed()
        {
            watchAdsButton.interactable = false;
            Ads.Instance.ShowRewardedVideo("skip", (state) =>
            {
                if (state == RewardedVideoState.NotReady || state == RewardedVideoState.Failed || state == RewardedVideoState.Closed)
                {
                    if( watchAdsButton.interactable == false) watchAdsButton.interactable = true;
                }

                if (state == RewardedVideoState.Watched)
                {
                    Profile.Instance.CoinAmount += Gameplay.CoinAmountEarned;
                    txtGemCollected.text = (Gameplay.CoinAmountEarned * 2).ToString();
                    watchAdsButton.gameObject.SetActive(false);
                    noButton.gameObject.SetActive(false);
                    continueButton.gameObject.SetActive(true);
                }
            });
            StartCoroutine(FlyGem());
        }

        private IEnumerator FlyGem()
        {
            for (int i = 0; i < 10; i++)
            {
                var gem = Instantiate(flyGem, startFlyGem);
                gem.transform.position = startFlyGem.position;
                gem.transform.DOMove(endFlyGem.position, 1);
                Destroy(gem,1.1f);
                yield return new WaitForSeconds(0.1f);
            }
        }    

        internal void OnNoButtonPressed()
        {
            Ads.Instance.ShowInterstitial((() => {}));
            CheckChestRoom();
        }

        internal void OnContinueButtonPressed()
        {
            CheckChestRoom();
        }

        private void ReloadGame()
        {
            SceneManager.Instance.ShowLoading(false, 1f, () =>
            {
                SceneManager.Instance.ReloadScene(SceneID.Gameplay);
                SceneManager.Instance.CloseScene(SceneID.UIWin);
            });
        }

        private void ShowSkinAndButton()
        {
            if (progessSlider.value < 1)
            {
                watchAdsButton.gameObject.SetActive(true);
            }
            else
            {
                characterContain.GetChild(0).gameObject.GetComponentInChildren<Character>().cRenderer.material = originMaterial;
                characterContain.GetChild(0).gameObject.GetComponentInChildren<Character>().Debut();
                getSkinButton.gameObject.SetActive(true);
                progessSlider.gameObject.SetActive(false);
                gemEarn.gameObject.SetActive(false);
                
            }
            Invoke("ShowNoButton",3);
        }

        private void ShowNoButton()
        {
            if (progessSlider.value < 1)
            {
                noButton.gameObject.SetActive(true);
            }
            else
            {
                loseSkinButton.gameObject.SetActive(true);
            }
        }

        private void LoadSkin()
        {
            var skinPrefab = Resources.Load<GameObject>($"Characters/{_nextSkin}");
            var skin =Instantiate(skinPrefab);
            skin.layer = 9;
            foreach (Transform trans in skin.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = 9;
            }
            skin.transform.parent = characterContain;
            skin.transform.localPosition = Vector3.zero;
            skin.transform.localRotation = Quaternion.identity;
            skin.transform.localScale = new Vector3(3f,3f,3f);
            originMaterial = skin.GetComponentInChildren<Character>().cRenderer.material;
            skin.GetComponentInChildren<Character>().cRenderer.material =
                blackMaterial;
        }

        internal void OnGetSkinButtonPressed()
        {
            getSkinButton.interactable = false;
            Ads.Instance.ShowRewardedVideo("unlockskin", (state) =>
            {
                if (state == RewardedVideoState.NotReady || state == RewardedVideoState.Failed || state == RewardedVideoState.Closed)
                {
                    if( getSkinButton.interactable == false) getSkinButton.interactable = true;
                }

                if (state == RewardedVideoState.Watched)
                {
                    Profile.Instance.UnlockSkin(_nextSkin);
                    ReloadGame();
                }
            });
        }

        private void CheckUnlockSkinProgress()
        {
            var progress = (float)(_currentLevel) % 7;
            if (progress == 0)
            {
                _currentProgess = 1;
                return;
            }
            _currentProgess = (progress / 7);
        }

        private void CheckNextSkin()
        {
            if (_currentLevel <= 7)
            {
                _nextSkin = "Humanoid";
            }
            if (_currentLevel > 7 && _currentLevel <= 14)
            {
                _nextSkin = "Girl";
            }
        }

        private void CheckChestRoom()
        {
            if (_currentLevel % 5 == 0)
            {
                SceneManager.Instance.ShowLoading(false, 1f, () =>
                {
                    SceneManager.Instance.CloseScenes();
                    SceneManager.Instance.OpenScene(SceneID.ChestRoom);
                });
            }
            else
            {
                ReloadGame();
            }
        }

        private void OnEnable()
        {
            SceneManager.Instance.HideLoading();
        }
    }
}
