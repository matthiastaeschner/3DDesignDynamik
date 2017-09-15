using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
	private ApplicationModel.PlayerCharacter character;

	public GameObject knightSword;
	public GameObject knightBow;
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
			knightSword.AddComponent<PlayerControlledKnight> ();
			// dragon.AddComponent<ComputerControlledDragon> ();
		} else {
			// dragon.AddComponent<PlayerControlledDragon> ();
			knightSword.AddComponent<ComputerControlledKnight> ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
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
