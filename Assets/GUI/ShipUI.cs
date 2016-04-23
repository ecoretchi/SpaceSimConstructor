using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Temporary realisation of Ship UI
/// </summary>
public class ShipUI : MonoBehaviour {

    public Text speedTxt;
    public Slider speedSlider;
    public Text descField;

    public float speed {
        get {
            return speedSlider.value;
        }
        set {
            speedSlider.value = value;
            //speedTxt.text = System.Math.Round( speed, 1 ).ToString();
        }
    }

    public string description {
        get {
            return descField.text;
        }
        set {
            descField.text = value;
        }
    }

    public void setSpeedLabel( float speed ) {
        speedTxt.text = System.Math.Round( speed, 2 ).ToString("0.00");

        if ((speed > -0.1f) && (speed < 0.1f) && (speed != 0)){
            speed = 0;
            speedSlider.value = 0;
        }
    }
}
