using UnityEngine;

namespace Funzilla
{
	internal class EditTurn : EditObject
	{
		[SerializeField] internal bool left;
		protected override void Spawn(Level level, Transform parent, float position)
		{
			var path = level.Turn(this, position);
			var obj = Gameplay.NewTurn(parent);
			obj.Init(this, path);
			SpawnObject = obj;
		}
	}
}