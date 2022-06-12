using UnityEngine;

namespace Funzilla
{
	internal class PatternedLevel : MonoBehaviour
	{
		internal Level Make()
		{
			var obj = new GameObject(name);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.localScale = Vector3.one;
			var level = obj.AddComponent<Level>();
			foreach (var pattern in GetComponentsInChildren<Pattern>())
			{
				pattern.Populate(level);
			}
			return level;
		}
	}
}