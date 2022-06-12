using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Funzilla
{
	[System.Serializable]
	internal struct SkinData
	{
		[SerializeField] private Sprite skin;
		[SerializeField] private string skinName;
		[SerializeField] private int skinPrice;
		[SerializeField] private bool isPurchased;
		internal Sprite Skin => skin;
		internal string SkinName => skinName;
		internal int SkinPrice => skinPrice;
		
		internal bool IsPurchased
		{
			get => isPurchased;
			set => isPurchased = value;
		}

		
	}
	[CreateAssetMenu(fileName = "shopdata",menuName = "Shop/Create shop data", order = 1)]
	internal class ShopData : ScriptableObject
	{
		[SerializeField] private SkinData[] _skinDatas;
		internal SkinData[] SkinDatas => _skinDatas;

		internal int GetSkinCount()
		{
			return _skinDatas.Length;
		}

		internal void PurchaseSkin(int index)
		{
			_skinDatas[index].IsPurchased = true;
		}
	}
}
