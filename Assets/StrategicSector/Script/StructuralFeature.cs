using UnityEngine;
using System.Collections;

public class StructuralFeature : MonoBehaviour {

    public enum Type {
        Small,
        Medium,
        Large
    };

    /// <summary>
    /// Structure could be Big or small, this value could using by inform the user how structure big are
    /// </summary>
    public Type type;
    /// <summary>
    /// All cycle properties using this value to update corresponded property
    /// </summary>
    public float minutesPerCycle;
    /// <summary>
    /// Is the main vital station value, if hull decreasing to zero the structure completely be destroyed 
    /// </summary>
    public float hull;
    /// <summary>
    /// Add improves the station structural or it declines
    /// </summary>
    public InfluenceProperty[] influences;
    /// <summary>
    /// Add specific consumation or production results here
    /// </summary>
    public Facility[] facilities;
    /// <summary>
    /// apartments 
    /// </summary>
    public Apartments[] apartments;
    /// <summary>
    /// Add improves the storage and it capacity or it declines
    /// </summary>
    public StorageProperty[] storage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
