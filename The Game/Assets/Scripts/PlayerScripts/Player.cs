using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player player = null;

	public float speedMod = 0.2f;

    public int jumpFrameCount = 100;
    public float jumpHeight = 1.0f;

    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public RocketBoost rocket;
    public Inventory inventory;

    public int keys = 0;

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
        inventory = GetComponent<Inventory>();
        if (inventory == null)
            throw new InvalidOperationException();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //handle rocket boost
        if (GravSwitch.changingGrav)
            return;
        if(Input.GetKeyDown(KeyCode.Space) && !isFalling){
            //StartCoroutine("Jump");
        }
        if(Input.GetKey(KeyCode.Space) && rocket.enabled) {
            rocket.boost();
        }
        transform.position += (Vector3) (GameMaster.rightDirection * Input.GetAxis("Horizontal") * speedMod * Time.fixedDeltaTime);
        if (Physics2D.Raycast(transform.position, Physics2D.gravity, bc.bounds.extents.y+float.Epsilon).collider != null)
            isFalling = false;
    }

    //Jump is unimplemented: On complete implementation, uncomment the line "StartCoroutine("Jump")" in the FixedUpdate function
    IEnumerator Jump(){
        for(int i=0; i<jumpFrameCount; i++){

        }
        yield break;
    }
}