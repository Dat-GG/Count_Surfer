using UnityEngine;
using UnityEngine.UI;

namespace Funzilla
{
	internal class FinishStep : MonoBehaviour
	{
		[SerializeField] private Text text;

		internal void Init(Level level, Color color, int index)
		{
			var straight = Gameplay.NewStraight(level.transform);
			straight.SetColor(color);
			var t = transform;
			t.SetParent(straight.transform);
			t.localPosition = new Vector3(0, 0.01f, 1.5f);

			text.text = $"×{index + 1}";
			level.AddFinishStep(straight, index);
		}
	}
}