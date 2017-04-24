using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tool
{
	Raise,
	Lower
}

/// <summary>
/// GOD CLASS NUMBER TTWWOOOOO SOOOONNNNNNN
/// </summary>
public class PaintingRaycaster : MonoBehaviour
{
	private static PaintingRaycaster _instance;

	public HeightPainter Painter;

	private bool _painting;

	private Tool _tool;

	private void Awake()
	{
		_instance = this;
	}

	public static void SelectTool(Tool tool)
	{
		_instance.SelectToolInternal(tool);
	}

	private void SelectToolInternal(Tool tool)
	{
		_tool = tool;
	}

	void Update()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			var targetTransform = hit.collider.transform;
			var objectPoint = targetTransform.InverseTransformPoint(hit.point);

			if (Input.GetMouseButtonDown(0))
			{
				_painting = true;
				Painter.BeginDrawing();
			}

			if (Input.GetMouseButton(0))
			{
				if (!_painting)
				{
					_painting = true;
					Painter.BeginDrawing();
				}

				switch (_tool)
				{
					case Tool.Lower:
						{
							Painter.Lower(objectPoint);
							break;
						}
					case Tool.Raise:
						{
							Painter.Raise(objectPoint);
							break;
						}
				}
			}
		}
		else
		{
			_painting = false;
			Painter.FinishDrawing();
		}

		if (Input.GetMouseButtonUp(0))
		{
			_painting = false;
			Painter.FinishDrawing();
		}
	}

	void UseRaiseTool()
	{

	}

	void UseLowerTool()
	{

	}
}
