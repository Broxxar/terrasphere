using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolToggle : MonoBehaviour
{
	public Tool ToolToUse;
	public KeyCode HotKey;

	void Start()
	{
		GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
	}

	void Update()
	{
		if (Input.GetKeyDown(HotKey))
		{
			GetComponent<Toggle>().isOn = true;
		}
	}

	private void OnValueChanged(bool value)
	{
		if (value)
		{
			SFXController.PlayToggle();
			PaintingRaycaster.SelectTool(ToolToUse);
		}
	}
}
