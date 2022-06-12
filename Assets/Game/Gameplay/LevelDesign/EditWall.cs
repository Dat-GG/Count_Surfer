using UnityEngine;

namespace Funzilla
{
	internal class EditWall : EditObject
	{
		[SerializeField] internal int height = 3;
		[SerializeField] internal int laneL = 1;
		[SerializeField] internal int laneR = Gameplay.LaneCount;

		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewWall(parent);
			obj.Init(this, position);
			SpawnObject = obj;
		}
	}
}