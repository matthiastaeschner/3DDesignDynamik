using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledKnightBow : MonoBehaviour
{
	private Animator anim;
	private CharacterController charControl;

	private float knightsRunningSpeed = 20.0f;
	private float knightsJumpingSpeed = 20.0f;
	private float knightsGravity = 50.0f;
	private float knightsRotationSpeed = 60.0f;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float arrowSpeed = 70.0f;

	private GameObject opponentPlayer;

	public GameObject OpponentPlayer {
		get {
			return opponentPlayer;
		}
		set {
			opponentPlayer = value;
		}
	}

	private AudioSource audioWalk;
	private AudioSource audioBowShoot;
	private AudioClip bowShoot;
	private AudioClip walkGravel;
	private AudioSource audioPain;
	private AudioClip pain;

	public void PlayPainAudio()
	{
		if (audioPain.clip.loadState == AudioDataLoadState.Loaded) {
			audioPain.Play ();
		}
	}

	// Use this for initialization
	void Start ()
	{
//		if (Debug.isDebugBuild) {
//			Debug.Log ("PlayerControlledKnightBow-Script attached to " + gameObject.name);
//		}
		anim = gameObject.GetComponent<Animator> ();
		charControl = gameObject.GetComponent<CharacterController> ();
		// audio sources with clips
		audioWalk = gameObject.AddComponent<AudioSource> ();
		audioWalk.playOnAwake = false;
		walkGravel = (AudioClip)Resources.Load ("Sounds/Knight/WalkingGravel", typeof(AudioClip));
		walkGravel.LoadAudioData ();
		audioWalk.clip = walkGravel;
		audioWalk.pitch = 1.5f;
		audioBowShoot = gameObject.AddComponent<AudioSource> ();
		audioBowShoot.playOnAwake = false;
		bowShoot = (AudioClip)Resources.Load ("Sounds/Knight/ArrowShoot", typeof(AudioClip));
		bowShoot.LoadAudioData ();
		audioBowShoot.clip = bowShoot;
		audioPain = gameObject.AddComponent<AudioSource> ();
		audioPain.playOnAwake = false;
		pain = (AudioClip)Resources.Load ("Sounds/Knight/KnightPain", typeof(AudioClip));
		pain.LoadAudioData ();
		audioPain.clip = pain;
	}

	// Update is called once per frame
	void Update ()
	{
		// move to direction / jump
		if (charControl.isGrounded) {
			moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
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
		rotation = new Vector3 (0, Input.GetAxis ("Mouse X"), 0) * Time.deltaTime * knightsRotationSpeed;
		gameObject.transform.Rotate (rotation);

		// load bow by holding left mouse
		if (Input.GetButton ("Fire1")) {
			anim.Play ("Knight_LoadBow");
		}
		// shoot bow by let left mouse up
		if (Input.GetButtonUp ("Fire1")) {
			anim.Play ("Knight_ShootBow");
			// create an instance from the arrow-prefab in resources-folder
			GameObject arrowClone = (GameObject)Instantiate (Resources.Load ("Prefabs/Arrow", typeof(GameObject)));
			// add collision-detection to arrow and tell who is oponent
			arrowClone.AddComponent<ArrowHit> ().OpponentPlayer = opponentPlayer;
			// set arrows startpoint in front of bow and in knights looking direction
			Vector3 arrowStartPos = gameObject.transform.position + new Vector3 (0f, 6f) + gameObject.transform.forward * 4f;
			arrowClone.transform.position = arrowStartPos;
			arrowClone.transform.rotation = gameObject.transform.rotation;
			// adjust arrows rotation to fly a little upwards
			arrowClone.transform.rotation *= Quaternion.AngleAxis (-5f, arrowClone.transform.right);
			arrowClone.GetComponent<Rigidbody> ().velocity = arrowClone.transform.forward * arrowSpeed;
			audioBowShoot.Play ();
		}

		// play walk audio
		if ((Input.GetButtonDown ("Horizontal") || Input.GetButtonDown ("Vertical"))) {
			audioWalk.Play ();
		}
		if (Input.GetButtonUp ("Horizontal") || Input.GetButtonUp ("Vertical")) {
			audioWalk.Pause ();
		}
	}
}
