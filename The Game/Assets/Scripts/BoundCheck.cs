using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundCheck : MonoBehaviour {
    public bool collidingWithWall = false;

    private void OnTriggerEnter2D(Collider2D other) {
        bool that = other.tag == "Environment";
        if(that) {
            collidingWithWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        bool that = other.tag == "Environment";
        if (that) {
            collidingWithWall = false;
        }
    }

    private void Update() {
        // triggered = false;
    }
}
