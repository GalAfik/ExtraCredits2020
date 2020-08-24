using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public Sound[] Sounds;

	public static AudioManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
		// Persistent
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);

		// Set up Audio sources for each clip in sounds
		foreach (var sound in Sounds)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.volume = sound.volume;
			sound.source.pitch = sound.pitch;
			sound.source.loop = sound.loop;
		}
    }

	private void Start()
	{
		// Play the opening theme
		Play("theme");
	}

	public void Play(string name)
	{
		Sound sound = Array.Find(Sounds, s => s.name == name);
		if (sound == null) throw new System.Exception("Sound clip '" + sound.name + "' could not be found.");
		else sound.source?.Play();
	}

	public void Play(Sound.SoundCategory category)
	{
		// Play a random sound within the chosen category
		Sound[] sounds = Sounds.Where(s => s.category == category).ToArray();
		Sound randomSound = sounds[UnityEngine.Random.Range(0, sounds.Length)];
		if (randomSound == null) throw new System.Exception("Sound clip of type '" + randomSound.category + "' could not be found.");
		else randomSound.source?.Play();
	}
}
