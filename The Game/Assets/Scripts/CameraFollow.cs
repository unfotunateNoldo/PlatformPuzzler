using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float zOffset = 10.0f;

    void Start(){
        target = Player.player.transform;
    }

    // Update is called once per frame
    void Update(){
        transform.position = new Vector3(target.position.x, target.position.y, target.position.z - zOffset);
    }
}
