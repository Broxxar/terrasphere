using UnityEngine;

public class HeightPainter : MonoBehaviour
{
	[Range(0.05f, 1.0f)]
	public float BrushSize = 0.2f;

	public Material _raiseMaterial;

	public Material _lowerMaterial;

	public Renderer _brushRenderer;

	[SerializeField]
	private GameObject _brush;

	[SerializeField]
	private Transform _brushInner;

	[SerializeField]
	private RenderTextureDataReader[] _dataReaders;

	public void BeginDrawing()
	{
		_brush.SetActive(true);
	}
	/*
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			foreach (var dataReader in _dataReaders)
			{
				File.WriteAllBytes(Path.Combine(Application.dataPath, dataReader.name + ".png"), dataReader.GetPNG());
			}
		}
	}*/

	public void Raise(Vector3 normal)
	{
		SFXController.SetRaiseTargetVolume(Mathf.Max(0.3f, 1 - ReadPixel(normal).r));

		_brushRenderer.sharedMaterial = _raiseMaterial;

		var rotation = Vector3.back * Random.Range(0.0f, 360.0f);

		_brushInner.localEulerAngles = rotation;
		_brushInner.localScale = Vector3.one * BrushSize;

		_brush.transform.localPosition = normal;
		_brush.transform.LookAt(transform.position);
	}

	public void Lower(Vector3 normal)
	{
		SFXController.SetLowerTargetVolume(Mathf.Max(0.3f, ReadPixel(normal).r * 10));

		_brushRenderer.sharedMaterial = _lowerMaterial;

		var rotation = Vector3.back * Random.Range(0.0f, 360.0f);

		_brushInner.localEulerAngles = rotation;
		_brushInner.localScale = Vector3.one * BrushSize;

		_brush.transform.localPosition = normal;
		_brush.transform.LookAt(transform.position);
	}

	public void FinishDrawing()
	{
		SFXController.SetRaiseTargetVolume(0);
		SFXController.SetLowerTargetVolume(0);
		_brush.SetActive(false);
	}

	public Color ReadPixel(Vector3 dir)
	{
		var abs = new Vector3(Mathf.Abs(dir.x), Mathf.Abs(dir.y), Mathf.Abs(dir.z));

		RenderTextureDataReader reader = null;
		var st = Vector2.zero;
		var face = FaceSelection(dir);

		switch (face)
		{
			case CubemapFace.PositiveX:
				{
					st.x = (-dir.z / abs.x + 1) * 0.5f;
					st.y = 1 - (-dir.y / abs.x + 1) * 0.5f;
					reader = _dataReaders[0];
					break;
				}

			case CubemapFace.PositiveY:
				{
					st.x = 1 - (dir.z / abs.y + 1) * 0.5f;
					st.y = 1 - (dir.x / abs.y + 1) * 0.5f;
					reader = _dataReaders[1];
					break;
				}
			case CubemapFace.PositiveZ:
				{
					st.x = (dir.x / abs.z + 1) * 0.5f;
					st.y = 1 - (-dir.y / abs.z + 1) * 0.5f;
					reader = _dataReaders[2];
					break;
				}
			case CubemapFace.NegativeX:
				{
					st.x = (dir.z / abs.x + 1) * 0.5f;
					st.y = 1 - (-dir.y / abs.x + 1) * 0.5f;
					reader = _dataReaders[3];
					break;
				}
			case CubemapFace.NegativeY:
				{
					st.x = (dir.z / abs.y + 1) * 0.5f;
					st.y = (-dir.x / abs.y + 1) * 0.5f;
					reader = _dataReaders[4];
					break;
				}
			case CubemapFace.NegativeZ:
				{
					st.x = 1 - (dir.x / abs.z + 1) * 0.5f;
					st.y = 1 - (-dir.y / abs.z + 1) * 0.5f;
					reader = _dataReaders[5];
					break;
				}
		}

		return reader.ReadPixel(st);
	}

	public CubemapFace FaceSelection(Vector3 dir)
	{
		var abs = new Vector3(Mathf.Abs(dir.x), Mathf.Abs(dir.y), Mathf.Abs(dir.z));

		if (abs.x > abs.y && abs.x > abs.z)
		{
			if (Mathf.Sign(dir.x) > 0)
			{
				return CubemapFace.PositiveX;
			}
			else
			{
				return CubemapFace.NegativeX;
			}
		}
		else if (abs.y > abs.x && abs.y > abs.z)
		{
			if (Mathf.Sign(dir.y) > 0)
			{
				return CubemapFace.PositiveY;
			}
			else
			{
				return CubemapFace.NegativeY;
			}
		}
		else
		{
			if (Mathf.Sign(dir.z) > 0)
			{
				return CubemapFace.PositiveZ;
			}
			else
			{
				return CubemapFace.NegativeZ;
			}
		}
	}
}
