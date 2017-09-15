using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControlledKnight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Debug.isDebugBuild) {
			Debug.Log ("ComputerControlledKnight-Script attached to " + gameObject.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
