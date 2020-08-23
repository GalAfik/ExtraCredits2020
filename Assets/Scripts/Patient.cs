using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{
	// Components
	private Animator Animator;
	private Animator StatsAnimator;

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
	[HideInInspector] public Emote Emote;

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
	public float HeartDecreaseTime = 20f;
	private bool HeartsDecreasing = false;
	private float HeartTimer;

	// Emotes
	public int RecoveredEmote;
	public int DeadEmote;

	private void Awake()
	{
		// Get components
		Animator = GetComponent<Animator>();
		StatsAnimator = StatsLabel?.GetComponent<Animator>();
		Emote = GetComponentInChildren<Emote>();

		// Read in names and conditions from json files
		Names = JsonUtility.FromJson<PatientNames>(jsonPatientNames.text);
		Conditions = JsonUtility.FromJson<ConditionNames>(jsonConditionNames.text);
	}

	private void Update()
	{
		// Handle heart loss
		if (HeartsDecreasing)
		{
			if (HeartTimer <= 0 && Hearts > 0)
			{
				// Decrement hearts
				Hearts--;
				// Reset timer
				HeartTimer = HeartDecreaseTime;
				// Change the condition to match the hearts
				switch (Hearts)
				{
					case 1:
						Condition = PatientCondition.HIGH;
						break;
					case 2:
						Condition = PatientCondition.MEDIUM;
						break;
					case 3:
						Condition = PatientCondition.LOW;
						break;
				}
				// Update the stats label
				StatsLabel?.SetHearts(Hearts);
				StatsLabel?.SetCondition(Condition);
			}

			// Decrement timer
			HeartTimer -= Time.deltaTime;
		}
	}

	public void DisplayStats()
	{
		StatsAnimator?.SetTrigger("Display");
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

		// Set condition name based on condition
		switch (Condition)
		{
			case PatientCondition.LOW:
				ConditionName = Conditions.low[UnityEngine.Random.Range(0, Conditions.low.Length)];
				break;
			case PatientCondition.MEDIUM:
				ConditionName = Conditions.medium[UnityEngine.Random.Range(0, Conditions.medium.Length)];
				break;
			case PatientCondition.HIGH:
				ConditionName = Conditions.high[UnityEngine.Random.Range(0, Conditions.high.Length)];
				break;
		}

		// Set hearts based on condition
		Hearts = MaxHearts - (int) Condition;

		// Start the heart decrease timer
		HeartsDecreasing = true;
		HeartTimer = HeartDecreaseTime;

		// Set the labels
		StatsLabel?.SetName(Name);
		StatsLabel?.SetDetails(Sex, Age);
		StatsLabel?.SetCondition(Condition);
		StatsLabel?.SetConditionName(ConditionName);
		StatsLabel?.SetHearts(Hearts);
	}
}
