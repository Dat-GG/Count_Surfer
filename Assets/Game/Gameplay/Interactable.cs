using UnityEngine;

namespace Funzilla
{
	public class Interactable : MonoBehaviour
	{
		internal Transform Group { get; set; }
		internal float Begin { get; private protected set; }
		internal float End { get; private protected set; }
	}

}