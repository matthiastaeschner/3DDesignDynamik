using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledDragon : MonoBehaviour {

    private GameManagement gameManager;
    private Animator dragonAnimator;
    private CharacterController dragonController;

    public float dragonRunningSpeed = 10.0f;
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
	
	// Update is called once per frame
	void Update () {

        Quaternion AddRot = Quaternion.identity;
        float roll = 0;
        float pitch = 0;
        float yaw = 0;

         if(dragonController.isGrounded)
         {
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



        } else if(!dragonController.isGrounded && dragonAnimator.GetBool("isFlying") == true)
        {
            if (Input.GetButton("Jump"))
            {  
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= (dragonRunningSpeed * 2);
                moveDirection.y = dragonJumpSpeed;
                dragonController.Move(moveDirection * Time.deltaTime);
            } else
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= dragonRunningSpeed;
                moveDirection.y = -dragonJumpSpeed / 2;
                dragonController.Move(moveDirection * Time.deltaTime);
            }
            Debug.Log("Dragon is Flying!");

        }
        else
        {
            moveDirection.y = -dragonJumpSpeed;
            dragonController.Move(moveDirection * Time.deltaTime); 
        }
        

        





    }
}
