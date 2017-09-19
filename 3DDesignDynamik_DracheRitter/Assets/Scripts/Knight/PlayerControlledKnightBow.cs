﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledKnightBow : MonoBehaviour {

	private GameManagement gameManager;

	private Animator anim;
	private CharacterController charControl;

	private float knightsRunningSpeed = 10.0f;
	private float knightsJumpingSpeed = 20.0f;
	private float knightsGravity = 50.0f;
	private float knightsRotationSpeed = 50.0f;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 rotation = Vector3.zero;

	// Use this for initialization
	void Start () {
//		if (Debug.isDebugBuild) {
//			Debug.Log ("PlayerControlledKnightBow-Script attached to " + gameObject.name);
//		}

		gameManager = GameObject.Find ("GameController").GetComponent<GameManagement> ();
		anim = gameObject.GetComponent<Animator> ();
		charControl = gameObject.GetComponent<CharacterController> ();
	}

	// Update is called once per frame
	void Update () {
		// move to direction / jump
		if (charControl.isGrounded) {
			moveDirection = new Vector3 (Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection (moveDirection);
			moveDirection *= knightsRunningSpeed;
			if (Input.GetButton ("Jump")) {
				moveDirection.y = knightsJumpingSpeed;
				anim.Play ("Knight_Stand_Bow");
			}
		}
		moveDirection.y -= knightsGravity * Time.deltaTime;
		charControl.Move (moveDirection * Time.deltaTime);
		// play run-animation while keys down, stop when keys up
		if (Input.GetButton ("Horizontal") || Input.GetButton ("Vertical")) {
			anim.Play ("Knight_Run_Bow");
		}
		if (Input.GetButtonUp ("Horizontal") || Input.GetButtonUp ("Vertical")) {
			anim.Play ("Knight_Stand_Bow");
		}
		// rotate with mouse x-axis
		rotation = new Vector3(0, Input.GetAxis("Mouse X"),0) * Time.deltaTime * knightsRotationSpeed;
		gameObject.transform.Rotate(rotation);

		// load bow by holding left mouse
		if (Input.GetButton ("Fire1")) {
			anim.Play ("Knight_LoadBow");
		}
		// shoot bow by let left mouse up
		if (Input.GetButtonUp ("Fire1")) {
			anim.Play ("Knight_ShootBow");
		}
	}
}
