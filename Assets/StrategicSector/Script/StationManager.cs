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
public class StationData {

	public double lifeSupportLimit;
	public double lifeSupport;
	public double credits;
	public double freelanceTaxFactor;
	public double socialApartments;
	public double conventionalApartments;
	public double luxApartments;

	public double stuffWorkers;
	public double freelanceWorkers;
	public double luxCivil;

	public double maintenanceLevel;
	public double maintenanceFunds;
	public double maintenanceRequired;


	//civil like this station
	public double GetStationAppeal () {
		//(2-O7/E7-O7/P7)*(W7/200)
		double O7 = GetCivil();
		double E7 = GetLifeSupportFree();
		double P7 = GetApartmentsTotal();
		double W7 = maintenanceLevel;
		return (2-O7/E7-O7/P7)*(W7/200);
	}
	public int GetLifeSupportFree(){ 
		double O7 = GetCivil();
		double E7 = GetLifeSupportFree();
		return E7-O7;
	}
	public double GetTaxProfit(){ 
		//TODO: 
		return 0;}
	public int GetCivil(){ 
		return stuffWorkers + freelanceWorkers + luxCivil;
	}
	public int GetApartmentsTotal(){ 
		//TODO: 
		return 0;
	}

	public double GetMaintenanceConsume(){ 
		//TODO: 
		return 0;
	}

	public double GetMaintenanceAvail(){ 
		//TODO: 
		return 0;
	}

	public double GetMaintenancePerformance(){ 
		//TODO: 
		return 0;
	}
	public double GetMaintenanceFree(){ 
		//TODO: 
		return 0;
	}
	//---- want come ---
	public int GetWantComeStuffWorkers(){ 
		//TODO: 
		return 0;
	}
	public int GetWantComeFreelanceWorkers(){ 
		//TODO: 
		return 0;
	}
	public int GetWantComeLuxCivil(){ 
		//TODO: 
		return 0;
	}
	public int GetIncomeCivil(){ 
		return GetWantComeStuffWorkers() + GetWantComeFreelanceWorkers() + GetWantComeLuxCivil();
	}

	//---- new accepted civil---
	public int GetNewStuffWorkersAccepted(){ 
		//TODO: 
		return 0;
	}
	public int GetNewFreelanceWorkersAccepted(){ 
		//TODO: 
		return 0;
	}
	public int GetNewLuxCivilAccepted(){ 
		//TODO: 
		return 0;
	}

}
//http://answers.unity3d.com/questions/901770/invoke-method-with-reflection-c.html

public class StationCharacteristics {
	public int theHull {get;set;}
	public int hullWeakening {get;set;}
	public double energyGWatts {get;set;}
    public double maintenance { get; set; }
    /// <summary>
    /// how many civil has life supporting
    /// </summary>
    public double civilSupport{ get; set; }
    public double civilSupportMax { get; set; }
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

