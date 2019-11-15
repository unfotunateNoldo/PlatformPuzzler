using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player player = null;

	public float speedMod = 0.2f;
    public float speed = 1.0f;
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    public BoundCheck leftCheck;
    public BoundCheck rightCheck;
    private Vector2 horizontalVelocity;

    public int jumpFrameCount = 100;
    public float jumpHeight = 1.0f;
    private bool jumping = false;
    
    public int healthPoints = 100;
    public int invincibilityFrames = 100;
    private bool isAlive = true;

    private int invincibility = 0;

    public Rigidbody2D rb;
    public Animator animator;
    public Transform footCircle;
    public float groundCheckRad = 0.01f;
    public LayerMask groundLayer;

    public CameraBehavior cameraBehavior;

    public bool isFalling;

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject collided = other.gameObject;
        switch (collided.tag) {
            case "Key":
                if (Inventory.instance.AddItem(collided.tag, collided)) {
                    Destroy(collided);
                }
                break;
            case "Door":
                List<Sprite> items = Inventory.instance.GetItems("Key");
                if (items != null && items.Count > 0) {
                    if (collided.GetComponent<DoorLock>().unlock())
                        Inventory.instance.RemoveItem("Key");
                }
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D c) {
        GameObject collided = c.gameObject;
        switch (collided.tag) {
            case "Enemy":
                if (invincibility<=0) {
                    healthPoints -= 20;
                    invincibility = invincibilityFrames;
                    Debug.Log("Player lost health");
                }
                if (healthPoints <= 0) {
                    Debug.Log("player is dead, resetting health");
                    healthPoints = 100;
                    invincibility = 0;
                }
                break;
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
        animator = GetComponent<Animator>();
        Face(true);
	}

	void FixedUpdate () {
        if (GravSwitch.changingGrav)
            return;
        Move(Input.GetAxisRaw("Horizontal"));
    }

    private void Move(float input) {
        horizontalVelocity = Vector3.Project(rb.velocity, GameMaster.rightDirection);
        if(input==0 && horizontalVelocity.sqrMagnitude != 0) {
            CancelHorizontalVelocity();
        } else {
            if (horizontalVelocity.normalized == GameMaster.rightDirection) {
                if (input < 0) {
                    CancelHorizontalVelocity();
                }
            } else {
                if(input > 0) {
                    CancelHorizontalVelocity();
                }
            }
            float m = horizontalVelocity.magnitude;
            if (m < speed && MoveCheck(input)) {
                Vector2 moveForce = GameMaster.rightDirection * input * speedMod;
                if (m < 0.9 * speed) {
                    rb.AddForce(moveForce * 10);
                    if (m < 0.01) {
                        Face(input > 0);
                    }
                } else {
                    rb.AddForce(moveForce);
                }
            }
        }
    }

    private void Face(bool right) {
        GetComponent<SpriteRenderer>().flipX = right;
        GetComponentInChildren<HandPositionController>().Face(right);
    }

    private void CancelHorizontalVelocity() {
        rb.velocity = Vector3.Project(rb.velocity, GameMaster.upDirection);
        horizontalVelocity = Vector2.zero;
    }

    private bool MoveCheck(float input) {
        return input!=0 && (input>0 ? canMoveRight : canMoveLeft);
    }

    private void Update() {
        // controls are disabled if we are changing gravity
        if (GravSwitch.changingGrav) {
            return;
        }

        //reduce invincibility frames
        if (invincibility > 0) {
            invincibility--;
        }

        //set isFalling
        if(!Physics2D.OverlapCircle(footCircle.position, groundCheckRad, groundLayer)) {
            if (!isFalling) {
                isFalling = true;
                if (!jumping) {
                    animator.SetTrigger("Fall");
                }
            }
        } else {
            if (isFalling) {
                isFalling = false;
                animator.SetTrigger("JumpEnd");
            }
        }

        //set ability to move left or right
        canMoveLeft = !leftCheck.collidingWithWall;
        canMoveRight = !rightCheck.collidingWithWall;

        //check for end of jump
        if(!isFalling && animator.GetCurrentAnimatorStateInfo(0).IsName("Falling")) {
            animator.SetTrigger("JumpEnd");
        }

        //detect jump input
        if (Input.GetButtonDown("Jump") && !isFalling) {
            StartCoroutine("Jump");
        }
    }

    IEnumerator Jump(){
        jumping = true;
        animator.SetTrigger("JumpInit");
        rb.AddForce(GameMaster.upDirection * jumpHeight, ForceMode2D.Impulse);
        for(int i=0; i<jumpFrameCount; i++){
            if (Input.GetKey(KeyCode.Space)) {
                rb.AddForce(GameMaster.upDirection * jumpHeight * 2.4f);
            } else {
                break;
            }
            yield return null;
        }
        animator.SetTrigger("JumpPeak");
        jumping = false;
        yield break;
    }
}