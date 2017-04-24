using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SpaceSkyboxMaker))]
public class SpaceSkyboxMakerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var maker = (SpaceSkyboxMaker)target;

		if (GUILayout.Button("Bake Skybox"))
		{
			var cubemap = maker.Bake();
			var path = EditorUtility.SaveFilePanel("Save New Cubemap", "Assets", "Cubemap", "asset");
			path.Replace(Application.dataPath, "");

			if (path.StartsWith(Application.dataPath))
			{
				path = "Assets" + path.Substring(Application.dataPath.Length);
			}

			if (!string.IsNullOrEmpty(path))
			{
				AssetDatabase.CreateAsset(cubemap, path);
				AssetDatabase.SaveAssets();
			}
		}
	}
}
