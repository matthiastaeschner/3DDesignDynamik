﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControlledKnightSword : MonoBehaviour {

	private Animator anim;

	private GameObject opponentPlayer;

	public GameObject OpponentPlayer {
		get {
			return opponentPlayer;
		}
		set {
			opponentPlayer = value;
		}
	}

	// Use this for initialization
	void Start () {
		if (Debug.isDebugBuild) {
			Debug.Log ("ComputerControlledKnight-Script attached to " + gameObject.name);
		}
		anim = gameObject.GetComponent<Animator> ();
		anim.Play ("Knight_Stand_Sword");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
