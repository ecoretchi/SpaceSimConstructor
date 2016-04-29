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
	public double freelanceTaxCreditRate;//credit tax rate

	public double socialApartments;
	public double conventionalApartments;
	public double luxApartments;

	public double stuffWorkers; // citizen for maintenance
	public double freelanceWorkers; // citizen for freelance
	public double luxCivil; // VIP citizen for high busness

	public double maintenanceLevel; // current station maintenance level
	public double maintenanceFunds; // current fund by user 
	public double maintenanceRequired; // station need maintenance at now

	protected double stuffWorkersFactorCome = 1;
	protected double freelanceWorkersFactorCome = 0.75f;
	protected double luxCivilFactorCome = 0.25f;

	public int wantComeStuffWorkers;
	public int wantComeFreelanceWorkers;
	public int wantComeLuxCivil;

	public int newAcceptedStuffWorkers;
	public int newAcceptedFreelanceWorkers;
	public int newAcceptedLuxCivil;


	//civil like this station
	public double CalcStationAppeal (int minLevel = 50) {
		double O7 = CalcCivil();
		double E7 = CalcLifeSupportFree();
		double P7 = CalcApartmentsTotal();
		double W7 = maintenanceLevel-minLevel;
		return (4-O7/E7-O7/P7)*W7/400;
	}
	public int CalcLifeSupportFree(){ 
		double O7 = CalcCivil();
		double E7 = CalcLifeSupportFree();
		return E7-O7;
	}
	public double CalcTaxProfit(){ 
		return freelanceTaxFactor * freelanceTaxCreditRate;
	}
	public int CalcCivil(){ 
		return stuffWorkers + freelanceWorkers + luxCivil;
	}
	public int CalcApartmentsTotal(){ 
	 return socialApartments + 
	 	conventionalApartments +
	 	luxApartments;
	}
	public double CalcMaintenanceConsume(){ 
		double AB = maintenanceFunds;
		double AC = CalcMaintenanceAvail();
		double AD = maintenanceRequired;
		double res = AB*((AC-AD)/5+AD);
		return res>0?res:0;
	}
	public double CalcMaintenanceAvail(){ 
		double W = stuffWorkers;
		return (int)(W/10);
	}
	public double CalcMaintenancePerformance(){ 
		double AB = maintenanceFunds;
		double AC = CalcMaintenanceAvail();
		double AD = maintenanceRequired;
		return AC/AD*AB/1000.0f-1.05f;
	}
	public double CalcMaintenanceFree(){ 
		double AC = CalcMaintenanceAvail();
		double AD = maintenanceRequired;
		return AC-AD;
	}
	//---- want come ---
	public int CalcWantComeStuffWorkers(){ 
		double C7 = CalcStationAppeal(50);
		double freeRooms = socialApartments - stuffWorkers;
		wantComeStuffWorkers = stuffWorkersFactorCome * C7 * freeRooms;
		return wantComeStuffWorkers;
	 	return ;
	}
	public int CalcWantComeFreelanceWorkers(){ 
		double C7 = CalcStationAppeal(60);
		double freeRooms = conventionalApartments - freelanceWorkers;
		wantComeFreelanceWorkers =  freelanceWorkersFactorCome * C7 * freeRooms;	 
		return wantComeFreelanceWorkers;
	}
	public int CalcWantComeLuxCivil(){ 
		double C7 = CalcStationAppeal(70);
		double freeRooms = luxApartments - luxCivil;
		wantComeLuxCivil = luxCivilFactorCome * C7 * freeRooms;
	 	return wantComeLuxCivil;
	}
	public int CalcWantComeCivil(){ 
		return CalcWantComeStuffWorkers() + CalcWantComeFreelanceWorkers() + CalcWantComeLuxCivil();
	}
	protected void CalcNewAcceptedCivil(){ 
		int H = CalcLifeSupportFree();
		double AH = CalcWantComeStuffWorkers();
		double AJ = CalcWantComeFreelanceWorkers();
		double AL = CalcWantComeLuxCivil();
		double heft = AH+AJ+AL;
		double AH_f = AH/heft;
		double AJ_f = AJ/heft;
		double AL_f = AH/heft;

		newAcceptedStuffWorkers = AH > H ? AH: H*AH_f;
	  	newAcceptedFreelanceWorkers = AJ > H ? AJ: H*AJ_f;
	  	newAcceptedLuxCivil = AL > H ? AL: H*AL_f;
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

