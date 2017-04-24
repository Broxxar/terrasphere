using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
	public GameObject DiskPrefab;

	List<Vector3> _points = new List<Vector3>();

	void Awake()
	{
		PlacePoints();

		foreach (var point in _points)
		{
			var disk = Instantiate(DiskPrefab);
			disk.transform.position = point;
			disk.transform.forward = -point;
        }
	}

	void PlacePoints()
	{
		_points.Clear();

		var n = 101;
		var goldenAngle = Mathf.PI * (3 - Mathf.Sqrt(5));

		for (int i = 0; i < n; i++)
		{
			var theta = goldenAngle * i;
			var y = i * (2.0f / n) - 1 + (2.0f / n);
			var radius = Mathf.Sqrt(1 - y * y);
			var x = radius * Mathf.Cos(theta);
			var z = radius * Mathf.Sin(theta);

			var point = new Vector3(x, y, z).normalized;

			_points.Add(point);
		}
	}
}
