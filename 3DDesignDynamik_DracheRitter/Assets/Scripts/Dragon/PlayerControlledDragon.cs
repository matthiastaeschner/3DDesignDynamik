using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledDragon : MonoBehaviour {

    private GameManagement gameManager;
    private Animator dragonAnimator;
    private CharacterController dragonController;

    public ParticleSystem fire;

    public float dragonGravity = 50.0f;
    public float dragonJumpSpeed = 20.0f;
    private Vector3 moveDirection = Vector3.zero;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameController").GetComponent<GameManagement>();
        dragonAnimator = gameObject.GetComponent<Animator>();
        dragonController = gameObject.GetComponent<CharacterController>();
        moveDirection.y = -dragonJumpSpeed;
        dragonController.Move(moveDirection * Time.deltaTime);

        // A simple particle material with no texture.
        Material particleMaterial = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

        // Create a green particle system.
        var go = new GameObject("Particle System");
        go.transform.Rotate(-90, 0, 0); // Rotate so the system emits upwards.
        fire = go.AddComponent<ParticleSystem>();
        go.GetComponent<ParticleSystemRenderer>().material = particleMaterial;
        var mainModule = fire.main;
        mainModule.startColor = Color.green;
        mainModule.startSize = 0.5f;

        InvokeRepeating("Fire", 2.0f, 2.0f);
    }
	
    void Fire()
    {
        // Any parameters we assign in emitParams will override the current system's when we call Emit.
        // Here we will override the start color and size.
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.startColor = Color.red;
        emitParams.startSize = 0.2f;
        fire.Emit(emitParams, 10);
        fire.Play(); // Continue normal emissions
    }

	// Update is called once per frame
	void Update () {

        if (dragonController.isGrounded)
         {
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
    }

    private void moveFlyingDragon()
    {
        Quaternion AddRot = Quaternion.identity;
        float roll = 0;
        float pitch = 0;
        float yaw = 0;

        float RotationSpeed = 50;

        float dragonRunningSpeed = 50f;

        roll = Input.GetAxis("Roll") * (Time.deltaTime * RotationSpeed);
        pitch = Input.GetAxis("Pitch") * (Time.deltaTime * RotationSpeed);
        // yaw = Input.GetAxis("Yaw") * (Time.deltaTime * RotationSpeed);
        Vector3 AddPos = Vector3.forward;
        gameObject.transform.Rotate(new Vector3(pitch, yaw, -roll));

        if (Input.GetButton("Jump"))
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= dragonRunningSpeed;
            moveDirection.y = dragonJumpSpeed;
            dragonController.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= dragonRunningSpeed;
            moveDirection.y = -dragonJumpSpeed / 2;
            dragonController.Move(moveDirection * Time.deltaTime);
        }
        Debug.Log("Dragon is Flying!");
    }

    private void moveGroundedDragon()
    {
        float dragonRunningSpeed = 20.0f;
        dragonAnimator.SetBool("isFlying", false);
        Debug.Log("Dragon is Grounded!");

        if (Input.GetKey("up") || Input.GetKey("down") ||
        Input.GetKey("left") || Input.GetKey("right"))
        {
            dragonAnimator.SetBool("isWalking", true);
            dragonAnimator.SetBool("isIdle", false);

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= dragonRunningSpeed;
            gameObject.transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal"), 0));
            dragonController.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            dragonAnimator.SetBool("isWalking", false);
            dragonAnimator.SetBool("isIdle", true);
        }

        if (Input.GetButton("Jump"))
        {
            dragonAnimator.SetBool("isFlying", true);
            moveDirection = Vector3.zero;
            moveDirection.y = dragonJumpSpeed;
            dragonController.Move(moveDirection * Time.deltaTime);
        }
    }
}
