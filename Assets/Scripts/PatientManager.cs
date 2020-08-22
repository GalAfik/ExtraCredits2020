using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
	public Patient DefaultPatient;

	private List<Patient> Patients;
	private List<Patient> Recovered; // Patients that have recovered
	private List<Patient> Dead; // Patients that have died

	public bool IsActive = true;
	public int SpawnInterval = 10;
	private float SpawnTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
		Patients = new List<Patient>();
		Recovered = new List<Patient>();
		Dead = new List<Patient>();
	}

	private void Update()
	{
		if (IsActive)
		{
			// Spawn a new patient every spawn interval
			if (SpawnTimer >= SpawnInterval)
			{
				SpawnPatient();
				SpawnTimer = 0;
			}
			SpawnTimer += Time.deltaTime;
		}
	}

	private void SpawnPatient()
	{
		// Get a random empty room to spawn in
		Room room = FindObjectOfType<RoomManager>().GetUnoccupiedRoom();

		if (room != null)
		{
			// Spawn new patient
			Patient newPatient = Instantiate<Patient>(DefaultPatient, room.transform.position, Quaternion.identity, room.transform);

			// Randomize patient stats
			// TODO

			// Add patient to list
			Patients.Add(newPatient);

			// Register patient to room
			room.Patient = newPatient;
		}
	}
}
