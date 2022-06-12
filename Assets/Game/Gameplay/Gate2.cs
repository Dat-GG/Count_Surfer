using System;
using UnityEngine;
using UnityEngine.UI;

namespace Funzilla
{
	internal class Gate2 : Interactable
	{
		[SerializeField] private Text textL;
		[SerializeField] private SpriteRenderer sensorL;
		[SerializeField] private Text textR;
		[SerializeField] private SpriteRenderer sensorR;
		[SerializeField] private GameObject fx;
		private EditGate2 _info;
		private static void InitMath(Text text, SpriteRenderer sensor, MathType type, int number)
		{
			switch (type)
			{
				case MathType.Add:
					sensor.color = Gate1.Blue;
					text.text = $"+{number}";
					break;
				case MathType.Substract:
					sensor.color = Gate1.Red;
					text.text = $"-{number}";
					break;
				case MathType.Multiply:
					sensor.color = Gate1.Blue;
					text.text = $"×{number}";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		internal void Init(EditGate2 info, float begin)
		{
			_info = info;
			Begin = begin;
			End = begin + info.spacing;
			InitMath(textL, sensorL, info.typeL, info.numberL);
			InitMath(textR, sensorR, info.typeR, info.numberR);
		}

		private void OnTriggerEnter(Collider other)
		{
			SoundManager.Instance.PlaySfx("CubeCollect3");
			if (!enabled) return;
			if (other.gameObject.layer != 7) return;
			if (!Gameplay.Instance.Playing) return;
			var p = transform.InverseTransformPoint(other.transform.position);
			enabled = false;
			if (p.x < 0)
			{
				Gate1.Activate(_info.typeL, _info.numberL);
				sensorL.gameObject.SetActive(false);
				textL.gameObject.SetActive(false);
				fx.transform.localPosition = sensorL.transform.localPosition;
			}
			else
			{
				Gate1.Activate(_info.typeR, _info.numberR);
				sensorR.gameObject.SetActive(false);
				textR.gameObject.SetActive(false);
				fx.transform.localPosition = sensorR.transform.localPosition;
			}
			fx.SetActive(true);
		}
	}
}