﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravSwitch : MonoBehaviour {
    public static bool changingGrav = false;
    public static float changeTime = 0.7f;

    private static float invChangeTime;
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
        Rigidbody2D[] freeze = FindObjectsOfType<Rigidbody2D>();
        RigidbodyConstraints2D[] freezeValues = new RigidbodyConstraints2D[freeze.Length];
        for (int i = 0; i < freeze.Length; i++) {
            Rigidbody2D r = freeze[i];
            freezeValues[i] = r.constraints;
            if (r.constraints!=RigidbodyConstraints2D.FreezePosition) {
                r.constraints = RigidbodyConstraints2D.FreezePosition;
            }
        }

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

        for(int i=0; i<freeze.Length; i++) {
            freeze[i].constraints = freezeValues[i];
        }

        Physics2D.gravity = currentDest;
        changingGrav = false;
        currentDest = currentDest != GameMaster.normalGrav ? GameMaster.normalGrav : dest;
        GameMaster.upDirection = (-1)*Physics2D.gravity;
        yield break;
    }

    public bool getNormalGrav(){
        return dest == GameMaster.normalGrav;
    }
}