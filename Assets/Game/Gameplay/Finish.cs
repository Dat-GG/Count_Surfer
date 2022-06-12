using UnityEngine;

namespace Funzilla
{
	public class Finish : MonoBehaviour
	{
		[SerializeField] private Color[] colors;
		[SerializeField] private FinishStep stepPrefab;

		internal void Init(Level level)
		{
			for (var i = 0; i < colors.Length; i++)
			{
				var step = Instantiate(stepPrefab);
				step.Init(level, colors[i], i);
			}
		}
	}
}