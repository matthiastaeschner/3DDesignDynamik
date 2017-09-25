using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledDragon : MonoBehaviour {

    private GameManagement gameManager;
    private Animator dragonAnimator;
    private CharacterController dragonController;
    private GameObject fireEffect;
    private GameObject fireEmitter;

    public float dragonGravity = 50.0f;
    public float dragonJumpSpeed = 20.0f;
	private float RotationSpeed = 50;
    private Vector3 moveDirection = Vector3.zero;
	private Vector3 initialRotation;
    private AudioSource fireSource, windSource, wingSource;

    private AudioClip fire, wings, wind;

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
	void Start ()
    {
        gameManager = GameObject.Find("GameController").GetComponent<GameManagement>();
        dragonAnimator = gameObject.GetComponent<Animator>();
        dragonController = gameObject.GetComponent<CharacterController>();
        moveDirection.y = -dragonJumpSpeed;
        dragonController.Move(moveDirection * Time.deltaTime);
        fireEmitter = GameObject.FindGameObjectWithTag("Fire");
		initialRotation = gameObject.transform.rotation.eulerAngles;
        fire = (AudioClip)Resources.Load("Sounds/Dragon/fire");
        wind = (AudioClip)Resources.Load("Sounds/Dragon/wind");
        wings = (AudioClip)Resources.Load("Sounds/Dragon/wings");

        fireSource = gameObject.AddComponent<AudioSource>();
        fireSource.clip = fire;
        wingSource = gameObject.AddComponent<AudioSource>();
        wingSource.loop = true;
        wingSource.clip = wings;
        windSource = gameObject.AddComponent<AudioSource>();
        windSource.loop = true;
        windSource.volume = 0.2f;
        windSource.clip = wind;
        windSource.Play();


    }

	// Update is called once per frame
	void Update () {

        if (dragonController.isGrounded)
         {
			Quaternion tempRot = Quaternion.Euler (new Vector3(initialRotation.x, gameObject.transform.rotation.eulerAngles.y, initialRotation.z));
			gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, tempRot, 5f * Time.deltaTime);
            moveGroundedDragon();

        } else if(!dragonController.isGrounded && dragonAnimator.GetBool("isFlying") == true)
        {
            moveFlyingDragon();
        }
        else
        {
            moveDirection.y = -dragonJumpSpeed;
            dragonController.Move(moveDirection * Time.deltaTime); 
        }

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

    }

    private void Fire()
    {
        fireSource.Play();
        
        fireEffect = (GameObject) Instantiate(Resources.Load("Prefabs/FirePrefab")) as GameObject;
		fireEffect.AddComponent<FireHit> ().OpponentPlayer = opponentPlayer;
		fireEffect.GetComponent<FireHit> ().Dragon = gameObject;
        fireEffect.transform.position = fireEmitter.transform.position;
        Destroy(fireEffect, fireEffect.GetComponent<ParticleSystem>().main.duration);
    }

    private void moveFlyingDragon()
    {
        Quaternion AddRot = Quaternion.identity;
        float roll = 0;
        float pitch = 0;
        float yaw = 0;
        float dragonRunningSpeed = 0.50f;

        roll = Input.GetAxis("Mouse X") * (Time.deltaTime * RotationSpeed);
        pitch = Input.GetAxis("Mouse Y") * (Time.deltaTime * RotationSpeed);
        yaw = Input.GetAxis("Horizontal") * (Time.deltaTime * RotationSpeed);
        Vector3 AddPos = Vector3.forward;
        gameObject.transform.Rotate(new Vector3(pitch, yaw, -roll));
        moveDirection = (new Vector3(Input.GetAxis("Horizontal"), 0, 90) * (dragonRunningSpeed));
        moveDirection *= dragonRunningSpeed;

        if (Input.GetButton("Jump"))
        {
            
            if(wingSource.isPlaying == false)
            {
                wingSource.Play();
            }
            dragonAnimator.SetBool("flyIdle", false);
            moveDirection.y = dragonJumpSpeed;
        }
        else
        {
            wingSource.Stop();
            dragonAnimator.SetBool("flyIdle", true);
            moveDirection.y = -dragonJumpSpeed;   
        }

        moveDirection = transform.TransformDirection(moveDirection);
        dragonController.Move(moveDirection * Time.deltaTime);
        
    }

    private void moveGroundedDragon()
    {
        float dragonRunningSpeed = 10.0f;
        dragonAnimator.SetBool("isFlying", false);

        if (Input.GetButton("Jump"))
        {
            dragonAnimator.SetBool("isFlying", true);
            moveDirection = Vector3.zero;
            moveDirection.y = dragonJumpSpeed;
            dragonController.Move(moveDirection * Time.deltaTime);
        }

        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("d"))
        {
            dragonAnimator.SetBool("isWalkingBackwards", false);
            dragonAnimator.SetBool("isWalking", true);
            dragonAnimator.SetBool("isIdle", false);

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= dragonRunningSpeed;
           
            dragonController.Move(moveDirection * Time.deltaTime);
        }

        else if(Input.GetKey("s")){
            dragonAnimator.SetBool("isWalking", false);
            dragonAnimator.SetBool("isWalkingBackwards", true);
            dragonAnimator.SetBool("isIdle", false);

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= dragonRunningSpeed;

            dragonController.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            dragonAnimator.SetBool("isWalking", false);
            dragonAnimator.SetBool("isWalkingBackwards", false);
            dragonAnimator.SetBool("isIdle", true);
        }

        gameObject.transform.RotateAroundLocal(new Vector3(0, 1, 0), Input.GetAxis("Mouse X") * 0.02f);


    }
}
