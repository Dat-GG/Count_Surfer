using UnityEngine;

namespace Funzilla
{
	internal class Lava : Obstacle, ITrapTriggerListener
	{
		private const float BoundaryWidth = 0.2f;
		[SerializeField] private MeshRenderer lavaRenderer;
		[SerializeField] private Transform boundaryL;
		[SerializeField] private Transform boundaryR;
		[SerializeField] private TrapTrigger trigger;
		private static readonly int SurfaceNoise = Shader.PropertyToID("_SurfaceNoise");
		private static readonly int SurfaceDistortion = Shader.PropertyToID("_SurfaceDistortion");

		internal void Init(EditLava info, float begin, float height)
		{
			Begin = begin;
			End = begin + info.length + info.spacing;

			var x = Gameplay.CalculatePosition(info.laneL, info.laneR);
			var w = Gameplay.CalculateWidth(info.laneL, info.laneR);
			boundaryL.localPosition = new Vector3(-w * 0.5f, -1, 0);
			boundaryR.localPosition = new Vector3(+w * 0.5f, 0, 0);
			boundaryL.localScale = boundaryR.localScale = new Vector3(1, 1, Gameplay.RoadHeight + height);

			var t = lavaRenderer.transform;
			t.localScale = new Vector3(w - BoundaryWidth, 1, 1);
			var textureScale = new Vector2((w - BoundaryWidth) * .2f, info.length * .2f);
			lavaRenderer.material.SetTextureScale(SurfaceNoise, textureScale);
			lavaRenderer.material.SetTextureScale(SurfaceDistortion, textureScale);

			var parent = t.parent;
			parent.localPosition = new Vector3(x, 0, 0);
			parent.localScale = new Vector3(1, info.length, 1);

			t = trigger.transform;
			t.localPosition = new Vector3(x, -Gameplay.RoadHeight * 0.5f, info.length * 0.5f);
			t.localScale = new Vector3(
				w - Gameplay.TrapTriggerSpacing,
				Gameplay.RoadHeight + 0.01f,
				info.length - Gameplay.TrapTriggerSpacing);
			trigger.Listener = this;
		}

		public void OnCollided(Tablet tablet)
		{
			Gameplay.Instance.Player.Stack.Discard(tablet);
			SoundManager.Instance.PlaySfx("CubeCollect1");
		}
	}
}