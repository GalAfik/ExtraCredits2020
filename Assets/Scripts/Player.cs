using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
	// Components
	CharacterController Controller;

	// State
	public enum PlayerState { FREE, HOLD };
	private PlayerState State = PlayerState.FREE;

	// Movement
	public float WalkSpeed = 3f;
	public float RunSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
		Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
		HandleMovement();
    }

	private void HandleMovement()
	{
		// Get movement input
		Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		float speed = velocity.magnitude > .5f ? RunSpeed : WalkSpeed;

		// Apply velocity to the player
		Controller.Move(velocity.normalized * speed * Time.deltaTime);
	}
}
