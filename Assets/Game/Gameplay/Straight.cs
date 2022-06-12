using UnityEngine;

namespace Funzilla
{
	internal class Straight : Interactable
	{
		[SerializeField] private Renderer model;
		private static readonly int ColorID = Shader.PropertyToID("_Color");

		internal void Setup(float width, float length, float height)
		{
			model.transform.localScale = new Vector3(width, length, Gameplay.RoadHeight + height);
		}

		internal void SetColor(Color color)
		{
			var props = new MaterialPropertyBlock();
			props.SetColor(ColorID, color);
			model.SetPropertyBlock(props);
		}
	}
}