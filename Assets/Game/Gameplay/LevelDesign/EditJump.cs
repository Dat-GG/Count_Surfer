using UnityEngine;

namespace Funzilla
{
	internal class EditJump : EditObject
	{
		[SerializeField] internal float x = -1.5f;
		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewRamp(parent);
			obj.Init(this, position);
			SpawnObject = obj;
		}
	}
}