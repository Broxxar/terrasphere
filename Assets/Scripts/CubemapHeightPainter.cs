using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubemapHeightPainter : MonoBehaviour
{
	private Camera _camera;
	[SerializeField]
	private RenderTexture _cubemap;

	void Awake()
	{
		_camera = GetComponent<Camera>();
		_cubemap = new RenderTexture(128, 128, 0)
		{
			dimension = UnityEngine.Rendering.TextureDimension.Cube
		};
		Shader.SetGlobalTexture("_CubePaintingTest", _cubemap);
	}

	void LateUpdate()
	{
		_camera.RenderToCubemap(_cubemap);
	}
}
