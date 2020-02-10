using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public PickupClass pickupClass = PickupClass.NONE;
    public string type = "none";

    // Update is called once per frame
    void Update() {
        
    }

    public enum PickupClass {
        Key,
        Health,
        NONE
    }
}