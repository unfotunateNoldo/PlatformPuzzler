using UnityEngine;
using UnityEngine.UI;

public class RocketBoost : Equipment {

    public float rocketBoost = 10.0f;
    public float fuel = 100.0f;
    public float decreaseMod = 1.0f;
    public float increaseMod = 1.0f;

    public Text fuelIndicatorPrefab;
    private Text fuelIndicator;

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
            fuel -= decreaseMod;
            fuelIndicator.text = "Fuel: " + fuel;
        }
    }

    public void OnEnable() {
        fuelIndicator = Instantiate(fuelIndicatorPrefab, GameObject.Find("Canvas").transform);
    }

    public void OnDisable() {
        Destroy(fuelIndicator);
    }
}
