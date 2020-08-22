using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public PatientManager PatientManager;
	public RoomManager RoomManager;
	public Player Player;
	public TimeBar TimeBar;

	public float MaxTime = 120f;
	private float Timer;

    // Start is called before the first frame update
    void Start()
    {
		// Set Timer
		TimeBar.SetMaxTime(MaxTime);
		Timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
		// Handle time running out
		if (Timer >= MaxTime)
		{
			// TODO ---------------------

			// Turn off the patient manager
			PatientManager.IsActive = false;
		}
		else
		{
			// Increment Timer
			Timer += Time.deltaTime;
			TimeBar.SetTime(Timer);
		}
    }
}
