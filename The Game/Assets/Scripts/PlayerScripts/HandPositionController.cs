using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPositionController : MonoBehaviour{
    public Vector2 rightPosition;
    public Vector2 leftPosition;

    public void Face(bool right) {
        transform.localPosition = right ? rightPosition : leftPosition;
        GetComponent<SpriteRenderer>().flipX = right;
    }
}
