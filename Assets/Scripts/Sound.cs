using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]

public class Sound
{
	public string name;

	public enum SoundCategory { THEME, BUZZ, MESSAGE };
	public SoundCategory category;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .3f;
	[Range(.1f, 3f)]
	public float pitch = 1;

	public bool loop;

	[HideInInspector]
	public AudioSource source;
}
