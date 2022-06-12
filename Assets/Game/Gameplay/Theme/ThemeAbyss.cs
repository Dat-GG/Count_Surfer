using UnityEngine;

namespace Funzilla
{
	internal class ThemeAbyss : Theme
	{
		[SerializeField] private Color abyssColor;
		[SerializeField] private MeshRenderer abyssRenderer;

		protected override void SetupEnvironment()
		{
			abyssRenderer.gameObject.SetActive(true);
			abyssRenderer.sharedMaterial.color = abyssColor;
		}

		private void OnValidate()
		{
			abyssRenderer.sharedMaterial.color = abyssColor;
		}
	}
}