using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHit : MonoBehaviour {

	private GameManagement gameManager;

	private GameObject dragon;

	public GameObject Dragon {
		get {
			return dragon;
		}
		set {
			dragon = value;
		}
	}

	Animator dragonAnim;

	private GameObject opponentPlayer;

	public GameObject OpponentPlayer {
		get {
			return opponentPlayer;
		}
		set {
			opponentPlayer = value;
		}
	}

	Animator opponentAnim;

	private int damageAmount = 10;

	private ParticleSystem partSys;
	private List<ParticleCollisionEvent> collisionEvents;

	// Use this for initialization
	void Start ()
	{
		gameManager = GameObject.Find ("GameController").GetComponent<GameManagement> ();
		dragonAnim = dragon.GetComponent<Animator> ();
		opponentAnim = opponentPlayer.GetComponent<Animator> ();
		partSys = gameObject.GetComponent<ParticleSystem> ();
		collisionEvents = new List<ParticleCollisionEvent> ();
	}

	// detect a fire-collision with the opponent player while opponent player is not in cover
	public void OnParticleCollision (GameObject other)
	{
		int numCollEvents = partSys.GetCollisionEvents (other, collisionEvents);
		if (other == opponentPlayer && !opponentAnim.GetCurrentAnimatorStateInfo (0).IsName ("Knight_ShieldCover_In")) {
			if (Debug.isDebugBuild) {
				Debug.Log ("Fire collision with " + other.name);
			}
			gameManager.MakeDamage (opponentPlayer, damageAmount);
		}
	}
}
