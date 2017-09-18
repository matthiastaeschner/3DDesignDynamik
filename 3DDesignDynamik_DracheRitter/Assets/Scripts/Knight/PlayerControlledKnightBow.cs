using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledKnightBow : MonoBehaviour {

	private GameManagement gameManager;

	private Animator anim;
	private CharacterController charControl;

	private float knightsRunningSpeed = 10.0f;
	private float knightsJumpingSpeed = 20.0f;
	private float knightsGravity = 50.0f;
	private Vector3 moveDirection = Vector3.zero;

	// Use this for initialization
	void Start () {
		if (Debug.isDebugBuild) {
			Debug.Log ("PlayerControlledKnightBow-Script attached to " + gameObject.name);
		}

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
			}
		}
		moveDirection.y -= knightsGravity * Time.deltaTime;
		charControl.Move (moveDirection * Time.deltaTime);
		// play run-animation while keys down, stop when keys up
		if (Input.GetKey ("up") || Input.GetKey ("down") ||
			Input.GetKey ("left") || Input.GetKey ("right")) {
			anim.Play ("Knight_Run_Bow");
		}
		if (Input.GetKeyUp ("up") || Input.GetKeyUp ("down") ||
			Input.GetKeyUp ("left") || Input.GetKeyUp ("right")) {
			anim.Play ("Knight_Stand_Bow");
		}

	}
}
