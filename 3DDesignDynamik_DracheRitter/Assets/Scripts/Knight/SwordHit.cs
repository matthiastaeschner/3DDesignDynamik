using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
	private GameObject knight;

	public GameObject Knight {
		get {
			return knight;
		}
		set {
			knight = value;
		}
	}

	Animator knightAnim;

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
		knightAnim = knight.GetComponent<Animator> ();
	}

	// detect a collision with the opponent player while sword is swinged by knight
	public void OnCollisionEnter(Collision collision){
		if(collision.gameObject == opponentPlayer && knightAnim.GetCurrentAnimatorStateInfo (0).IsName ("Knight_Hit_Sword")) {
			if (Debug.isDebugBuild) {
				Debug.Log ("Sword collision with " + collision.gameObject.name);
			}
		}
	}
}
