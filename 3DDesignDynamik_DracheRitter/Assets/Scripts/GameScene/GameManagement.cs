using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour {

	private ApplicationModel.PlayerCharacter character;

	// Use this for initialization
	void Start () {
		character = ApplicationModel.character;
		if (Debug.isDebugBuild) {
			Debug.Log ("Started game with " + character);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy () {
		if (Debug.isDebugBuild) {
			Debug.Log ("Destroy");
		}
		character = ApplicationModel.PlayerCharacter.None;
	}
}
