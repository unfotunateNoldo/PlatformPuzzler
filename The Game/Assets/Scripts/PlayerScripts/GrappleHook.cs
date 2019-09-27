using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : Equipment {
    private float cTime = 0;
    public float minTimeToPreciseGrapple = 1.0f;

    private void Update() {
        if (Input.GetKey(KeyCode.C)) {
            cTime += Time.deltaTime;
            if(cTime >= minTimeToPreciseGrapple) {
                Debug.Log("Put precise grapple code here");
            }
        }
        if (Input.GetKeyUp(KeyCode.C)) {
            if (cTime < minTimeToPreciseGrapple) {
                Debug.Log("Put Quick Grapple code here");
            }
            cTime = 0;
        }
    }
}
