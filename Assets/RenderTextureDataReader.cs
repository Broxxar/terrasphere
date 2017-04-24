using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureDataReader : MonoBehaviour
{
	private RenderTexture _rt;
	private Texture2D internalTex;
	[SerializeField]
	private Texture _defaultTexture;

	private void Awake()
	{
		_rt = GetComponent<Camera>().targetTexture;

		Graphics.SetRenderTarget(_rt);
		GL.Clear(true, true, Color.clear);
		Graphics.Blit(_defaultTexture, _rt);

        internalTex = new Texture2D(128, 128);
	}

	private void OnPostRender()
	{
		Graphics.SetRenderTarget(_rt);
		internalTex.ReadPixels(new Rect(0, 0, 128, 128), 0, 0);
		internalTex.Apply();
	}

	public Color ReadPixel(Vector2 st)
	{
		return internalTex.GetPixel((int)(st.x * internalTex.width), (int)(st.y * internalTex.height));
	}

	public byte[] GetPNG()
	{
		return internalTex.EncodeToPNG();
	}
}
