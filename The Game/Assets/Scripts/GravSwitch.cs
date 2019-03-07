using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravSwitch : MonoBehaviour {
    Rigidbody2D rb;
    static bool gravNorm;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        gravNorm = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (gravNorm)
            {
                rb.gravityScale = (float)-1.0;
                gravNorm = false;
            }
            else {
                rb.gravityScale = (float)1.0;
                gravNorm = true;

            }
        }

    }
    public static bool getGrav()
    {
        return gravNorm;
    }
}
