using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

	//how the player moves laterally
    public float acceleration;
    public float maxspeed;

	//pointer for rotating car model
	[SerializeField]
	private GameObject model;

	private GameController gameController;
    private Rigidbody rb;

	//whether the player has collided with anything yet
	public bool crashed;

	//pointers for starting particle effects
	public ParticleSystem smokesystem;
	public ParticleSystem firesystem;

	void Start ()
    {
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null) {
			Debug.Log("Cannot find GameController");
		}
		rb = GetComponent<Rigidbody>();
    }

	private void Update() {
		if(!crashed){

			const float radianstodegrees = 180f / (float)Math.PI;

			Vector3 modelrot = new Vector3((float)Math.Atan(rb.velocity.y / GameController.currentSimulatedSpeed) * radianstodegrees, (float)Math.Atan(rb.velocity.x / GameController.currentSimulatedSpeed) * radianstodegrees, 0.0f);
			model.transform.localEulerAngles = modelrot;
		}
	}

	//when the player crashes
	public void crash() {
		if (!crashed) {
			smokesystem.Play();
			crashed = true;
			rb.drag = 0;
			rb.useGravity = true;
			rb.AddForce(new Vector3(0.0f, 250.0f, 0.0f));
			//doesn't end until colliding with floor; see EndByContact
		}
	}

	public void ignite() {
		firesystem.Play();
	}

	void FixedUpdate () {
		if(!crashed){
			float moveHorizontal = Input.GetAxis("Horizontal");
			float moveVertical = Input.GetAxis("Vertical");

			Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);

			rb.AddForce(movement * acceleration);

			rb.velocity = clampVector(rb.velocity, 0.0f, maxspeed);
		}
	}

    public static Vector3 clampVector(Vector3 vec, float min, float max)
    {
        if (vec.magnitude < min && vec.magnitude != 0.0f) return (vec * min/vec.magnitude);
        else if (vec.magnitude > max) return (vec * max / vec.magnitude);
        else return vec * 1;
    }
}
