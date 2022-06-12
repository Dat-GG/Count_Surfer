using UnityEngine;

namespace Funzilla
{
	internal class DiamondSequence : Interactable
	{
		internal const int Spacing = 2;
		[SerializeField] private Diamond prefab;
		internal void Init(EditDiamond info, float position)
		{
			Begin = position;
			End = position + info.amount * Spacing + info.spacing;

			var level = Gameplay.Instance.Level;
			var x = Gameplay.CalculatePosition(info.lane);
			for (var i = 0; i < info.amount; i++)
			{
				var diamond = Instantiate(prefab, level.transform);
				diamond.Init(x, position);
				level.AddInteractable(diamond);
				position += 2;
			}
		}

		internal static int CalculateLength(int amount, int spacing)
		{
			return amount * spacing;
		}
	}
}