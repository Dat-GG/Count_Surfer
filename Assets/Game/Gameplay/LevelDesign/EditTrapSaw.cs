using DG.Tweening;
using UnityEngine;

namespace Funzilla
{
	internal class EditTrapSaw : EditObject
	{
		private const float Radius = 1.0f;
		[SerializeField] internal float x = -1.5f;
		[SerializeField] internal int height;

		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewSawTrap(parent);
			obj.Init(position, position + Radius * 4 + Gameplay.InteractableGap + spacing);
			var t = obj.transform.GetChild(0);
			t.localPosition = new Vector3(x, height, 0);
			t.DOLocalRotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360)
				.SetEase(Ease.Linear).SetLoops(-1);
			SpawnObject = obj;
		}
	}
}