using UnityEngine;

namespace Funzilla
{
	internal class EditGate2 : EditObject
	{
		[SerializeField] internal MathType typeL = MathType.Add;
		[SerializeField] internal int numberL = 5;
		[SerializeField] internal MathType typeR = MathType.Multiply;
		[SerializeField] internal int numberR = 2;

		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewGate2(parent);
			obj.Init(this, position);
			SpawnObject = obj;
		}
	}
}