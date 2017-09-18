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
	public enum KnightWeaponMode 
	{
		Sword,
		Bow
	}
	private KnightWeaponMode knightWeapon;
	public KnightWeaponMode KnightWeapon {
		get {
			return knightWeapon;
		}
	}
	private Vector3 knightCurrentPosition;
	private Quaternion knightCurrentRotation;

	// dragon
	public GameObject dragon;

	// Use this for initialization
	void Start ()
	{
		character = ApplicationModel.character;
		if (Debug.isDebugBuild) {
			Debug.Log ("Started game with " + character);
		}

		// TODO:
		// add controll-scripts to characers depending on loaded character
		// e.g. Dragon was choosen -> load scripts for pc-controlled Knight and player-controlled Dragon
		if (character == ApplicationModel.PlayerCharacter.Knight) {
			knightSword.AddComponent<PlayerControlledKnightSword> ();
			knightBow.AddComponent<PlayerControlledKnightBow> ();
			// dragon.AddComponent<ComputerControlledDragon> ();
		} else {
			// dragon.AddComponent<PlayerControlledDragon> ();
			knightSword.AddComponent<ComputerControlledKnightSword> ();
		}
		// always start with knight standing with sword
		knightWeapon = KnightWeaponMode.Sword;
		knightSword.GetComponent<Animator> ().Play ("Knight_Stand_Sword");
	}
	
	// Update is called once per frame
	void Update ()
	{
		// change knights weapon
		if (Input.GetKeyDown ("left shift")) {
			if (knightWeapon == KnightWeaponMode.Sword) {
				knightWeapon = KnightWeaponMode.Bow;
				knightSword.GetComponent<Animator> ().Play ("Knight_ChangeWeapon_ToBow_Part1");
				knightCurrentPosition = knightSword.transform.position;
				knightCurrentRotation = knightSword.transform.rotation;
				knightSword.SetActive (false);
				knightBow.SetActive (true);
				knightBow.transform.position = knightCurrentPosition;
				knightBow.transform.rotation = knightCurrentRotation;
				knightBow.GetComponent<Animator> ().Play ("Knight_ChangeWeapon_ToBow_Part2");
			} else {
				knightWeapon = KnightWeaponMode.Sword;
				knightBow.GetComponent<Animator> ().Play ("Knight_ChangeWeapon_ToSword_Part1");
				knightCurrentPosition = knightBow.transform.position;
				knightCurrentRotation = knightBow.transform.rotation;
				knightBow.SetActive (false);
				knightSword.SetActive (true);
				knightSword.transform.position = knightCurrentPosition;
				knightSword.transform.rotation = knightCurrentRotation;
				knightSword.GetComponent<Animator> ().Play ("Knight_ChangeWeapon_ToSword_Part2");
			}
		}
	}

	void OnDestroy ()
	{
		if (Debug.isDebugBuild) {
			Debug.Log ("Destroy");
		}
		ApplicationModel.character = ApplicationModel.PlayerCharacter.None;
	}

	public void SwitchToStartScene ()
	{
		if (Debug.isDebugBuild) {
			Debug.Log ("SwitchToStartScene");
		}
		SceneManager.LoadScene ("Start");
	}
}
