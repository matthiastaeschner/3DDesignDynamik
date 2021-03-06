﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControlledDragon : MonoBehaviour {

	private Animator anim;
	private CharacterController charControl;
	private GameObject fireEffect;
	private GameObject fireEmitter;
    private AudioSource fireSource, windSource, wingSource, footstepSource;

    private AudioClip fire, wings, wind, footsteps;

    private float dragonRunningSpeed = 5.0f;
	private float dragonRotationSpeed = 5.0f;
	private float dragonGravity = 50.0f;

	private GameObject opponentPlayer;

	public GameObject OpponentPlayer {
		get {
			return opponentPlayer;
		}
		set {
			opponentPlayer = value;
		}
	}

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		charControl = gameObject.GetComponent<CharacterController> ();
		fireEmitter = GameObject.FindGameObjectWithTag("Fire");
        fire = (AudioClip)Resources.Load("Sounds/Dragon/fire");
        wind = (AudioClip)Resources.Load("Sounds/Dragon/wind");
        wings = (AudioClip)Resources.Load("Sounds/Dragon/wings");
        footsteps = (AudioClip)Resources.Load("Sounds/Dragon/footsteps");

        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.spatialBlend = 1.0f;
        footstepSource.rolloffMode = AudioRolloffMode.Logarithmic;
        footstepSource.maxDistance = 20f;
        footstepSource.minDistance = 4f;
        footstepSource.clip = footsteps;
        fireSource = gameObject.AddComponent<AudioSource>();
        fireSource.spatialBlend = 1.0f;
        fireSource.rolloffMode = AudioRolloffMode.Linear;
        fireSource.clip = fire;
        wingSource = gameObject.AddComponent<AudioSource>();
        wingSource.loop = true;
        wingSource.clip = wings;
        windSource = gameObject.AddComponent<AudioSource>();
        windSource.loop = true;
        windSource.volume = 0.2f;
        windSource.clip = wind;
        windSource.Play();

        InvokeRepeating("Fire", 0.0f, 10.0f);

    }
	
	// Update is called once per frame
	void Update () {
		if (Cursor.lockState == CursorLockMode.Locked) {
			// get the direction to opponent on ground
			Vector3 directionToOpponent = opponentPlayer.transform.position - transform.position;
			directionToOpponent.y = 0f;

			// rotate towards opponent
			Quaternion lookRotation = Quaternion.LookRotation (directionToOpponent.normalized);
			transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * dragonRotationSpeed);

			Vector3 directionToOpponentMove = directionToOpponent.normalized * dragonRunningSpeed;

			directionToOpponentMove.y -= dragonGravity;

			if (directionToOpponent.magnitude > 30f) {
				anim.SetBool ("isWalking", true);
				anim.SetBool ("isIdle", false);
				if (footstepSource.isPlaying == false) {
					footstepSource.Play ();
				}
				charControl.Move (directionToOpponentMove * Time.deltaTime);
			} else {
				footstepSource.Stop ();
				anim.SetBool ("isWalking", false);
				anim.SetBool ("isIdle", true); 
			}
		}
    }

    private void Fire()
    {
		if (Cursor.lockState == CursorLockMode.Locked) {
			if (fireSource.isPlaying == false) {
				fireSource.Play ();
			}
			fireEffect = (GameObject)Instantiate (Resources.Load ("Prefabs/FirePrefab")) as GameObject;
			fireEffect.AddComponent<FireHit> ().OpponentPlayer = opponentPlayer;
			fireEffect.GetComponent<FireHit> ().Dragon = gameObject;
			fireEffect.transform.position = fireEmitter.transform.position;
			fireEffect.transform.rotation = transform.rotation;
			Destroy (fireEffect, fireEffect.GetComponent<ParticleSystem> ().main.duration);
		}
    }
}
