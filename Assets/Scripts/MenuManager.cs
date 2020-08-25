using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public string WebsiteURL;
	public Canvas Transition;

	public void Play()
	{
		Transition.GetComponent<Animator>().SetTrigger("StartGame");
		// Play a sound
		FindObjectOfType<AudioManager>()?.Play(Sound.SoundCategory.BUTTON);
	}

	public void StartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void Website()
	{
		Application.OpenURL(WebsiteURL);
		// Play a sound
		FindObjectOfType<AudioManager>()?.Play(Sound.SoundCategory.BUTTON);
	}
}
