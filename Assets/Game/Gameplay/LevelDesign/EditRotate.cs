using UnityEngine;

namespace Funzilla
{
	internal class EditRotate : EditObject
	{
		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewRotate(parent);
			obj.Init(this, position);
			SpawnObject = obj;
		}
	}
}