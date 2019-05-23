using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player player = null;

	public float speedMod = 0.2f;

    public int jumpFrameCount = 100;
    public float jumpHeight = 1.0f;
    private bool jumping = false;

    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public RocketBoost rocket;
    public List<Equipment> equipment;

    private int keys = 0;

    public bool isFalling;

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
                throw new Exception("Forgot to tag the goddamn collision");
        }
    }

    // Intitialize component references and singleton
    void Awake () {
        if (player == null)
            player = this;
        else if (player!=this)
            Destroy(gameObject);
        DontDestroyOnLoad(player);
        rb = GetComponent<Rigidbody2D> ();
		bc = GetComponent<BoxCollider2D> ();
        rocket = GetComponent<RocketBoost>();
        equipment = new List<Equipment>(GetComponents<Equipment>());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //handle rocket boost
        if (GravSwitch.changingGrav)
            return;
        if(Input.GetKeyDown(KeyCode.Space) && !isFalling){
            isFalling = true;
            StartCoroutine("Jump");
        }
        if(Input.GetKey(KeyCode.Space) &&!jumping && rocket.enabled && isFalling) {
            Debug.Log("ascendere");
            rocket.boost();
        }
        transform.position += (Vector3) (GameMaster.rightDirection * Input.GetAxis("Horizontal") * speedMod * Time.fixedDeltaTime);
        if (Physics2D.Raycast(transform.position, Physics2D.gravity, bc.bounds.extents.y+0.1f).collider != null)
            isFalling = false;
    }

    IEnumerator Jump(){
        jumping = true;
        rb.velocity = GameMaster.upDirection * jumpHeight;
        for(int i=0; i<jumpFrameCount; i++){
            if (Input.GetKeyUp(KeyCode.Space)) {
                Debug.Log("done jumping");
                break;
            } else {
                rb.velocity = GameMaster.upDirection * jumpHeight;
            }
            yield return null;
        }
        jumping = false;
        yield break;
    }
}