using UnityEditor;
using UnityEditor;
using UnityEngine;

namespace Funzilla
{
	[ExecuteInEditMode]
	internal class EditObject : MonoBehaviour
	{
		[SerializeField] internal int spacing = 12;
		internal Interactable SpawnObject { get; private protected set; }

		private void Awake()
		{
#if UNITY_EDITOR
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			var t = transform;
			t.localPosition = new Vector3(
				0,
				0,
				t.GetSiblingIndex());
#endif
		}

		protected float Init(Level level, Transform parent, float position)
		{
			Spawn(level, parent, position);
			return Mathf.Max(
				SpawnObject ? SpawnObject.End : position,
				InitChildren(transform, level, position)
			);
		}

		protected virtual void Spawn(Level level, Transform parent, float position)
		{
		}

		private float InitChildren(Transform t, Level level, float position)
		{
			Transform group;
			if (SpawnObject)
			{
				SpawnObject.Group = group = new GameObject("Group").transform;
				SpawnObject.Group.SetParent(SpawnObject.transform);
				level.AddInteractable(SpawnObject);
			}
			else
			{
				group = level.transform;
			}

			var end = position;
			var max = position;
			for (var i = 0; i < t.childCount; i++)
			{
				var obj = t.GetChild(i).GetComponent<EditObject>();
				if (!obj) continue;
				end = obj.Init(level, group, end);
				if (max < end)
				{
					max = end;
				}
			}

			return max;
		}
	}
}