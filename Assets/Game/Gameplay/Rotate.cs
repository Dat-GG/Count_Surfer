using UnityEngine;
using DG.Tweening;

namespace Funzilla
{
	internal class Rotate : Interactable
	{
		private const float RotateTime = 5;

		internal void Init(EditRotate info, float begin)
		{
			Begin = begin;
			End = begin + info.spacing;
		}
		private void Start()
		{
			var t = transform.GetChild(0);
			t.localRotation = Quaternion.identity;
			t
				.DOLocalRotate(new Vector3(0, 360, 0), RotateTime, RotateMode.FastBeyond360)
				.SetEase(Ease.Linear).SetLoops(-1);
		}
	}
}