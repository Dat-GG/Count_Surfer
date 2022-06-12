
using UnityEngine;

namespace Funzilla
{
	internal class EditHeight : EditObject
	{
		[SerializeField] internal int height = 2;
		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewHeightChange(parent);
			obj.Init(level,this, position);
			SpawnObject = obj;
		}
	}
}