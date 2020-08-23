using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{
	// Components
	private Animator Animator;

	// Patient resource json file
	public class PatientNames
	{
		public string[] family;
		public string[] male;
		public string[] female;
	}

	public class ConditionNames
	{
		public string[] high;
		public string[] medium;
		public string[] low;
	}
	public TextAsset jsonPatientNames;
	private PatientNames Names;
	public TextAsset jsonConditionNames;
	private ConditionNames Conditions;

	// Patient stats
	public string Name { get; private set; }
	public int Age { get; private set; }
	public PatientSex Sex { get; private set; }
	public enum PatientSex { M, F };
	public PatientCondition Condition { get; private set; }
	public enum PatientCondition { LOW = 1, MEDIUM = 2, HIGH = 3 };
	public string ConditionName { get; private set; }

	// Stats label
	public StatsLabel StatsLabel;

	// Health
	public static int MaxHearts = 4;
	public int Hearts { get; private set; }

	// Patient heart decrease time
	public float HeartDecreaseTime_Low = 20f;
	public float HeartDecreaseTime_Medium = 15f;
	public float HeartDecreaseTime_High = 10f;
	private float HeartDecreaseTime;
	private float HeartTimer;

	private void Awake()
	{
		// Get components
		Animator = GetComponent<Animator>();

		// Read in names and conditions from json files
		Names = JsonUtility.FromJson<PatientNames>(jsonPatientNames.text);
		Conditions = JsonUtility.FromJson<ConditionNames>(jsonConditionNames.text);

		// Zero out heart decrease timer
		HeartTimer = 0;
	}

	private void Update()
	{
		// Handle heart loss
		if (HeartTimer <= 0 && Hearts > 0)
		{
			// Decrement hearts
			Hearts--;
			// Reset timer
			HeartTimer = HeartDecreaseTime;
			// Update the stats label
			StatsLabel?.SetHearts(Hearts);
		}
		else if (HeartDecreaseTime != 0) // Make sure that the object has been initialized first
		{
			// Decrement timer
			HeartTimer -= Time.deltaTime;
		}
	}

	public void Initialize(PatientCondition condition = PatientCondition.LOW)
	{
		// Set Sex
		Sex = (PatientSex) UnityEngine.Random.Range(0, Enum.GetValues(typeof(PatientSex)).Length);

		// Set Name based on Sex
		Name = Names.family[UnityEngine.Random.Range(0, Names.family.Length)] + ", ";
		switch (Sex)
		{
			case PatientSex.M:
				Name += Names.male[UnityEngine.Random.Range(0, Names.male.Length)];
				break;
			case PatientSex.F:
				Name += Names.female[UnityEngine.Random.Range(0, Names.female.Length)];
				break;
		}

		// Set Age
		Age = UnityEngine.Random.Range(8, 76);

		// Set Sprite based on sex and age
		if (Age <= 20)
		{
			// Set a child sprite
			if (Sex == PatientSex.M) Animator?.SetInteger("Choice", 6);
			else Animator?.SetInteger("Choice", 7);
		}
		else if (Age >= 50)
		{
			// Set an elder sprite
			if (Sex == PatientSex.M) Animator?.SetInteger("Choice", 4);
			else Animator?.SetInteger("Choice", 5);
		}
		else
		{
			// Set an adult sprite
			if (Sex == PatientSex.M)
			{
				// Pick a random adult male sprite - there are currently two set
				Animator?.SetInteger("Choice", UnityEngine.Random.Range(1, 3));
			}
			else Animator?.SetInteger("Choice", 3);
		}

		// Set condition
		Condition = condition;

		// Set condition name and heart decrease time based on condition
		switch (Condition)
		{
			case PatientCondition.LOW:
				ConditionName = Conditions.low[UnityEngine.Random.Range(0, Conditions.low.Length)];
				HeartDecreaseTime = HeartDecreaseTime_Low;
				break;
			case PatientCondition.MEDIUM:
				ConditionName = Conditions.medium[UnityEngine.Random.Range(0, Conditions.medium.Length)];
				HeartDecreaseTime = HeartDecreaseTime_Medium;
				break;
			case PatientCondition.HIGH:
				ConditionName = Conditions.high[UnityEngine.Random.Range(0, Conditions.high.Length)];
				HeartDecreaseTime = HeartDecreaseTime_High;
				break;
		}

		// Set hearts based on condition
		Hearts = MaxHearts - (int) Condition;

		// Set the labels
		StatsLabel?.SetName(Name);
		StatsLabel?.SetDetails(Sex, Age);
		StatsLabel?.SetCondition(Condition);
		StatsLabel?.SetConditionName(ConditionName);
		StatsLabel?.SetHearts(Hearts);
	}
}
