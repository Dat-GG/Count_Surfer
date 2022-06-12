using UnityEngine;

namespace Funzilla
{
	internal class TrapStanding : Trap
	{
		[SerializeField] private Transform single;
		[SerializeField] private Transform upper;
		[SerializeField] private Transform lower;
		[SerializeField] private Transform middle;
		[SerializeField] private CapsuleCollider capsuleCollider;
		
		
		internal void Init(EditTrapStanding info, float begin)
		{
			Init(begin, begin + info.spacing);

			var singleModel = single.gameObject;
			singleModel.SetActive(info.tall <= 1);
			upper.gameObject.SetActive(!singleModel.activeSelf);
			lower.gameObject.SetActive(!singleModel.activeSelf);
			middle.gameObject.SetActive(info.tall >= 3);
			for (var i = 3; i < info.tall; i++)
			{
				var m = Instantiate(middle, single.parent);
				m.localPosition = new Vector3(0, 0, (i - 0.5f) * Gameplay.LaneWidth);
			}

			if (!singleModel.activeSelf)
			{
				upper.localPosition = new Vector3(0, 0, (info.tall - 0.5f) * Gameplay.LaneWidth);
			}
			single.parent.localPosition = new Vector3(Gameplay.CalculatePosition(info.lane), 0, 0);
			capsuleCollider.height = info.tall * Gameplay.LaneWidth;
			capsuleCollider.center = new Vector3(0, 0, capsuleCollider.height * 0.5f);
		}
	}
}