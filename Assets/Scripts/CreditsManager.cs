using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CreditsManager : MonoBehaviour
{
	public TMP_Text DeadCountLabel;
	public TMP_Text RecoveredList;

    // Start is called before the first frame update
    void Start()
    {
		// Set Dead count
		DeadCountLabel?.SetText(FindObjectOfType<DataCollector>()?.DeadPatients.ToString("D3"));

		// Set recovered list
		string[] patientNames = FindObjectOfType<DataCollector>()?.RecoveredPatients.Select(patient => patient.Name).ToArray();
		string recoveredPatients = string.Join("\n", patientNames);
		RecoveredList?.SetText(recoveredPatients);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
