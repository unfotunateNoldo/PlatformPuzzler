using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {
    /*
      Parent reference for the player
      Manages and exposes all player properties
      Including health and inventory
    */

    public static Player instance = null;

    //life controls
    public int healthPoints = 100;
    public int invincibilityFrames = 100;
    private bool isAlive = true;
    private int invincibility = 0;

    public CameraBehavior cameraBehavior;
    public PlayerInputHandler inputHandler;
    public PlayerCollisionHandler collisionHandler;

    //certain exposed player events
    public event Action onDamageTaken;

    // Intitialize component references and singleton
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance!=this)
            Destroy(gameObject);
        DontDestroyOnLoad(instance);

        inputHandler = GetComponent<PlayerInputHandler>();
        collisionHandler = GetComponent<PlayerCollisionHandler>();

        collisionHandler.onEnemyHit += EnemyHandler;
        collisionHandler.onInteractHit += InteractHandler;
        collisionHandler.onPickupHit += PickupHandler;
	}

    private void Update() {
        //reduce invincibility frames
        if (invincibility > 0) {
            invincibility--;
        }
    }

    private void EnemyHandler(GameObject enemy) {
        if (invincibility <= 0) {
            healthPoints -= 20;
            invincibility = invincibilityFrames;
            onDamageTaken();
            Debug.Log("Player lost health");
        }
        if (healthPoints <= 0) {
            Debug.Log("player is dead, resetting health");
            healthPoints = 100;
            invincibility = 0;
        }
    }

    private void PickupHandler(GameObject pickup) {
        //stopgap code: inventory needs slight redesign
        if (pickup.GetComponent<Pickup>().pickupClass == Pickup.PickupClass.Key) {
            if (Inventory.instance.AddItem("Key", pickup)) {
                Destroy(pickup);
            }
        } else {
            throw new Exception("anioop the pickup class");
        }
    }

    private void InteractHandler(GameObject interactable) {
        //stopgap code
        if (interactable.GetComponent<Interactable>().interactClass == Interactable.InteractClass.Door) {
            List<Sprite> items = Inventory.instance.GetItems("Key");
            if (items != null && items.Count > 0) {
                if (interactable.GetComponent<DoorLock>().unlock())
                    Inventory.instance.RemoveItem("Key");
            }
        } else {
            throw new Exception("the spaghetti has spaghettied @interactclass");
        }
    }

}