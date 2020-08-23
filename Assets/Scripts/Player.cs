﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
	// Components
	private CharacterController Controller;
	private Animator Animator;
	[HideInInspector] public Emote Emote;

	// State
	public enum PlayerState { FREE, HOLD };
	private PlayerState State = PlayerState.FREE;

	// Medicine
	public Medicine Medicine;
	public Pharmacy CollidingPharmacy;

	// Movement
	public float WalkSpeed = 3f;
	public float RunSpeed = 5f;
	private bool Flipped = false;

	// Input
	public float KeyHoldTime = 2f;
	public float KeyPressThreshold = .5f;
	private float KeyTimer = 0f;
	private bool HoldFired = false;

	// Stress
	public float MaxStress = 30f;
	public float Stress { get; private set; }
	public float StressIncreaseRate = 1;
	public float StressDecreaseRate = 2;
	private bool Resting = false;

	// Emotes
	[Header("Emotes")]
	public int RestingEmote;
	public int HappyEmote;
	public int SadEmote;
	public int StressedEmote;

	// Patient near player
	private Patient CurrentPatient;

    // Start is called before the first frame update
    void Start()
    {
		Controller = GetComponent<CharacterController>();
		Animator = GetComponent<Animator>();
		Emote = GetComponentInChildren<Emote>();

		// Zero out stress
		Stress = 0;
    }

    // Update is called once per frame
    void Update()
    {
		HandleMovement();
		HandleInput();
		HandleStress();
    }

	private void HandleStress()
	{
		// Change stress level
		if (Resting) Stress -= StressDecreaseRate * Time.deltaTime;
		else Stress += StressIncreaseRate * Time.deltaTime;

		// Insure Stress bounds
		if (Stress > MaxStress) Stress = MaxStress;
		if (Stress < 0) Stress = 0;

		// Display an emote when the player reaches different levels of stress
		if (Stress == MaxStress) Emote?.Display(StressedEmote);
	}

	private void HandleAnimation(Vector3 velocity)
	{
		// Handle walk/run animations
		Animator?.SetFloat("Speed", velocity.magnitude);

		// Flip the sprite to face the correct movement direction
		if (Mathf.Abs(velocity.x) > 0) Flipped = velocity.x < 0;
		GetComponentInChildren<SpriteRenderer>().flipX = Flipped;
	}

	private void HandleMovement()
	{
		if (State == PlayerState.FREE)
		{
			// Get movement input
			Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			float speed = velocity.magnitude > .5f ? RunSpeed : WalkSpeed;

			// Handle movement animations
			HandleAnimation(velocity * speed);

			// Apply velocity to the player
			Controller.Move(velocity.normalized * speed * Time.deltaTime);

			// Apply gravity to player
			if (Controller.isGrounded == false) Controller.Move(Physics.gravity * Time.deltaTime);
		}
	}

	private void HandleInput()
	{
		// First check if the button has been held long enough
		if (KeyTimer >= KeyHoldTime && HoldFired == false)
		{
			// Trigger Hold Action
			ActionHold();

			// Reset Timer
			KeyTimer = 0;
			HoldFired = true;
			State = PlayerState.FREE;

			// Skip the rest of the input
			return;
		}

		// Check if action button is being held down
		if (Input.GetButton("Action"))
		{
			// Holding down button
			KeyTimer += Time.deltaTime;

			// If the player holds the button long enough, stop movement
			if (KeyTimer > KeyPressThreshold) State = PlayerState.HOLD;
		}

		// Check for button release
		if (Input.GetButtonUp("Action"))
		{
			// Check if the button was only held down for a short time
			if (KeyTimer > 0 && KeyTimer < KeyPressThreshold && HoldFired == false)
			{
				// Assume normal button press
				ActionPress();
			}

			// Reset timer
			KeyTimer = 0;
			HoldFired = false;
			State = PlayerState.FREE;
		}
	}

	private void ActionHold()
	{
		// Pick up medicine from the pharmacy
		if (CollidingPharmacy != null && Medicine == null)
		{
			// Spawn a new medicine object
			if (CollidingPharmacy.Medicine != null)
			{
				var newMedicine = Instantiate(CollidingPharmacy.Medicine, transform);
				// Attach the new medicine to this player
				PickUpMedicine(newMedicine);
			}
		}

		// Diagnose a patient
		if (CurrentPatient != null)
		{
			CurrentPatient.DisplayStats();
		}
	}

	private void ActionPress()
	{
		// Drop/Pick up medicine
		if (Medicine != null)
		{
			Medicine.transform.parent = null;
			Medicine = null;
		}
		else
		{
			// Check if standing inside a medicine trigger zone
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, .01f);
			foreach (var hitCollider in hitColliders)
			{
				// Check for medicine
				if (hitCollider.GetComponent<Medicine>() != null)
				{
					// Pick up the first medicine found
					PickUpMedicine(hitCollider.GetComponent<Medicine>());
					return;
				}
			}
		}
	}

	private void PickUpMedicine(Medicine medicine)
	{
		Medicine = medicine;
		medicine.transform.parent = transform;
		medicine.transform.position = new Vector3(medicine.transform.position.x, medicine.transform.position.y + 1, medicine.transform.position.z);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Pharmacy>() != null) CollidingPharmacy = other.GetComponent<Pharmacy>();
		if (other.GetComponent<RestZone>() != null)
		{
			Resting = true;
			// Display a resting emote
			Emote?.Display(RestingEmote);
		}
		if (other.GetComponent<Patient>() != null) CurrentPatient = other.GetComponent<Patient>();
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Pharmacy>() != null) CollidingPharmacy = null;
		if (other.GetComponent<RestZone>() != null) Resting = false;
		if (other.GetComponent<Patient>() != null) CurrentPatient = null;
	}
}
