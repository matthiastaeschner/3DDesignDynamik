using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	public Button endButton;
	public Text playersLifeText;
	public Image playersLifeBar;
	public Text opponentsLifeText;
	public Image opponentsLifeBar;
	public Button winOrLoseButton;
	private GameObject opponent;

    // Use this for initialization
    void Start ()
	{
        Cursor.lockState = CursorLockMode.Locked;
		character = ApplicationModel.character;
		if (Debug.isDebugBuild) {
			Debug.Log ("Started game with " + character);
		}
        
		// add character-scripts and camera-objects depending on loaded player-character
		if (character == ApplicationModel.PlayerCharacter.Knight) {
			// switch cameras
			dragonCameraHelper.SetActive (false);
			knightCameraHelper.SetActive (true);
			// add character-scripts and define the oponent player
			// knight with sword
			knightSword.AddComponent<PlayerControlledKnightSword> ();
			knightSword.GetComponent<PlayerControlledKnightSword> ().OpponentPlayer = dragon;
			// knight with bow
			knightBow.AddComponent<PlayerControlledKnightBow> ();
			knightBow.GetComponent<PlayerControlledKnightBow> ().OpponentPlayer = dragon;
			// dragon
			dragon.AddComponent<ComputerControlledDragon> ();
			dragon.GetComponent<ComputerControlledDragon> ().OpponentPlayer = knightSword;
			// set text and life
			playersLifeText.text = "Ritter";
			opponentsLifeText.text = "Drache";
			opponent = dragon;
		} else {
            // switch cameras
            dragonCameraHelper.SetActive(true);
			knightCameraHelper.SetActive (false);
			// add character-scripts and define the oponent player
			// dragon
			dragon.AddComponent<PlayerControlledDragon> ();
			dragon.GetComponent<PlayerControlledDragon> ().OpponentPlayer = knightSword;
			// knight with sword
			knightSword.AddComponent<ComputerControlledKnightSword> ();
			knightSword.GetComponent<ComputerControlledKnightSword> ().OpponentPlayer = dragon;
			// set text and life
			playersLifeText.text = "Drache";
			opponentsLifeText.text = "Ritter";
			opponent = knightSword;
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
					// tell dragon the new opponent
					dragon.GetComponent<ComputerControlledDragon> ().OpponentPlayer = knightBow;
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
					// tell dragon the new opponent
					dragon.GetComponent<ComputerControlledDragon> ().OpponentPlayer = knightSword;
				}
			}

		} else if (character == ApplicationModel.PlayerCharacter.Dragon) {
			// playing with dragon

		} 

		// for both players
		// show or hide End-Button when hitting Escape
		if (Input.GetButtonDown ("Cancel")) {
			if (endButton.IsActive()) {
				endButton.gameObject.SetActive (false);
			} else {
				endButton.gameObject.SetActive (true);
			}
		}

		// check life bars
		if (opponentsLifeBar.fillAmount == 0 || playersLifeBar.fillAmount == 0) {
			EndGameWinOrLose (character);
		}
	}

	private void EndGameWinOrLose(ApplicationModel.PlayerCharacter character)
	{
		// Game is over, remove controls
		if (character == ApplicationModel.PlayerCharacter.Knight) {
			Destroy (knightSword.GetComponent<PlayerControlledKnightSword> ());
			Destroy (knightBow.GetComponent<PlayerControlledKnightBow> ());
			Destroy (dragon.GetComponent<ComputerControlledDragon> ());
		} else if (character == ApplicationModel.PlayerCharacter.Dragon) {
			Destroy (knightSword.GetComponent<ComputerControlledKnightSword> ());
			Destroy (dragon.GetComponent<PlayerControlledDragon> ());
		}
		winOrLoseButton.gameObject.SetActive (true);
		if (opponentsLifeBar.fillAmount == 0) {
			winOrLoseButton.GetComponentInChildren<Text> ().text = "Du hast gewonnen.\nHier klicken zum Beenden.";
		} else if (playersLifeBar.fillAmount == 0) {
			winOrLoseButton.GetComponentInChildren<Text> ().text = "Du hast verloren.\nHier klicken zum Beenden.";
		}

	}

	public void MakeDamage(GameObject character, int damageAmount)
	{
		if (character == opponent) {
			opponentsLifeBar.fillAmount -= damageAmount / 100f;
		} else {
			playersLifeBar.fillAmount -= damageAmount / 100f;
		}
		// play pain sound
		if (character == knightSword && opponent == dragon) {
			knightSword.GetComponent<PlayerControlledKnightSword> ().PlayPainAudio ();
		}
		if (character == knightBow && opponent == dragon) {
			knightSword.GetComponent<PlayerControlledKnightBow> ().PlayPainAudio ();
		}
		if (character == knightSword && opponent == knightSword) {
			knightSword.GetComponent<ComputerControlledKnightSword> ().PlayPainAudio ();
		}
	}

	public void OnDestroy ()
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
