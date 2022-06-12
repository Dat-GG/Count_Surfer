using UnityEngine;

namespace Funzilla
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera cam;

		private void Update()
		{
			var t1 = cam.transform;
			var t2 = transform;
			t1.rotation = t2.rotation;
			var distance = Mathf.Max(0, (Gameplay.Instance.Player.Stack.TabletCount - 20) * .12f);

			var p = t1.position;
			var v = t2.position - t2.forward * distance - p;
			p += v * (Time.smoothDeltaTime * 20);
			t1.position = p;
		}
	}
}