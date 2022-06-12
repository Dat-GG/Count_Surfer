
using System;
using UnityEngine;
using DG.Tweening;

namespace Funzilla
{
	internal class Swing : Interactable
	{
		private EditSwing _info;
		internal void Init(EditSwing info, float begin)
		{
			_info = info;
			Begin = begin;
			End = begin + info.spacing;
		}

		private void Start()
		{
			var t = Group.transform;
			if (t.childCount <= 0) return;
			int l1, l2;
			if (_info.left)
			{
				l1 = _info.laneL;
				l2 = _info.laneR;
			}
			else
			{
				l1 = _info.laneR;
				l2 = _info.laneL;
			}
			t.localPosition = new Vector3(Gameplay.CalculatePosition(l1), 0, 0);
			t.DOLocalMove(new Vector3(Gameplay.CalculatePosition(l2), 0, 0), _info.duration)
				.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		}
	}
}