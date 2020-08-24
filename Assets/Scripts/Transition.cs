using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Transition : MonoBehaviour
{
	public GameManager GameManager;
	public MenuManager MenuManager;

	public void StartLevel()
	{
		GameManager?.StartLevel();
	}

	public void GoToNextLevel()
	{
		GameManager?.GoToNextLevel();
	}

	public void StartGame()
	{
		MenuManager?.StartGame();
	}
}
