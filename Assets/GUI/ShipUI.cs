using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour {

    public Text speedTxt;
    public Slider speedSlider;

    public float speed {
        get {
            return speedSlider.value;
        }
        set {
            speedSlider.value = value;
            //speedTxt.text = System.Math.Round( speed, 1 ).ToString();
        }
    }

    public void setSpeedLabel( float speed ) {
        speedTxt.text = System.Math.Round( speed, 1 ).ToString();

        if ((speed > -0.1f) && (speed < 0.1f) && (speed != 0)){
            speed = 0;
            speedSlider.value = 0;
        }
    }
}
