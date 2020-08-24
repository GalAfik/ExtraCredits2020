using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
	// Components
	private CharacterController Controller;
	private Animator Animator;
	[HideInInspector] public Emote Emote;
	public Controls Controls;

	// Medicine
	public Medicine Medicine;
	private bool HoldingMedicine;
	private Pharmacy CollidingPharmacy;
	public GameObject MedicineUI;

	// Movement
	public float WalkSpeed = 3f;
	public float RunSpeed = 5f;
	private bool Flipped = false;
	public bool CanMove;
	private Vector3 Velocity;

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

		// Disable the picked up medicine UI
		MedicineUI.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
		if (CanMove) HandleMovement();
		HandleInput();
		HandleStress();
    }

	private void HandleStress()
	{
		// Change stress level
		if (Resting) Stress -= StressDecreaseRate * Time.deltaTime;
		else if (Velocity.magnitude == 0) Stress -= Time.deltaTime;
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
		if (Stress >= MaxStress) Animator?.SetFloat("Speed", velocity.magnitude / 2);
		else Animator?.SetFloat("Speed", velocity.magnitude);

		// Flip the sprite to face the correct movement direction
		if (Mathf.Abs(velocity.x) > 0) Flipped = velocity.x < 0;
		GetComponentInChildren<SpriteRenderer>().flipX = Flipped;
	}

	private void HandleMovement()
	{
		// Get movement input
		Velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		float speed = Velocity.magnitude > .5f ? RunSpeed : WalkSpeed;

		// Limit player to walking if under too much stress
		if (Stress >= MaxStress) speed = WalkSpeed;

		// Handle movement animations
		HandleAnimation(Velocity.normalized * speed);

		// Apply velocity to the player
		Controller.Move(Velocity.normalized * speed * Time.deltaTime);

		// Apply gravity to player
		if (Controller.isGrounded == false) Controller.Move(Physics.gravity * Time.deltaTime);
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

			// Skip the rest of the input
			return;
		}

		// Check if action button is being held down
		if (Input.GetButton("Action"))
		{
			// Holding down button
			KeyTimer += Time.deltaTime;
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
		}
	}

	private void ActionHold()
	{
		Controls.ShowHold(false);

		// Pick up medicine from the pharmacy
		if (CollidingPharmacy != null && !HoldingMedicine) PickUpMedicine();

		// Diagnose a patient
		if (CurrentPatient != null) CurrentPatient.DisplayStats();
	}

	private void ActionPress()
	{
		// Give patient medicine
		if (CurrentPatient != null && HoldingMedicine)
		{
			GiveMedicine(CurrentPatient);
		}

		// Drop/Pick up medicine
		if (HoldingMedicine)
		{
			DropMedicine();
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

	private void PickUpMedicine(Medicine medicine = null)
	{
		// Destroy the picked up medicine if it exists
		if (medicine != null) Destroy(medicine.gameObject);
		HoldingMedicine = true;
		MedicineUI.SetActive(true);

		// Dismiss the control prompt
		Controls.ShowPress(false);
	}

	private void DropMedicine()
	{
		// Spawn new medicine and drop it
		Vector2 randomCircle = Random.insideUnitCircle.normalized;
		Vector3 dropPosition = new Vector3(transform.position.x + randomCircle.x, 0, transform.position.z + randomCircle.y);
		Instantiate(Medicine, dropPosition, Quaternion.identity);
		HoldingMedicine = false;
		MedicineUI.SetActive(false);
	}

	private void GiveMedicine(Patient patient)
	{
		HoldingMedicine = false;
		patient.TakeMedicine();

		// UI
		Controls.ShowPress(false);
		MedicineUI.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Medicine>() != null)
		{
			Controls.ShowPress(true);
		}
		if (other.GetComponent<Pharmacy>() != null)
		{
			CollidingPharmacy = other.GetComponent<Pharmacy>();
			Controls.ShowHold(true);
		}
		if (other.GetComponent<RestZone>() != null)
		{
			Resting = true;
			// Display a resting emote
			Emote?.Display(RestingEmote);
		}
		if (other.GetComponent<Patient>() != null)
		{
			CurrentPatient = other.GetComponent<Patient>();
			Controls.ShowHold(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Medicine>() != null)
		{
			Controls.ShowPress(false);
		}
		if (other.GetComponent<Pharmacy>() != null)
		{
			CollidingPharmacy = null;
			Controls.ShowHold(false);
		}
		if (other.GetComponent<RestZone>() != null)
		{
			Resting = false;
		}
		if (other.GetComponent<Patient>() != null)
		{
			CurrentPatient = null;
			Controls.ShowHold(false);
		}
	}
}
