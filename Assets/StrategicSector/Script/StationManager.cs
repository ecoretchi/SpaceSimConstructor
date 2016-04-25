using UnityEngine;
using System.Collections;

public class StationManager : MonoBehaviour {

	//station characteristics
	StationCharacteristics m_stationInfo;
    MainStationModule m_cms;
    // Use this for initialization
    void Start () {
        m_cms = FindObjectOfType<MainStationModule>();

        // begin from build scrach, but we must save the station state in future

        m_cms.Release();

    }

    // Update is called once per frame
    void Update () {
	
	}
}

//http://answers.unity3d.com/questions/901770/invoke-method-with-reflection-c.html

public class StationCharacteristics {
	public int theHull {get;set;}
	public int hullWeakening {get;set;}
	public double energyGWatts {get;set;}
	public double maintenance {get;set;}
	public double civilLoyalty {get;set;}

	public int transportRoutesMax {get;set;}
	public int transportRoutes {get;set;}

	public double pureApartment {get;set;}
	public double normalApartment {get;set;}
	public double luxApartment {get;set;}

	// ========== demands ========== 
	public double pureApartmentDemands {get;set;}
	public double normalApartmentDemands {get;set;}
	public double luxApartmentDemands {get;set;}

}

