using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbParticleHack : MonoBehaviour
{
	private ParticleSystem _ps;
	private ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[1000];

	void Awake()
	{
		_ps = GetComponent<ParticleSystem>();
	}

	void Update()
	{
		var count = _ps.GetParticles(_particles);

		for (int i = 0; i < count; i++)
		{
			var rotation = Quaternion.FromToRotation(Vector3.forward, _particles[i].position.normalized);
			_particles[i].rotation3D = rotation.eulerAngles;
        }

		_ps.SetParticles(_particles, count);
	}
}
