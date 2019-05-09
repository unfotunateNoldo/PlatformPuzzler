using UnityEngine;

public class GameMaster : MonoBehaviour {
    private static Vector2 UpDirection;
    public static Vector2 upDirection {
        get {
            return UpDirection;
        }
        set {
            UpDirection = value.normalized;
        }
    }
    public static Vector2 rightDirection {
        get {
            return upDirection.Rotate(-90.0f);
        }
    }
    public static Vector2 leftDirection {
        get {
            return upDirection.Rotate(90.0f);
        }
    }
    public static Vector2 downDirection {
        get {
            return (-1) * upDirection;
        }
    }
    public static GameMaster gm = null;
    public static readonly Vector2 normalGrav = new Vector2(0, -9.81f);

    // Use this for initialization
	void Awake () {
        if (gm == null)
            gm = this;
        else if (gm != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gm);
        upDirection = (-1)*Physics2D.gravity;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Y)) {
            Debug.Log("up: " + upDirection);
            Debug.Log("down: " + downDirection);
            Debug.Log("right: " + rightDirection);
            Debug.Log("left: " + leftDirection);
        }
    }
}
