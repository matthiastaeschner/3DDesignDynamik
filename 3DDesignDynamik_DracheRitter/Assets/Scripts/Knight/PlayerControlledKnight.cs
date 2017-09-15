using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledKnight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Debug.isDebugBuild) {
			Debug.Log ("PlayerControlledKnight-Script attached to " + gameObject.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
