using UnityEngine;

namespace Funzilla
{
	internal class Turn : Interactable
	{
		[SerializeField] private Transform model;

		internal void Init(EditTurn info, PathTurn path)
		{
			Begin = End = path.Begin;
			model.localScale = new Vector3(5, 5, Gameplay.RoadHeight + path.Height);
			if (info.left) return;
			const float d = Gameplay.TurnRadius;
			model.localPosition = new Vector3(d, 0, d);
			model.localEulerAngles = new Vector3(-90, -90, 0);
		}
	}
}