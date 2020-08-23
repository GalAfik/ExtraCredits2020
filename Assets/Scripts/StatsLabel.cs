using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsLabel : MonoBehaviour
{
	// Stats
	public TMP_Text NameLabel;
	public TMP_Text DetailsLabel;
	public TMP_Text RiskLabel;
	public TMP_Text ConditionLabel;

	public void SetName(string name)
	{
		NameLabel?.SetText(name);
	}

	public void SetDetails(Patient.PatientSex sex, int age)
	{
		DetailsLabel?.SetText(sex.ToString() + ", " + age);
	}

	public void SetCondition(Patient.PatientCondition condition)
	{
		// Set text
		RiskLabel?.SetText(condition.ToString());

		// Set color
		switch (condition)
		{
			case Patient.PatientCondition.LOW:
				RiskLabel.color = Color.green;
				break;
			case Patient.PatientCondition.MEDIUM:
				RiskLabel.color = Color.yellow;
				break;
			case Patient.PatientCondition.HIGH:
				RiskLabel.color = Color.red;
				break;
		}
	}

	public void SetConditionName(string condition)
	{
		ConditionLabel?.SetText(condition);
	}
}
