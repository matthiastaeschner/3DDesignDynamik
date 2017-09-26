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

	private float knightsRunningSpeed = 15.0f;
	private float knightsRotationSpeed = 60.0f;
	private float knightsGravity = 50.0f;
	private bool isWalking = false;
	private bool isWalkingAudio = false;

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
        //			Debug.Log ("ComputerControlledKnight-Script attached to " + gameObject.name);
        //		}
        
		anim = gameObject.GetComponent<Animator> ();
		anim.Play ("Knight_Stand_Sword");
		charControl = gameObject.GetComponent<CharacterController> ();
		// audio sources with clips
		audioWalk = gameObject.AddComponent<AudioSource> ();
        audioWalk.spatialBlend = 1.0f;
        audioWalk.rolloffMode = AudioRolloffMode.Linear;
		audioWalk.playOnAwake = false;
		walkGravel = (AudioClip)Resources.Load ("Sounds/Knight/WalkingGravel", typeof(AudioClip));
		walkGravel.LoadAudioData ();
		audioWalk.clip = walkGravel;
		audioWalk.loop = true;
		audioWalk.volume = 0.3f;
		audioWalk.pitch = 1.5f;
		audioGrunt = gameObject.AddComponent<AudioSource> ();
		audioGrunt.playOnAwake = false;
		grunt = (AudioClip)Resources.Load ("Sounds/Knight/KnightGrunt", typeof(AudioClip));
		grunt.LoadAudioData ();
		audioGrunt.clip = grunt;
		audioGrunt.volume = 0.3f;
		audioSwordSwing = gameObject.AddComponent<AudioSource> ();
		audioSwordSwing.playOnAwake = false;
		swordSwing = (AudioClip)Resources.Load ("Sounds/Knight/SwordSwing", typeof(AudioClip));
		swordSwing.LoadAudioData ();
		audioSwordSwing.clip = swordSwing;
		audioSwordSwing.volume = 0.3f;
		audioPain = gameObject.AddComponent<AudioSource> ();
		audioPain.playOnAwake = false;
		pain = (AudioClip)Resources.Load ("Sounds/Knight/KnightPain", typeof(AudioClip));
		pain.LoadAudioData ();
		audioPain.clip = pain;

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
			isWalking = true;
			anim.Play ("Knight_Run_Sword");
			charControl.Move (directionToOpponentMove * Time.deltaTime);
		} 
		if (isWalking && !isWalkingAudio) {
			audioWalk.Play ();
			isWalkingAudio = true;
		}
	}

	public void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		// if the character collides with dragon while moving
		if (hit.gameObject == opponentPlayer) {
			anim.Play ("Knight_Hit_Sword");
			audioWalk.Pause ();
			isWalking = false;
			isWalkingAudio = false;
			audioSwordSwing.Play ();
			audioGrunt.Play ();
		}
	}
}
