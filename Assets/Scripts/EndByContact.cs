using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndByContact : MonoBehaviour 
{
	//moved to controller
	//float timeLeft = 2.0f;

	private GameController gameController;
	PlayerController playerController;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController> ();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find GameController");
		}

		//get pointers

		playerController = gameObject.GetComponent<PlayerController>();
	}

	void OnCollisionEnter(Collision collision) {
		playerController.crash();

		//end game if collided with floor
		if (collision.gameObject.tag == "Floor") {
			gameController.ended = true;
		}
	}

	void Update () 
	{
		//moved game ending to game controller
	}
}
