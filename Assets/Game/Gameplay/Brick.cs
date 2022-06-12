using System;
using UnityEngine;

namespace Funzilla
{
	public class Brick : MonoBehaviour
	{
		[SerializeField] private Rigidbody body;

		internal void WakeUp()
		{
			body.isKinematic = false;
		}
	}
}