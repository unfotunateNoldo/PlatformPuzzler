using System.Collections;
using UnityEngine;
using System;

public class PlayerInputHandler : MonoBehaviour{
    /*
      Class for handling all player input
      Physically moves player, calls lots
        of event states through the parent player class
      Queries collision handler for positional info
    */

    private Player parentRef;

    //speed controls
    public float acceleration = 0.2f;
    public float speed = 1.0f;
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    private Vector2 horizontalVelocity;
    public Rigidbody2D rb;

    //jump controls
    public int jumpFrameCount = 100;
    public float jumpHeight = 1.0f;
    private bool isFalling = false;
    private bool isWalking = false;

    public delegate void FacingHandler(bool isRight);
    public event Action onWalkStart;
    public event Action onWalkEnd;
    public event FacingHandler onFacing;
    public event Action onAttack;
    public event Action onJumpInit;
    public event Action onJumpPeak;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        parentRef = Player.instance;

        parentRef.collisionHandler.leftCheck.collided += () => canMoveLeft = false;
        parentRef.collisionHandler.leftCheck.exited += () => canMoveLeft = true;
        parentRef.collisionHandler.rightCheck.collided += () => canMoveRight = false;
        parentRef.collisionHandler.rightCheck.exited += () => canMoveRight = true;

        parentRef.collisionHandler.onFall += () => isFalling = true;
        parentRef.collisionHandler.onLand += () => isFalling = false;
    }

    private void Update() {
        // controls are disabled if we are changing gravity
        if (GravSwitch.changingGrav) {
            return;
        }

        //detect jump input
        if (Input.GetButtonDown("Jump") && !isFalling) {
            StartCoroutine("Jump");
        }

        //detect attack input
        if (Input.GetButtonDown("Attack")) {
            onAttack();
        }
    }

    void FixedUpdate() {
        if (GravSwitch.changingGrav)
            return;
        Move(Input.GetAxisRaw("Horizontal"));
    }

    private void Move(float input) {
        horizontalVelocity = Vector3.Project(rb.velocity, GameMaster.rightDirection);
        if (input == 0 && isWalking) {
            isWalking = false;
            onWalkEnd();
            if(horizontalVelocity.sqrMagnitude != 0) {
                CancelHorizontalVelocity();
            }
        } else if (input != 0) {
            if (!isWalking) {
                isWalking = true;
                onWalkStart();
            }
            if (horizontalVelocity.normalized == GameMaster.rightDirection) {
                if (input < 0) {
                    CancelHorizontalVelocity();
                }
            } else {
                if (input > 0) {
                    CancelHorizontalVelocity();
                }
            }
            float m = horizontalVelocity.magnitude;
            if (m < speed && MoveCheck(input)) {
                Vector2 moveForce = GameMaster.rightDirection * input * acceleration;
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
        //face the sprite
        GetComponent<SpriteRenderer>().flipX = right;
        //invoke the facing direction event
        onFacing(right);
    }

    private void CancelHorizontalVelocity() {
        rb.velocity = Vector3.Project(rb.velocity, GameMaster.upDirection);
        horizontalVelocity = Vector2.zero;
    }

    private bool MoveCheck(float input) {
        return input != 0 && (input > 0 ? canMoveRight : canMoveLeft);
    }

    IEnumerator Jump() {
        onJumpInit();
        rb.AddForce(GameMaster.upDirection * jumpHeight, ForceMode2D.Impulse);
        yield return null;
        for (int i = 1; i < jumpFrameCount; i++) {
            if (Input.GetKey(KeyCode.Space)) {
                rb.AddForce(GameMaster.upDirection * jumpHeight * 2 / Mathf.Sqrt(i));
            } else {
                break;
            }
            yield return null;
        }
        onJumpPeak();
        yield break;
    }

}
