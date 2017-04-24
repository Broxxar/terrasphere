using UnityEngine;

public class SpaceSkyboxMaker : MonoBehaviour
{
	public Cubemap Bake()
	{
		var cubemap = new Cubemap(1024, TextureFormat.ARGB32, false);
		GetComponent<Camera>().RenderToCubemap(cubemap);
		return cubemap;
	}
}
