using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
	private ApplicationModel.PlayerCharacter character;
	// knight
	public GameObject knightSword;
	public GameObject knightBow;
	public GameObject knightCameraHelper;

	public enum KnightWeaponMode
	{
		Sword,
		Bow
	}

	private KnightWeaponMode knightWeapon;
	private Vector3 knightCurrentPosition;
	private Quaternion knightCurrentRotation;

	// dragon
	public GameObject dragon;
	public GameObject dragonCameraHelper;

	// GUI
	public GameObject endButton;

	// Use this for initialization
	void Start ()
	{
		character = ApplicationModel.character;
		if (Debug.isDebugBuild) {
			Debug.Log ("Started game with " + character);
		}

		// add character-scripts and camera-objects depending on loaded player-character
		if (character == ApplicationModel.PlayerCharacter.Knight) {
			// add cameras
			dragonCameraHelper.SetActive (false);
			knightCameraHelper.SetActive (true);
			knightCameraHelper.transform.parent = knightSword.transform;
			knightCameraHelper.transform.position = knightSword.transform.position;
			knightCameraHelper.transform.rotation = knightSword.transform.rotation;
			// add character-scripts and define the oponent player
			knightSword.AddComponent<PlayerControlledKnightSword> ();
			knightSword.GetComponent<PlayerControlledKnightSword> ().OpponentPlayer = dragon;
			knightBow.AddComponent<PlayerControlledKnightBow> ();
			knightBow.GetComponent<PlayerControlledKnightBow> ().OpponentPlayer = dragon;
			dragon.AddComponent<ComputerControlledDragon> ();
		} else {
			// add cameras
			dragonCameraHelper.SetActive (true);
			knightCameraHelper.SetActive (false);
			dragonCameraHelper.transform.parent = dragon.transform;
			dragonCameraHelper.transform.position = dragon.transform.position;
			dragonCameraHelper.transform.rotation = dragon.transform.rotation;
			// add character-scripts and define the oponent player
			dragon.AddComponent<PlayerControlledDragon> ();
			knightSword.AddComponent<ComputerControlledKnightSword> ();
			knightSword.GetComponent<ComputerControlledKnightSword> ().OpponentPlayer = dragon;
		}
		// knight always starts standing with sword
		knightWeapon = KnightWeaponMode.Sword;
		knightSword.GetComponent<Animator> ().Play ("Knight_Stand_Sword");
	}
	
	// Update is called once per frame
	void Update ()
	{		
		if (character == ApplicationModel.PlayerCharacter.Knight) {
			// playing with knight
			// change knights weapon
			if (Input.GetButtonDown ("Fire3")) {
				if (knightWeapon == KnightWeaponMode.Sword) {
					knightWeapon = KnightWeaponMode.Bow;
					knightSword.GetComponent<Animator> ().Play ("Knight_ChangeWeapon_ToBow_Part1");
					// switch knight assets
					knightCurrentPosition = knightSword.transform.position;
					knightCurrentRotation = knightSword.transform.rotation;
					knightSword.SetActive (false);
					knightBow.SetActive (true);
					knightBow.transform.position = knightCurrentPosition;
					knightBow.transform.rotation = knightCurrentRotation;
					knightBow.GetComponent<Animator> ().Play ("Knight_ChangeWeapon_ToBow_Part2");
					// adjust camera to bow knight
					knightCameraHelper.transform.parent = knightBow.transform;
					knightCameraHelper.transform.position = knightBow.transform.position;
					knightCameraHelper.transform.rotation = knightBow.transform.rotation;
				} else {				
					knightWeapon = KnightWeaponMode.Sword;
					knightBow.GetComponent<Animator> ().Play ("Knight_ChangeWeapon_ToSword_Part1");
					// switch knight assets
					knightCurrentPosition = knightBow.transform.position;
					knightCurrentRotation = knightBow.transform.rotation;
					knightBow.SetActive (false);
					knightSword.SetActive (true);
					knightSword.transform.position = knightCurrentPosition;
					knightSword.transform.rotation = knightCurrentRotation;
					knightSword.GetComponent<Animator> ().Play ("Knight_ChangeWeapon_ToSword_Part2");
					// adjust camera to sword knight
					knightCameraHelper.transform.parent = knightSword.transform;
					knightCameraHelper.transform.position = knightSword.transform.position;
					knightCameraHelper.transform.rotation = knightSword.transform.rotation;
				}
			}

		} else if (character == ApplicationModel.PlayerCharacter.Dragon) {
			// playing with dragon

		} 

		// for both players
		// show or hide End-Button when hitting Escape
		if (Input.GetButtonDown ("Cancel")) {
			if (endButton.activeInHierarchy) {
				endButton.SetActive (false);
			} else {
				endButton.SetActive (true);
			}
		}
	}

	void OnDestroy ()
	{
//		if (Debug.isDebugBuild) {
//			Debug.Log ("Destroy");
//		}
		ApplicationModel.character = ApplicationModel.PlayerCharacter.None;
	}

	public void SwitchToStartScene ()
	{
//		if (Debug.isDebugBuild) {
//			Debug.Log ("SwitchToStartScene");
//		}
		SceneManager.LoadScene ("Start");
	}
}
