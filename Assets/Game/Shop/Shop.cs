using UnityEngine;
using UnityEngine.UI;

namespace Funzilla
{
	

	internal class Shop : Scene
	{
		[SerializeField] private ShopData shopData;
		[SerializeField] private Transform itemsContainer;
		[SerializeField] private GameObject shopItemPrefab;
		[SerializeField] private Button purchaseButton;
		private ShopItem[] _shopItems;
		private int _previewItemClickedIndex = -1;
		private int _currentItemIndex = 0;

		private void Awake()
		{
			GeneratorShopItem();
		}

		private void GeneratorShopItem()
		{
			_shopItems = new ShopItem[shopData.GetSkinCount()];
			for (int i = 0; i < shopData.GetSkinCount(); i++)
			{
				var item = Instantiate(shopItemPrefab.GetComponent<ShopItem>(), itemsContainer);
				_shopItems[i] = item;
				item.SetSkinSprite(shopData.SkinDatas[i].Skin);
				item.SetSkinPrice(shopData.SkinDatas[i].SkinPrice);
				item.ShopItemClickListener(i, OnShopItemClick);
			}
		}

		private void OnShopItemClick(int index)
		{
			_currentItemIndex = index;
			ShowBorderSelected(index);
			if (shopData.SkinDatas[index].IsPurchased)
			{
				purchaseButton.GetComponentInChildren<Text>().text = "Purchased";
				purchaseButton.interactable = false;
			}
			else
			{
				purchaseButton.GetComponentInChildren<Text>().text = "Purchase";
				purchaseButton.interactable = true;
			}
		}
		
		private void ShowBorderSelected(int index)
		{
			if (_previewItemClickedIndex != -1) _shopItems[_previewItemClickedIndex].ItemOutline.enabled = false;;
			_shopItems[index].ItemOutline.enabled = true;
			_previewItemClickedIndex = index;
		}

		public void OnPurchaseButtonPressed()
		{
			
		}
		
		
	}
}
