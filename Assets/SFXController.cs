using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
	private static SFXController _instance;

	public AudioSource RaiseAudio;
	public AudioSource LowerAudio;
	public AudioSource Toggle;

	void Awake()
	{
		_instance = this;
    }

	public static void SetRaiseTargetVolume(float target)
	{
		_instance.RaiseAudio.volume = target;
    }

	public static void PlayToggle()
	{
		_instance.Toggle.PlayOneShot(_instance.Toggle.clip);
    }

	public static void SetLowerTargetVolume(float target)
	{
		_instance.LowerAudio.volume = target;
	}
}
