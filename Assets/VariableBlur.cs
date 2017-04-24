using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VariableBlur : MonoBehaviour
{
	public Material BlurMaterial;
	[Range(0, 1)]
	public float BlurAmount;

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		var final = RenderTexture.GetTemporary(src.width, src.height);
		Graphics.Blit(src, final);

		for (int i = 1; i <= 4; i++)
		{
			BlurMaterial.SetVector("_BlurSize", (src.texelSize * i * 2) * BlurAmount);
			var temp = RenderTexture.GetTemporary(src.width, src.height);

			Graphics.Blit(final, temp, BlurMaterial, 0);
			Graphics.Blit(temp, final, BlurMaterial, 1);

			RenderTexture.ReleaseTemporary(temp);
		}

		Graphics.Blit(final, dst);
		RenderTexture.ReleaseTemporary(final);
	}
}
