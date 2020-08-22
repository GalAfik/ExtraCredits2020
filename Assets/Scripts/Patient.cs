using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{
	// Health
	public static int MaxHearts = 4;
	public int Hearts;

	// Patient stats
	public string Name;
	public int Age;
	public PatientCondition Condition;
	public enum PatientCondition
	{
		LOW = 1, // Severity of condition
		MEDIUM = 2,
		HIGH = 3
	};
}
