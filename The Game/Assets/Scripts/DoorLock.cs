using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour {

    bool locked = true;

    public void unlock()
    {
        locked = false;
    }

    public void OnTriggerEnter2D(Collider2D c){
        if (c.gameObject.tag == "Player" && !locked)
            GetComponent<Animator>().SetTrigger("openTrigger");
    }
    public void OnTriggerExit2D(Collider2D collision){
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DoorOpen"))
            GetComponent<Animator>().SetTrigger("closeTrigger");
    }
}
