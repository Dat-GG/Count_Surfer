
using UnityEngine;
using DG.Tweening;
namespace Funzilla
{
	internal class EditDiamond : EditObject
	{
		[SerializeField] internal int amount = 5;
		[SerializeField] internal int lane = 3;
		protected override void Spawn(Level level, Transform parent, float position)
		{
			var obj = Gameplay.NewDiamondSequence(parent);
			obj.Init(this, position);
			SpawnObject = obj;
		}
	}
}