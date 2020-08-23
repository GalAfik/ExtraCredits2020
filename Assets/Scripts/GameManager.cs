using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public PatientManager PatientManager;
	public RoomManager RoomManager;
	public StressEffect StressEffect;
	public Player Player;
	public ShiftClock Clock;

	public float MaxTime = 120f;
	private float Timer;

	public bool Cheats = false;

	private void Start()
	{
		// Set the timer
		Timer = MaxTime;
	}

	// Update is called once per frame
	void Update()
    {
		HandleCheats();

		// Handle time running out
		if (Timer <= 0)
		{
			// TODO ---------------------

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

	private void HandleCheats()
	{
		if (Cheats)
		{
			if (Input.GetKey(KeyCode.F)) Time.timeScale = 5;
			else Time.timeScale = 1;
		}
	}
}
