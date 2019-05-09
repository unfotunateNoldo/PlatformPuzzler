using UnityEngine;
using UnityEngine.UI;

public class RocketBoost : MonoBehaviour {

    public float rocketBoost = 10.0f;
    public float fuel = 100.0f;
    public float decreaseMod = 1.0f;
    public float increaseMod = 1.0f;

    public Text fuelIndicatorPrefab;
    private Text fuelIndicator;

    // Use this for initialization
    void Start () {
        fuelIndicator = Instantiate(fuelIndicatorPrefab);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (fuel < 100.0f && !Player.player.isFalling){
            fuel = Mathf.Clamp(fuel + increaseMod, 0, 100);
            fuelIndicator.text = "Fuel: " + fuel;
        }
    }

    public void boost(){
        if (fuel > 0){
            Player.player.isFalling = true;
            Player.player.rb.AddForce(GameMaster.upDirection * rocketBoost * Time.fixedDeltaTime * 100);
            Debug.Log(GameMaster.upDirection * rocketBoost * Time.fixedDeltaTime * 10);
            fuel -= decreaseMod;
            fuelIndicator.text = "Fuel: " + fuel;
        }
    }
}
