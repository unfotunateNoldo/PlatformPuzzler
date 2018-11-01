using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jumping : MonoBehaviour {

	public Text fuelIndicator;
	public Rigidbody2D rb;
	public int jumpHeight = 10;
	public float fuel = 100.0f;
	public float decreaseMod = 1.0f;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.Space) && fuel>0) {
			rb.AddForce(Vector2.up * jumpHeight);
			fuel -= decreaseMod;
			fuelIndicator.text = "Fuel: " + fuel;
		}
		if (Physics2D.Raycast (transform.position, Vector2.down).distance <2 && fuel<100) {
			fuel += decreaseMod;
			fuelIndicator.text = "Fuel: " + fuel;
		}
	}
}
