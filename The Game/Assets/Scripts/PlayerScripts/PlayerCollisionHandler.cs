using UnityEngine;
using System;

public class PlayerCollisionHandler : MonoBehaviour{
    /*
      Class for handling all player collsion, player position checking.
      Invokes lots of events when it detects things.
    */

    private Player parentRef;

    public BoundCheck leftCheck;
    public BoundCheck rightCheck;
    public Transform footCircle;
    public float groundCheckRad = 0.01f;
    public LayerMask groundLayer;
    private bool isJumping = false;

    public delegate void Contact(GameObject target);

    public event Contact onEnemyHit;
    public event Contact onPickupHit;
    public event Contact onInteractHit;
    public event Action onLand;
    public event Action onFall;

    private void Start() {
        parentRef = Player.instance;

        parentRef.inputHandler.onJumpInit += () => isJumping = true;
        parentRef.inputHandler.onJumpPeak += () => isJumping = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject collided = other.gameObject;
        switch (collided.tag) {
            case "Pickup":
                onPickupHit(collided);
                break;
            case "TransparentInteractable":
                onInteractHit(collided);
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D c) {
        GameObject collided = c.gameObject;
        switch (collided.tag) {
            case "Enemy":
                onEnemyHit(collided);
                break;
            case "Environment":
                bool feetTouching = Physics2D.OverlapCircle(footCircle.position, groundCheckRad, groundLayer);
                if (feetTouching) {
                    onLand();
                    Debug.Log("land");
                }
                break;
        }
    }
    private void OnCollisionExit2D(Collision2D c) {
        if (c.gameObject.tag == "Environment") {
            bool feetTouching = Physics2D.OverlapCircle(footCircle.position, groundCheckRad, groundLayer);
            if (!(feetTouching || isJumping)) {
                onFall();
            }
        }
    }
}
