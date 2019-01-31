using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jumping : MonoBehaviour {

	public Text fuelIndicator;
	public Rigidbody2D rb;
	public BoxCollider2D bc;
    public int jumpHeight = 10;
	public float fuel = 100.0f;
	public float decreaseMod = 1.0f;
	public float increaseMod = 1.0f;
	public float speedMod = 0.2f;

	private bool isFalling;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		bc = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.Space) && fuel > 0) {
			rb.AddForce (Vector2.up * jumpHeight);
			isFalling = true;
			fuel -= decreaseMod;
			fuelIndicator.text = "Fuel: " + fuel;
		}
		if (fuel < 100.0f && !isFalling) {
			fuel += increaseMod;
			fuelIndicator.text = "Fuel: " + fuel;
		}
        transform.position += (Vector3.right * Input.GetAxis("Horizontal") * speedMod);
        if (Physics2D.Raycast(transform.position, Vector2.down, bc.bounds.extents.y+0.1f).collider != null)
            isFalling = false;
    }
}