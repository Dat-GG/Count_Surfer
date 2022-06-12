using UnityEngine;

namespace Funzilla
{
	internal class TrapLying : Trap
	{
		[SerializeField] private Transform single;
		[SerializeField] private Transform left;
		[SerializeField] private Transform right;
		[SerializeField] private Transform middle;
		[SerializeField] private CapsuleCollider capsuleCollider;

		internal void Init(EditTrapLying info, float begin)
		{
			Init(begin, begin + info.spacing);
			var singleModel = single.gameObject;
			singleModel.SetActive(info.laneL >= info.laneR);
			left.gameObject.SetActive(!singleModel.activeSelf);
			right.gameObject.SetActive(!singleModel.activeSelf);
			middle.gameObject.SetActive(info.laneL + 1 < info.laneR);

			var w = Gameplay.CalculateWidth(info.laneL, info.laneR);
			if (!singleModel.activeSelf)
			{
				left.localPosition = new Vector3(0, 0, (-w + Gameplay.LaneWidth) * 0.5f);
				right.localPosition = new Vector3(0, 0, (w - Gameplay.LaneWidth) * 0.5f);
			}

			if (middle.gameObject.activeSelf)
			{
				middle.localPosition = new Vector3(0, 0, left.localPosition.z + Gameplay.LaneWidth);
			}

			var x = middle.localPosition.z;
			for (var i = info.laneL + 2; i <= info.laneR; i++)
			{
				var m = Instantiate(middle, single.parent);
				m.localPosition = new Vector3(0, 0, x);
				x += Gameplay.LaneWidth;
			}

			capsuleCollider.height = w;
			single.parent.localPosition = new Vector3(
				Gameplay.CalculatePosition(info.laneL, info.laneR),
				(info.height + 0.5f * Gameplay.LaneWidth), 0);
		}
	}
}