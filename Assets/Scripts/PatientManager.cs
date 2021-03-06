﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
	public Patient DefaultPatient;

	[HideInInspector] public List<Patient> Patients;
	[HideInInspector] public List<Patient> Recovered; // Patients that have recovered
	[HideInInspector] public List<Patient> Dead; // Patients that have died

	public int PatientsWaiting = 30;
	public bool IsActive = false;
	public int SpawnInterval = 10;
	private float SpawnTimer = 0;

	// Patient Spawn percentages
	[Range(0, 100)] public int PatientHighRiskPercentage = 20;
	[Range(0, 100)] public int PatientMediumRiskPercentage = 30;

	// Start is called before the first frame update
	void Start()
    {
		Patients = new List<Patient>();
		Recovered = new List<Patient>();
		Dead = new List<Patient>();
	}

	private void Update()
	{
		// Only spawn if there are still patients in the waiting room
		if (IsActive && PatientsWaiting > 0)
		{
			// Spawn a new patient every spawn interval
			if (SpawnTimer >= SpawnInterval)
			{
				SpawnPatient();
				SpawnTimer = 0;
			}
			SpawnTimer += Time.deltaTime;
		}

		// Handle patient dying and recovering
		HandlePatientStatus();
	}

	private void HandlePatientStatus()
	{
		// Handle recovered patients
		var deadPatients = Patients.Where(patient => patient.Hearts == 0).ToList();
		deadPatients.ForEach(patient => Patients.Remove(patient));
		Dead.AddRange(deadPatients);

		// Handle dead patients
		var recoveredPatients = Patients.Where(patient => patient.Hearts == Patient.MaxHearts).ToList();
		recoveredPatients.ForEach(patient => Patients.Remove(patient));
		Recovered.AddRange(recoveredPatients);
	}

	private void SpawnPatient()
	{
		// Get a random empty room to spawn in
		Room room = FindObjectOfType<RoomManager>().GetUnoccupiedRoom();

		if (room != null)
		{
			// Decriment number of patients in waiting room
			PatientsWaiting--;

			// Spawn new patient
			Patient newPatient = Instantiate<Patient>(DefaultPatient, room.SpawnPoint.position, Quaternion.identity, room.transform);

			// Randomize patient stats
			int conditionIndex = Random.Range(0, 100) + 1;
			if (conditionIndex <= PatientHighRiskPercentage) newPatient.Initialize(Patient.PatientCondition.HIGH);
			else if (conditionIndex <= PatientHighRiskPercentage + PatientMediumRiskPercentage) newPatient.Initialize(Patient.PatientCondition.MEDIUM);
			else newPatient.Initialize(Patient.PatientCondition.LOW);

			// Add patient to list
			Patients.Add(newPatient);

			// Flip the patient sprite if it is in rooms 4-6
			if (room.Number >= 4) newPatient.GetComponentInChildren<SpriteRenderer>().flipX = true;

			// Register patient to room
			room.Patient = newPatient;

			// Notify the player to the new patient
			FindObjectOfType<Player>()?.Emote.Display(0, room.Number);
			FindObjectOfType<AudioManager>()?.Play(Sound.SoundCategory.BUZZ);

			// Play the third message
			FindObjectOfType<AudioManager>()?.Play("message3");
		}
	}
}
