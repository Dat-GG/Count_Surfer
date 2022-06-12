using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Funzilla
{
	internal class ShopItem : MonoBehaviour
	{
		[SerializeField] private Image skinImage;
		[SerializeField] private Text skinPriceText;
		[SerializeField] private Button itemButton;
		[SerializeField] private Outline itemOutline;
		internal Outline ItemOutline => itemOutline;
		internal void SetSkinSprite(Sprite skinSprite)
		{
			skinImage.sprite = skinSprite;
		}

		internal void SetSkinPrice(int price)
		{
			skinPriceText.text = price.ToString();
		}

		internal void ShopItemClickListener<T>(T param, Action<T> callback)
		{
			itemButton.onClick.AddListener(delegate { callback(param); });
		}
	}
}
