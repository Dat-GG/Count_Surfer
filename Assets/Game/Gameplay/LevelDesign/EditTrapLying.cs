using UnityEngine;
using DG.Tweening;
namespace Funzilla
{
	internal class EditTrapLying : EditObject
	{
		[SerializeField] internal int height;
		[SerializeField] internal int laneL = 1;
		[SerializeField] internal int laneR = 5;
		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewLyingTrap(parent);
			obj.Init(this, position);
			level.AddInteractable(obj);
			SpawnObject = obj;
			
			var t = obj.transform.GetChild(0);
			var r = t.localRotation.eulerAngles;
			t.DOLocalRotate(new Vector3(r.x, r.y, 360), 2f, RotateMode.FastBeyond360)
				.SetEase(Ease.Linear).SetLoops(-1);
		}
	}
}