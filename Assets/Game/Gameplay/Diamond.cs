using UnityEngine;
using DG.Tweening;
namespace Funzilla
{
	internal class Diamond : Interactable
	{
		internal void Init(float x, float position)
		{
			Begin = position;
			End = position + 2;
			transform.GetChild(0).localPosition = new Vector3(x, 0.5f, 0);
			
			var t = transform.GetChild(0);
			var r = t.localRotation.eulerAngles;
			t.DOLocalRotate(new Vector3(r.x, 360, r.z), 2f, RotateMode.FastBeyond360)
				.SetEase(Ease.Linear).SetLoops(-1);
			t.DOShakePosition(8, .1f, 1, 90, false, false).SetLoops(-1);
		}
	}
}