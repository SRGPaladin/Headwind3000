using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndByContact : MonoBehaviour 
{
	float timeLeft = 1.0f;
	bool end = false;
	private GameController gameController;

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
	
	}

	void OnCollisionEnter(Collision collision) 
	{
		end = true;
	}

	void Update () 
	{
		if (end)
		{
		    timeLeft -= Time.deltaTime;
			if (timeLeft < 0) 
			{
				//Time.timeScale = 0;
				gameController.GameOver ();
			}
		}
	}
}
