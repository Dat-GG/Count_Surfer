using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funzilla
{
	public class Bridge : MonoBehaviour
	{
		[SerializeField] private Transform model;
		internal void Init(int lane, float length, float height)
		{
			var t = model.transform;
			t.localScale = new Vector3(Gameplay.LaneWidth, length, Gameplay.RoadHeight + height);
			t.localPosition = new Vector3(Gameplay.CalculatePosition(lane), 0, 0);
		}
	}
}