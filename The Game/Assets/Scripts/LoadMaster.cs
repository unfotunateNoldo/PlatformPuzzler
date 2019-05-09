using UnityEngine;

public class LoadMaster : MonoBehaviour {

    public static LoadMaster lm = null;

	// Use this for initialization
	void Awake () {
        if (lm == null)
            lm = this;
        else if (lm != this)
            Destroy(gameObject);
        DontDestroyOnLoad(lm);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
