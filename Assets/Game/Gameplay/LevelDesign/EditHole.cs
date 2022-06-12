using UnityEngine;

namespace Funzilla
{
	internal class EditHole : EditObject
	{
		[SerializeField] internal int laneL = 1;
		[SerializeField] internal int laneR = 5;
		[SerializeField] internal int length = 5;

		internal float Begin => SpawnObject.Begin;
		internal float End => SpawnObject.End - spacing;

		internal static int EstimateLoss(int length)
		{
			var t = (length - 0.9f) / Gameplay.NormalSpeed;
			var s = Physics.gravity.magnitude * t * t / 2;
			return Mathf.FloorToInt(s / Tablet.Thickness);
		}

		internal static int EstimateLossSpeed(int length)
		{
			float t;
			const float tmax = (Gameplay.FastSpeed - Gameplay.NormalSpeed) / Gameplay.Deacceleration;
			const float lmax = Gameplay.FastSpeed * tmax - Gameplay.Deacceleration * tmax * tmax * 0.5f;
			if (length >= lmax)
			{
				t = tmax + (length - lmax + 2) / Gameplay.NormalSpeed;
			}
			else
			{
				var delta = Gameplay.FastSpeed * Gameplay.FastSpeed + 2 * (length + 2) * Gameplay.Deacceleration;
				t = (Mathf.Sqrt(delta) - Gameplay.FastSpeed) / Gameplay.Deacceleration;
			}
			var s = Physics.gravity.magnitude * t * t / 2;
			return Mathf.FloorToInt(s / Tablet.Thickness);
		}
	}
}