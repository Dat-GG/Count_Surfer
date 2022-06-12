using UnityEngine;
using DG.Tweening;
namespace Funzilla
{
	internal class EditTrapStanding : EditObject
	{
		[SerializeField] internal int tall = 4;
		[SerializeField] internal int lane = 2;

		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewStandingTrap(parent);
			obj.Init(this, position);
			SpawnObject = obj;
			
			var t = obj.transform.GetChild(0);
			var r = t.localRotation.eulerAngles;
			t.DOLocalRotate(new Vector3(r.x, r.y + 360, r.z), 2f, RotateMode.FastBeyond360)
				.SetEase(Ease.Linear).SetLoops(-1);
		}
	}
}