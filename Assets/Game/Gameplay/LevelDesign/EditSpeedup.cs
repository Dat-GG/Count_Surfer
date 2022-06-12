using UnityEngine;

namespace Funzilla
{
	internal class EditSpeedup : EditObject
	{
		[SerializeField] internal float x = -1.5f;
		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewSpeedup(parent);
			obj.Init(this, position);
			SpawnObject = obj;
		}
	}
}