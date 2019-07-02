using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player player = null;

	public float speedMod = 0.2f;

    public int jumpFrameCount = 100;
    public float jumpHeight = 1.0f;
    
    public int healthPoints = 100;
    public bool isInvincible = false;
    public int invincibilityFrames = 100;
    public bool isAlive = true;

    public Rigidbody2D rb;
    public BoxCollider2D bc;
    //public RocketBoost rocket;
    //public List<Equipment> equipment;

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
            case "Enemy":
                if(!isInvincible) {
                    StartCoroutine("invincible");
                    healthPoints-=20;
                    Debug.Log("Player lost health");
                }
                if(healthPoints <= 0) {
                   Debug.Log("player is dead, resetting health");
                   healthPoints = 100;
                   isInvincible = false;
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
        //rocket = GetComponent<RocketBoost>();
        //equipment = new List<Equipment>(GetComponents<Equipment>());
	}

	void FixedUpdate () {
        if (GravSwitch.changingGrav)
            return;
        if(Input.GetKeyDown(KeyCode.Space) && !isFalling){
            isFalling = true;
            StartCoroutine("Jump");
        }
        transform.position += (Vector3) (GameMaster.rightDirection * Input.GetAxis("Horizontal") * speedMod * Time.fixedDeltaTime);
        if (Physics2D.Raycast(transform.position, Physics2D.gravity, bc.bounds.extents.y+0.1f).collider != null)
            isFalling = false;
    }

    IEnumerator Jump(){
        rb.velocity = GameMaster.upDirection * jumpHeight;
        for(int i=0; i<jumpFrameCount; i++){
            if (Input.GetKey(KeyCode.Space)) {
                rb.velocity = GameMaster.upDirection * jumpHeight;
            } else {
                break;
            }
            yield return null;
        }
        yield break;
    }
    
    IEnumerator invincible(){
    isInvincible = true;
        for(int i = invincibilityFrames; i>0; i++){
            i--;
            yield return null;
        }
    isInvincible = false;
    }
}