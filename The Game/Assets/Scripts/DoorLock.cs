using UnityEngine;

public class DoorLock : MonoBehaviour {

    private bool locked = true;

    public void unlock(){
        locked = false;
        GetComponent<Animator>().SetBool("Unlocked", true);
    }

    public void OnTriggerEnter2D(Collider2D c){
        if (c.gameObject.tag == "Player" && !locked)
            GetComponent<Animator>().SetTrigger("openTrigger");
    }
    public void OnTriggerExit2D(Collider2D collision){
        if (collision.gameObject.tag == "Player") {
            GetComponent<Animator>().SetTrigger("closeTrigger");
        }
    }
}
