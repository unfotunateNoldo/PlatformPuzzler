using UnityEngine;

public class DoorLock : MonoBehaviour {

    private bool locked = true;

    public bool unlock(){
        if (!locked) return false;
        locked = false;
        GetComponent<Animator>().SetBool("Unlocked", true);
        return true;
    }

    public void OnTriggerEnter2D(Collider2D c){
        if (c.gameObject.tag == "Player" && !locked)
            GetComponent<Animator>().SetTrigger("openTrigger");
    }
    public void OnTriggerExit2D(Collider2D c){
        if (c.gameObject.tag == "Player") {
            GetComponent<Animator>().SetTrigger("closeTrigger");
        }
    }
}
