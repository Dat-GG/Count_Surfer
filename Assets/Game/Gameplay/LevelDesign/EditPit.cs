using UnityEngine;

namespace Funzilla
{
	internal class EditPit : EditHole
	{
		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewPit(parent);
			obj.Init(this, position);
			SpawnObject = obj;
			level.Holes.Add(this);
		}
	}
}