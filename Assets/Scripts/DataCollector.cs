using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
	public int DeadPatients { get; set; }
	public List<Patient> RecoveredPatients { get; private set; }

	// Start is called before the first frame update
	void Start()
    {
		// Persistent
		DontDestroyOnLoad(gameObject);

		// Initialize the recovered patient list for later use
		RecoveredPatients = new List<Patient>();
	}
}
