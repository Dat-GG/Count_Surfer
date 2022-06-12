using UnityEngine;
using DG.Tweening;
namespace Funzilla
{
	internal class EditTrapSphere : EditObject
	{
		private const float Radius = 1.5f;
		[SerializeField] internal float x = -1f;

		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewSphereTrap(parent);
			obj.Init(position, position + Radius * 6 + Gameplay.InteractableGap + spacing);
			obj.transform.GetChild(0).localPosition = new Vector3(x, 1.5f, 0);
			SpawnObject = obj;
			
			var t = obj.transform.GetChild(0);
			var r = t.localRotation.eulerAngles;
			t.DOLocalRotate(new Vector3(r.x, 360, r.z), 2f, RotateMode.FastBeyond360)
				.SetEase(Ease.Linear).SetLoops(-1);
		}
	}
}