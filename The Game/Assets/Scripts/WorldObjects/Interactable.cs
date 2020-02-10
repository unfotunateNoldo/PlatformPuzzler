using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractClass interactClass = InteractClass.NONE;
    public string type = "none";

    // Update is called once per frame
    void Update() {

    }

    public enum InteractClass {
        Door,
        GravSwitch,
        NONE
    }
}
