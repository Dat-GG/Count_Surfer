using DG.Tweening;
using UnityEngine;

namespace Funzilla
{
	public class Speedup : Interactable
	{
		internal const int Length = 2;

		[SerializeField] private MeshRenderer model;

		internal void Init(EditSpeedup info, float begin)
		{
			Begin = begin;
			End = begin + Length + info.spacing;
			model.transform.localPosition = new Vector3(info.x, 0, 0);
			model.material.DOOffset(new Vector2(0, -1), 0.7f).SetLoops(-1).SetEase(Ease.Linear);
			model.material.DOColor(new Color32(138, 255, 100, 255), 0.5f)
				.SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
		}
	}
}