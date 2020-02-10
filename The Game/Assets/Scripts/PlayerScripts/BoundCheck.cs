using UnityEngine;
using System;

public class BoundCheck : MonoBehaviour {
    public event Action collided;
    public event Action exited;

    private void OnTriggerEnter2D(Collider2D other) {
        bool that = other.tag == "Environment";
        if(that) {
            collided();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        bool that = other.tag == "Environment";
        if (that) {
            exited();
        }
    }
}
