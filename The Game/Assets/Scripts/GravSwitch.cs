using System.Collections;
using UnityEngine;

public class GravSwitch : MonoBehaviour {
    public static bool changingGrav = false;
    public static float changeTime = 0.7f;

    private static float invChangeTime;
    public static readonly Vector2 normalGrav = new Vector2(0, -9.81f);
    public Vector2 dest;
    private Vector2 currentDest;

    public Camera refCamera;
    public GameObject player;

    void Start (){
        currentDest = dest;
        refCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        invChangeTime = 1 / changeTime;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G) && !changingGrav){
            changingGrav = true;
            StartCoroutine("changeGrav", 0.0f);
        }
    }

    IEnumerator changeGrav() {
        float rotation = Vector2.Angle(currentDest, Physics2D.gravity);
        float remainingRotation = rotation;
        while (remainingRotation > float.Epsilon){
            float newRotation = rotation * invChangeTime * Time.deltaTime;
            player.transform.Rotate(Vector3.forward, newRotation);
            refCamera.transform.Rotate(Vector3.forward, newRotation);
            remainingRotation -= newRotation;
            yield return null;
        }
        player.transform.Rotate(Vector3.forward, remainingRotation);
        refCamera.transform.Rotate(Vector3.forward, remainingRotation);
        Physics2D.gravity = currentDest;
        changingGrav = false;
        currentDest = currentDest != normalGrav ? normalGrav : dest;
        yield return null;
    }

    public bool getNormalGrav(){
        return dest == normalGrav;
    }
}