using UnityEngine;

public class PlanetSpinner : MonoBehaviour
{
	public Vector3 RotationPerSecond;

	private void Update()
	{
		transform.Rotate(RotationPerSecond * Time.deltaTime);
	}
}
