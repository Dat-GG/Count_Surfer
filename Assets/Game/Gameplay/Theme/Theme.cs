using System;
using UnityEngine;

namespace Funzilla
{
	internal class Theme : MonoBehaviour
	{
		[SerializeField] private Color fogColor;

		private void OnEnable()
		{
			RenderSettings.fogColor = fogColor;
			SetupEnvironment();
		}

		protected virtual void SetupEnvironment()
		{

		}

		private void OnValidate()
		{
			RenderSettings.fogColor = fogColor;
		}

		private void Update()
		{

		}
	}
}