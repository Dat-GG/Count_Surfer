
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Funzilla
{
	[ExecuteInEditMode]
	internal class GradientBackground : Theme
	{
		[SerializeField] private new Camera camera;
		[SerializeField] private Color upper;
		[SerializeField] private Color lower;
		[SerializeField] private MeshRenderer meshRenderer;
		[SerializeField] private MeshFilter meshFilter;

		private static readonly Vector3[] Vertices = new Vector3[]
		{
			new Vector3(0.5f, 0.5f, 0),
			new Vector3(-0.5f, 0.5f, 0),
			new Vector3(-0.5f, -0.5f, 0),
			new Vector3(0.5f, -0.5f, 0)
		};

		private static readonly int[] Triangles = new int[] { 0, 2, 1, 0, 3, 2 };

		private void OnEnable()
		{
			MakeMesh();
		}

		[ContextMenu("Refresh")]
		private void MakeMesh()
		{
			if (camera == null)
			{
				return;
			}
			meshFilter.sharedMesh = new Mesh()
			{
				vertices = Vertices,
				colors = new Color[] { upper, upper, lower, lower },
				triangles = Triangles
			};

			var p = (camera.farClipPlane - 1f);
			var t = camera.transform;
			var h = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad * 0.5f) * p * 2f;
			var t2 = transform;
			t2.localPosition = new Vector3(0, 0, p);
			t2.localScale = new Vector3(h * camera.aspect, h, 0f);
		}

#if UNITY_EDITOR
		private Color _cacheColor0, _cacheColor1;
		private void OnValidate()
		{
			if (meshFilter == null)
			{
				return;
			}

			EditorApplication.delayCall += () => {
				if (meshFilter == null)
				{
					return;
				}

				if (_cacheColor0 == upper && _cacheColor1 == lower) return;
				_cacheColor0 = upper;
				_cacheColor1 = lower;
				MakeMesh();
			};
		}
#endif

		private Tweener _tween;
		internal void FadeTo(Color upper, Color lower)
		{
			_tween?.Kill();
			var oldUpper = this.upper;
			var oldLower = this.lower;
			_tween = DOVirtual.Float(0, 1, 1f, (t) =>
			{
				SetGradientColor(
					Color.Lerp(oldUpper, upper, t),
					Color.Lerp(oldLower, lower, t));
			}).OnComplete(()=> _tween = null);
		}

		internal void SetGradientColor(Color upper, Color lower)
		{
			this.upper = upper;
			this.lower = lower;
			MakeMesh();
		}
	}
}