using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Funzilla
{
	public class Tutorial : MonoBehaviour
	{
		[SerializeField] private Image handImage;

		private void Awake()
		{
			var handPos = handImage.transform.localPosition;
			handImage.transform.DOLocalMove(new Vector3(handPos.x +666,handPos.y ,handPos.z), 1f).
				SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		}

		private void Update()
		{
			if (!Utils.NonUITapped())
			{
				return;
			}

			gameObject.SetActive(false);
			enabled = false;
			handImage.transform.DOKill();
			SceneManager.Instance.CloseScene(SceneID.Home);
			Gameplay.Instance.Play();
		}
	}
}