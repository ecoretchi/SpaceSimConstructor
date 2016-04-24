using UnityEngine;
using System.Collections;

public class StructuralFeature : MonoBehaviour {

    public enum Type {
        Small,
        Medium,
        Large
    };

    /// <summary>
    /// Structure could be Big or small 
    /// </summary>
    public Type type { get; set; }
    /// <summary>
    /// Is the main vital station value, if hull decreasing to zero the structure completely be destroyed 
    /// </summary>
    public float hull { get; set; }
    /// <summary>
    /// Important property to normal functional the station 
    /// </summary>
    public PlantProperty maintanance { get; set; }
    /// <summary>
    /// GWats energy production and consumation by structure
    /// </summary>
    public PlantProperty energy { get; set; }
    /// <summary>
    /// Some negative factor that could decrease hull, or positive that increase it.
    /// This value is the absolute, means that the value apply once.
    /// </summary>
    public FluenceProperty hullFluence { get; set; }

    StructuralFeature() {

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
