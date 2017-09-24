using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHit : MonoBehaviour
{
	private GameManagement gameManager;

	private GameObject opponentPlayer;

	public GameObject OpponentPlayer {
		get {
			return opponentPlayer;
		}
		set {
			opponentPlayer = value;
		}
	}

	private int damageAmount = 2;

	// Use this for initialization
	void Start ()
	{
		gameManager = GameObject.Find("GameController").GetComponent<GameManagement>();
	}

	// detect a collision with the opponent player
	public void OnCollisionEnter(Collision collision){
		if(collision.gameObject == opponentPlayer) {
//			if (Debug.isDebugBuild) {
//				Debug.Log ("Arrow collision with " + collision.gameObject.name);
//			}
			gameManager.MakeDamage (opponentPlayer, damageAmount);
		}
	}
}
