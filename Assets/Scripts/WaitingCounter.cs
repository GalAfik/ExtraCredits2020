using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingCounter : MonoBehaviour
{
	public PatientManager PatientManager;
	public TMP_Text Label;

    // Update is called once per frame
    void Update()
    {
		Label?.SetText(PatientManager.PatientsWaiting.ToString("D2"));
    }
}
