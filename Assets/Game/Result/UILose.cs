using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Funzilla
{
    internal class UILose : Scene
    {
        [SerializeField] private Button skipButton;
        internal void OnSkipButtonPressed()
        {
            skipButton.interactable = false;
            Ads.Instance.ShowRewardedVideo("skip", (state) =>
            {
                if (state == RewardedVideoState.NotReady || state == RewardedVideoState.Failed || state == RewardedVideoState.Closed)
                {
                    if( skipButton.interactable == false) skipButton.interactable = true;
                }

                if (state == RewardedVideoState.Watched)
                {
                    Profile.Instance.Level++;
                    ReloadGame();
                }

                
           
            });
        }

        internal void OnRetryButtonPressed()
        {
            Ads.Instance.ShowInterstitial((() => {}));
            ReloadGame();
        }
        private void ReloadGame()
        {
            SceneManager.Instance.ShowLoading(false, 1f, () =>
            {
                SceneManager.Instance.ReloadScene(SceneID.Gameplay);
                SceneManager.Instance.CloseScene(SceneID.UILose);
            });
        }

    }
}
