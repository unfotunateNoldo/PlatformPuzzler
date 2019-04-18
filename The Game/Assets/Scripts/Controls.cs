using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour {

	public Text fuelIndicator;
	public Rigidbody2D rb;
	public BoxCollider2D bc;
    public float jumpHeight = 10.0f;
	public float fuel = 100.0f;
	public float decreaseMod = 1.0f;
	public float increaseMod = 1.0f;
	public float speedMod = 0.2f;

	private bool isFalling;

    private int keys = 0;

    private void OnTriggerEnter2D(Collider2D c){
        GameObject collided = c.gameObject;
        switch (collided.tag){
            case "Key":
                keys++;
                Destroy(collided);
                break;
            case "Door":
                if (keys > 0){
                    keys--;
                    collided.GetComponent<DoorLock>().unlock();
                }
                break;
            default:
                throw new System.Exception("Forgot to tag the goddamn collision");
        }
    }

    // Intitialize component references
    void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		bc = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //handle rocket boost
        if (GravSwitch.changingGrav)
            return;
        if (Input.GetKey (KeyCode.Space) && fuel > 0) {
            rb.AddForce(new Vector2(-Physics.gravity.x, -Physics.gravity.y) * jumpHeight * Time.deltaTime);

			isFalling = true;
			fuel -= decreaseMod;
			fuelIndicator.text = "Fuel: " + fuel;
		}
		if (fuel < 100.0f && !isFalling) {
			fuel = Mathf.Clamp(fuel + increaseMod, 0, 100);
			fuelIndicator.text = "Fuel: " + fuel;
		}
        transform.position += (Vector3.right * Input.GetAxis("Horizontal") * speedMod);
        if (Physics2D.Raycast(transform.position, Vector2.down, bc.bounds.extents.y+0.1f).collider != null)
            isFalling = false;
    }
}