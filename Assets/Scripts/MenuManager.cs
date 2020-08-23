using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public string WebsiteURL;

	public static void Play()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public static void Quit()
	{
		Application.Quit();
	}

	public void Website()
	{
		Application.OpenURL(WebsiteURL);
	}
}
