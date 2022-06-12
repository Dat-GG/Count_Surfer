using Funzilla;
using UnityEngine;

public class NormalsReplacementShader : MonoBehaviour
{
	[SerializeField] private Shader normalsShader;

	private RenderTexture _renderTexture;
	private Camera _camera;
	private static readonly int CameraNormalsTexture = Shader.PropertyToID("_CameraNormalsTexture");

	private void Start()
	{
		var thisCamera = GetComponent<Camera>();

		// Create a render texture matching the main camera's current dimensions.
		_renderTexture = new RenderTexture(thisCamera.pixelWidth, thisCamera.pixelHeight, 24);
		// Surface the render texture as a global variable, available to all shaders.
		Shader.SetGlobalTexture(CameraNormalsTexture, _renderTexture);

		// Setup a copy of the camera to render the scene using the normals shader.
		var copy = new GameObject("Normals camera");
		_camera = copy.AddComponent<Camera>();
		_camera.CopyFrom(thisCamera);
		_camera.transform.SetParent(transform);
		_camera.targetTexture = _renderTexture;
		_camera.SetReplacementShader(normalsShader, "RenderType");
		_camera.depth = thisCamera.depth - 1;
		_camera.cullingMask = Layers.RoadMask | Layers.DeadTabletMask;
		_camera.depthTextureMode = DepthTextureMode.Depth;
		_camera.backgroundColor = Color.black;
	}
}