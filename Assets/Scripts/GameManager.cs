using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public PatientManager PatientManager;
	public RoomManager RoomManager;
	public StressEffect StressEffect;
	public Player Player;
	public ShiftClock Clock;
	public Transition Transition;

	// Game vars
	public float MaxTime = 120f;
	private float Timer;
	private bool IsPlaying;

	// Settings
	public string Day;
	public TMP_Text DayLabel;
	public string Date;
	public TMP_Text DateLabel;
	public TMP_Text DeadCountLabel;
	public string InspiringMessage;
	public TMP_Text InspiringMessageLabel;

	// Extras
	public bool Cheats = false;

	private void Start()
	{
		// Set the timer
		Timer = MaxTime;

		// Open the pharmacy immediately if not on the tutorial level
		if (SceneManager.GetActiveScene().name!= "Level1")
		{
			FindObjectOfType<Pharmacy>().IsOpen = true;
		}

		// Set the starting transition text
		DayLabel.SetText(Day);
		DateLabel.SetText(Date);
		InspiringMessageLabel.SetText(InspiringMessage);

		// Play the first message
		FindObjectOfType<AudioManager>()?.Play("message1");
	}

	public void StartLevel()
	{
		IsPlaying = true;
		Player.CanMove = true;

		Invoke("StartPatientManager", 5f);
	}

	private void StartPatientManager()
	{
		PatientManager.IsActive = true;

		// Play the second message
		FindObjectOfType<AudioManager>()?.Play("message2");
	}

	public void EndLevel()
	{
		IsPlaying = false;

		// Add dead patients to the datacollector master lists
		DataCollector dataCollector = FindObjectOfType<DataCollector>();
		int deadPatientsCount = PatientManager.Dead.Count + PatientManager.PatientsWaiting + PatientManager.Patients.Count;
		dataCollector.DeadPatients += deadPatientsCount;
		dataCollector?.RecoveredPatients.AddRange(PatientManager.Recovered);

		// Set the Dead patient count in the transition
		DeadCountLabel.SetText(deadPatientsCount.ToString("D3"));

		// Start the ending animation
		Transition?.GetComponent<Animator>()?.SetTrigger("End");

		// Play a message
		FindObjectOfType<AudioManager>()?.Play("message9");
	}

	public void GoToNextLevel()
	{
		// Go to the next level
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	// Update is called once per frame
	void Update()
    {
		if (IsPlaying)
		{
			HandleCheats();

			// Handle time running out
			if (Timer <= 0)
			{
				// End the level
				EndLevel();

				// Turn off the patient manager
				PatientManager.IsActive = false;
			}
			else
			{
				// Increment Timer
				Timer -= Time.deltaTime;
				Clock?.SetTime(Timer);
			}

			// Set the stress effect in the post processing volume
			StressEffect?.SetStressEffect(Player.Stress, Player.MaxStress);
		}

		// Press escape to quit the game
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private void HandleCheats()
	{
		if (Cheats)
		{
			if (Input.GetKey(KeyCode.F)) Time.timeScale = 5;
			else Time.timeScale = 1;
		}
	}
}
