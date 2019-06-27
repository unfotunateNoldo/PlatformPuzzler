using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyBehavior : MonoBehaviour{

    private bool isFalling = false;
    private BoxCollider2D bc;

    public float moveSpeed = 10;
    public int moveDist = 10;
    private float distTraveled = 0;

    void Awake(){
        bc = GetComponent<BoxCollider2D>();
    }

    //This code just moves the object back and forth by moveDist at speed moveSpeed*deltaTime/frame
    void Update() {
        if (!isFalling) {
            float distToMove = Time.deltaTime * moveSpeed;
            if (distTraveled + Mathf.Abs(distToMove) > moveDist) {
                distToMove = moveDist - distTraveled;
                moveSpeed = -moveSpeed;
                distTraveled = 0;
            } else {
                distTraveled += Mathf.Abs(distToMove);
            }
            transform.position += (Vector3) GameMaster.rightDirection * distToMove;
        }
    }

    // Update is called once per frame
    void FixedUpdate(){
        if (Physics2D.Raycast(transform.position, Physics2D.gravity, bc.bounds.extents.y + 0.1f).collider != null)
            isFalling = false;
        else
            isFalling = true;
    }
}
