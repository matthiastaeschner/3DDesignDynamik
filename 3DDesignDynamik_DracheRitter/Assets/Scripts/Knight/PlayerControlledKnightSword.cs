using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledKnightSword : MonoBehaviour
{	
	private Animator anim;
	private CharacterController charControl;

	private float knightsRunningSpeed = 20.0f;
	private float knightsJumpingSpeed = 20.0f;
	private float knightsGravity = 50.0f;
	private float knightsRotationSpeed = 40.0f;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 rotation = Vector3.zero;

	private GameObject opponentPlayer;

	public GameObject OpponentPlayer {
		get {
			return opponentPlayer;
		}
		set {
			opponentPlayer = value;
		}
	}

	private GameObject sword;

	private AudioSource audioWalk;
	private AudioSource audioGrunt;
	private AudioSource audioSwordSwing;
	private AudioClip swordSwing;
	private AudioClip walkGravel;
	private AudioClip grunt;
	private AudioSource audioPain;
	private AudioClip pain;

	public void PlayPainAudio()
	{
		if (audioPain.clip.loadState == AudioDataLoadState.Loaded) {
			audioGrunt.Pause ();
			audioSwordSwing.Pause ();
			audioPain.Play ();
		}
	}

	// Use this for initialization
	void Start ()
	{
//		if (Debug.isDebugBuild) {
//			Debug.Log ("PlayerControlledKnightSword-Script attached to " + gameObject.name);
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
		audioGrunt = gameObject.AddComponent<AudioSource> ();
		audioGrunt.playOnAwake = false;
		grunt = (AudioClip)Resources.Load ("Sounds/Knight/KnightGrunt", typeof(AudioClip));
		grunt.LoadAudioData ();
		audioGrunt.clip = grunt;
		audioGrunt.volume = 0.2f;
		audioSwordSwing = gameObject.AddComponent<AudioSource> ();
		audioSwordSwing.playOnAwake = false;
		swordSwing = (AudioClip)Resources.Load ("Sounds/Knight/SwordSwing", typeof(AudioClip));
		swordSwing.LoadAudioData ();
		audioSwordSwing.clip = swordSwing;
		audioPain = gameObject.AddComponent<AudioSource> ();
		audioPain.playOnAwake = false;
		pain = (AudioClip)Resources.Load ("Sounds/Knight/KnightPain", typeof(AudioClip));
		pain.LoadAudioData ();
		audioPain.clip = pain;
		// add collision-detection to sword and tell who is knight and who is opponent
		sword = GameObject.Find ("Sword");
		sword.AddComponent<SwordHit> ();
		sword.GetComponent<SwordHit> ().OpponentPlayer = opponentPlayer;
		sword.GetComponent<SwordHit> ().Knight = gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Cursor.lockState == CursorLockMode.Locked) {
			// move to direction / jump
			if (charControl.isGrounded) {
				moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
				moveDirection = transform.TransformDirection (moveDirection);
				moveDirection *= knightsRunningSpeed;
				if (Input.GetButton ("Jump")) {
					moveDirection.y = knightsJumpingSpeed;
					anim.Play ("Knight_Stand_Sword");
				}
			}
			moveDirection.y -= knightsGravity * Time.deltaTime;
			charControl.Move (moveDirection * Time.deltaTime);

			// play run-animation while keys down, but dont interrupt sword swinging
			if ((Input.GetButton ("Horizontal") || Input.GetButton ("Vertical")) &&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Knight_Hit_Sword")) {
				anim.Play ("Knight_Run_Sword");
			}
			if (Input.GetButtonUp ("Horizontal") || Input.GetButtonUp ("Vertical")) {
				anim.Play ("Knight_Stand_Sword");
			}

			// rotate with mouse x-axis
			rotation = new Vector3 (0, Input.GetAxis ("Mouse X"), 0) * Time.deltaTime * knightsRotationSpeed;
			gameObject.transform.Rotate (rotation);

			// hit with sword
			if (Input.GetButtonDown ("Fire1")) {
				anim.Play ("Knight_Hit_Sword");
				audioSwordSwing.Play ();
				audioGrunt.Play ();
			}

			// get shield cover
			if (Input.GetButtonDown ("Fire2")) {
				anim.Play ("Knight_ShieldCover_In");
			}

			// play walk audio
			if ((Input.GetButtonDown ("Horizontal") || Input.GetButtonDown ("Vertical")) &&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Knight_Hit_Sword")) {
				audioWalk.Play ();
			}
			if (Input.GetButtonUp ("Horizontal") || Input.GetButtonUp ("Vertical")) {
				audioWalk.Pause ();
			}
		}
	}
}
