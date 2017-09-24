using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledDragon : MonoBehaviour {

    private GameManagement gameManager;
    private Animator dragonAnimator;
    private CharacterController dragonController;

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
    }
	
    void Fire()
    {
        SpecialEffectsHelper.Instance.Explosion(gameObject.transform.position);
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

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Button down!");
            Fire();
        }
    }

    private void moveFlyingDragon()
    {
        Quaternion AddRot = Quaternion.identity;
        float roll = 0;
        float pitch = 0;
        float yaw = 0;

        float RotationSpeed = 50;

        float dragonRunningSpeed = 0.50f;

        roll = Input.GetAxis("Mouse X") * (Time.deltaTime * RotationSpeed);
        pitch = Input.GetAxis("Mouse Y") * (Time.deltaTime * RotationSpeed);
        //yaw = Input.GetAxis("Yaw") * (Time.deltaTime * RotationSpeed);
        Vector3 AddPos = Vector3.forward;
        gameObject.transform.Rotate(new Vector3(pitch, yaw, -roll));
        moveDirection = (new Vector3(Input.GetAxis("Horizontal"), 0, 90) * (dragonRunningSpeed));
        moveDirection *= dragonRunningSpeed;

        if (Input.GetButton("Jump"))
        {
            dragonAnimator.SetBool("flyIdle", false);
            moveDirection.y = dragonJumpSpeed;
            
        }
        else
        {
            dragonAnimator.SetBool("flyIdle", true);
            moveDirection.y = -dragonJumpSpeed;   
        }

        moveDirection = transform.TransformDirection(moveDirection);
        dragonController.Move(moveDirection * Time.deltaTime);
        Debug.Log("Dragon is Flying!");
    }

    private void moveGroundedDragon()
    {
        float dragonRunningSpeed = 10.0f;
        dragonAnimator.SetBool("isFlying", false);
        Debug.Log("Dragon is Grounded!");

        if (Input.GetButton("Jump"))
        {
            dragonAnimator.SetBool("isFlying", true);
            moveDirection = Vector3.zero;
            moveDirection.y = dragonJumpSpeed;
            dragonController.Move(moveDirection * Time.deltaTime);
        }

        if (Input.GetKey("w") || Input.GetKey("s"))
        {
            dragonAnimator.SetBool("isWalking", true);
            dragonAnimator.SetBool("isIdle", false);

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= dragonRunningSpeed;
           
            dragonController.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            dragonAnimator.SetBool("isWalking", false);
            dragonAnimator.SetBool("isIdle", true);
        }

        gameObject.transform.RotateAroundLocal(new Vector3(0, 1, 0), Input.GetAxis("Mouse X") * 0.02f);


    }
}
