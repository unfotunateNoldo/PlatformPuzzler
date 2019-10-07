using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : Equipment {
    private float cTime = 0;
    public float minTimeToPreciseGrapple = 1.0f;

    public KeyCode fire;
    public KeyCode up;
    public KeyCode down;
    public int rotspd;
    public GameObject target;
    public Transform pathPrefab;
    public float aimPathUp;

    private int rot = 0;
    private Transform[] path = new Transform[10];

    private void Start()
    {
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = Instantiate(pathPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    private void Update() {
        if (Input.GetKey(fire)) {
            cTime += Time.deltaTime;
            if(cTime >= minTimeToPreciseGrapple) {
                if (Input.GetKey(up))
                {
                    rot -= rotspd;
                }

                if (Input.GetKey(down))
                {
                    rot += rotspd;
                }

                transform.parent.transform.rotation = Quaternion.Euler(0, 0, rot);

                int layerMask = 12;
                layerMask = ~layerMask;

                Quaternion dir = Quaternion.Euler(0, 0, rot);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right);

                for (int i = 0; i < path.Length; i++)
                {
                    float upMult = Mathf.Abs(i - (path.Length / 2)) - 3;

                    float mathystuff = ((hit.distance - ((hit.distance / 10) * i)) / Mathf.Sqrt((hit.point.x - transform.position.x) * (hit.point.x - transform.position.x) + (hit.point.y - transform.position.y) * (hit.point.y - transform.position.y)));
                    path[i].transform.position = new Vector3(transform.position.x + mathystuff * (hit.point.x - transform.position.x), transform.position.y + mathystuff * (hit.point.y - transform.position.y) - aimPathUp * upMult, 0);
                }

                if (hit.collider != null)
                {
                    target.transform.position = hit.point;
                }
            }
        }
        if (Input.GetKeyUp(fire)) {
            if (cTime < minTimeToPreciseGrapple) {
                Debug.Log("Put Quick Grapple code here");
            }
            cTime = 0;
        }
    }
}
