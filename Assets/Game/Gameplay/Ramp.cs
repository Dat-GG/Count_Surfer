using UnityEngine;

namespace Funzilla
{
	internal class Ramp : Trap
	{

		[SerializeField] private Transform model;
		internal void Init(EditJump info, float begin)
		{
			Init(begin, begin + info.spacing);
			model.localPosition = new Vector3(info.x, 0, 0);
		}
	}
}