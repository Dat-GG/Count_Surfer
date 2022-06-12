using System;
using UnityEngine;
using UnityEngine.UI;

namespace Funzilla
{
	internal enum MathType
	{
		Add, Substract, Multiply
	}

	internal class Gate1 : Interactable
	{
		internal static readonly Color Blue = new Color32(0, 194, 255, 250);
		internal static readonly Color Red = new Color32(222, 45, 45, 250);

		[SerializeField] private Text text;
		[SerializeField] private SpriteRenderer sensor;
		[SerializeField] private Transform particleTransform;
		[SerializeField] private GameObject fx;
		private EditGate1 _info;
		
		internal static void Activate(MathType type, int number)
		{
			switch (type)
			{
				case MathType.Add:
					Gameplay.Instance.Player.Stack.Add(number);
					break;
				case MathType.Substract:
					Gameplay.Instance.Player.Stack.Subtract(number);
					break;
				case MathType.Multiply:
					Gameplay.Instance.Player.Stack.Multiply(number);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		internal void Activate()
		{
			Activate(_info.type, _info.number);
			sensor.gameObject.SetActive(false);
			text.gameObject.SetActive(false);
			fx.SetActive(true);
		}

		internal void Init(EditGate1 info, float begin)
		{
			Begin = begin;
			End = begin + info.spacing;

			_info = info;
			transform.GetChild(0).localPosition = new Vector3(info.x, 1, 0);
			switch (info.type)
			{
				case MathType.Add:
					sensor.color = Blue;
					text.text = $"+{info.number}";
					break;
				case MathType.Substract:
					sensor.color = Red;
					text.text = $"-{info.number}";
					break;
				case MathType.Multiply:
					sensor.color = Blue;
					text.text = $"×{info.number}";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}