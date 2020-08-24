using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Room : MonoBehaviour
{
	public Door Door;
	public Transform SpawnPoint;
	public int Number;
	public bool IsPlayerInRoom;
	public bool IsOccupied
	{
		get
		{
			return patient != null;
		}
		private set
		{
			IsOccupied = value;
		}
	}
	private Patient patient;
	public Patient Patient
	{
		get
		{
			return patient;
		}
		set
		{
			if (value == null) // Clearing the room
			{
				patient = null;
				Door?.Close();
			}
			else if (patient == null) // Setting the patient when none is in the room
			{
				patient = value;
				Door?.Open();
			}
			else // Trying to set a patient to this occupied room
			{
				throw new System.Exception("Room Error: Room " + Number + " is already occupied. Patient " + value.Name + " cannot be spawned in this room.");
			}
		}
	}

	public TMP_Text Label;

	private void Start()
	{
		// Set the label text
		Label.text = Number.ToString();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Player>() != null)
		{
			IsPlayerInRoom = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Player>() != null)
		{
			IsPlayerInRoom = false;
		}
	}
}
