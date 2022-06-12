using UnityEngine;

namespace Funzilla
{
	internal class EditSwing : EditObject
	{
		[SerializeField] internal int laneL = 1;
		[SerializeField] internal int laneR = Gameplay.LaneCount;
		[SerializeField] internal int duration = 2;
		[SerializeField] internal bool left;

		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewSwing(parent);
			obj.Init(this, position);
			SpawnObject = obj;
		}
	}
}