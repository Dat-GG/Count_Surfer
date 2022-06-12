using UnityEngine;

namespace Funzilla
{
	internal class EditLava : EditHole
	{
		protected override void Spawn(Level level, Transform t, float position)
		{
			var obj = Gameplay.NewLava(t);
			obj.Init(this, position, level.Height);
			level.Holes.Add(this);
			SpawnObject = obj;
		}
	}
}