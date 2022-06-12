using UnityEngine;

namespace Funzilla
{
	internal class EditGate1 : EditObject
	{
		[SerializeField] internal MathType type = MathType.Add;
		[SerializeField] internal int number = 5;
		[SerializeField] internal float x = -1f;

		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewGate1(parent);
			obj.Init(this, position);
			SpawnObject = obj;
		}
	}
}