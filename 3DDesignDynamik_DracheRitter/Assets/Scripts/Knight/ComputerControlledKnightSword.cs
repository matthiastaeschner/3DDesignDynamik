using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControlledKnightSword : MonoBehaviour
{
	private Animator anim;
	private CharacterController charControl;

	private GameObject opponentPlayer;

	public GameObject OpponentPlayer {
		get {
			return opponentPlayer;
		}
		set {
			opponentPlayer = value;
		}
	}

	private float knightsRunningSpeed = 5.0f;
	private float knightsRotationSpeed = 60.0f;
	private float knightsGravity = 50.0f;

	private GameObject sword;

	// Use this for initialization
	void Start ()
	{
//		if (Debug.isDebugBuild) {
//			Debug.Log ("ComputerControlledKnight-Script attached to " + gameObject.name);
//		}
		anim = gameObject.GetComponent<Animator> ();
		anim.Play ("Knight_Stand_Sword");
		charControl = gameObject.GetComponent<CharacterController> ();

		sword = GameObject.Find ("Sword");
		sword.AddComponent<SwordHit> ();
		sword.GetComponent<SwordHit> ().OpponentPlayer = opponentPlayer;
		sword.GetComponent<SwordHit> ().Knight = gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// get the direction to opponent on ground
		Vector3 directionToOpponent = opponentPlayer.transform.position - transform.position;
		directionToOpponent.y = 0f;

		// rotate towards opponent
		Quaternion lookRotation = Quaternion.LookRotation(directionToOpponent.normalized);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * knightsRotationSpeed);

		// move to opponent if too far away but dont interrupt sword swinging
		Vector3 directionToOpponentMove = directionToOpponent.normalized * knightsRunningSpeed;
		// keep the knight on the ground
		directionToOpponentMove.y -= knightsGravity;
		if ((directionToOpponent.magnitude > 0.1f) && !anim.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Knight_Hit_Sword")) {
			anim.Play ("Knight_Run_Sword");
			charControl.Move (directionToOpponentMove * Time.deltaTime);
		}
	}

	public void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		// if the character collides with dragon while moving
		if (hit.gameObject == opponentPlayer) {
			anim.Play ("Knight_Hit_Sword");
		}
	}
}
