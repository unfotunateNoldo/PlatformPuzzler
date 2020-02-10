using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public Transform target;
    public float zOffset = 10.0f;
    public float yOffset = 0;
    public float xOffset = 0;
    private bool doFollow = true;

    void Start(){
        target = Player.instance.transform;
    }

    // Update is called once per frame
    void Update(){
        if (doFollow) {
            transform.position = new Vector3(target.position.x - xOffset, target.position.y - yOffset, target.position.z - zOffset);
        }
    }

    public void Attach() {
        doFollow = true;
    }

    public void Detach() {
        doFollow = false;
    }

    public void bump(float amount) {
        yOffset = amount;
    }
}
