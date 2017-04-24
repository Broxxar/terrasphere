using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastAudioFadeIn : MonoBehaviour
{
	AudioSource _source;

	void Awake()
	{
		_source = GetComponent<AudioSource>();
	}

	void Update()
	{
		_source.volume = Mathf.Lerp(0, 1, Time.time);
	}
}
