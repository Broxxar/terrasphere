using System.Collections;
using UnityEngine;

/// <summary>
/// FUCK THIS IS A GOD CLASS NOW WHATEVER
/// </summary>
public class CameraController : MonoBehaviour
{
	[SerializeField]
	private Transform _targetRotationTransform;

	[SerializeField]
	private float _lookSmoothFactor = 5.0f;

	[SerializeField]
	private Vector2 _zoomExtents = new Vector2(-5.0f, -2.5f);

	[SerializeField]
	private float _zoomSmoothFactor = 5.0f;

	[SerializeField]
	private float _defaultZoom = -4.0f;

	[SerializeField]
	private VariableBlur _blur;

	[SerializeField]
	private Animator _titleAnim;

	[SerializeField]
	private Animator _hotbarAnim;


	private Camera _camera;
	private Vector3 _lastMouse;
	private float _targetZoom;
	private bool _gameStarted;

	private void Awake()
	{
		_camera = GetComponentInChildren<Camera>();
		_lastMouse = _camera.ScreenToViewportPoint(Input.mousePosition);
	}

	private void StartGame()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_gameStarted = true;
			_targetZoom = _defaultZoom;
			StartCoroutine(Deblur());
			_titleAnim.SetBool("Visible", false);
			_hotbarAnim.SetBool("Visible", true);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (!_gameStarted)
		{
			StartGame();
			return;
		}

		UpdateZoom();
		UpdateLookRotation();
	}

	private IEnumerator Deblur()
	{
		for (float t = 0; t < 1.0f; t += Time.deltaTime * 2.0f)
		{
			_blur.BlurAmount = (1 - t);

			yield return null;
		}

		_blur.enabled = false;
	}

	private void UpdateZoom()
	{
		_targetZoom = Mathf.Clamp(_targetZoom + Input.mouseScrollDelta.y * 0.5f, _zoomExtents.x, _zoomExtents.y);
		_camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, Vector3.forward * _targetZoom, Time.deltaTime * _zoomSmoothFactor);
	}

	private void UpdateLookRotation()
	{
		var frameMousePos = _camera.ScreenToViewportPoint(Input.mousePosition);
		var deltaMousePos = _lastMouse - frameMousePos;

		if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
		{
			_targetRotationTransform.Rotate(new Vector3(deltaMousePos.y * -360.0f, deltaMousePos.x * 360.0f), Space.Self);
		}

		transform.rotation = Quaternion.Slerp(transform.localRotation, _targetRotationTransform.localRotation, Time.deltaTime * _lookSmoothFactor);
		_lastMouse = frameMousePos;
	}
}
