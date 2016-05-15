using UnityEngine;
using System.Collections;

public class StationManager : MonoBehaviour {

	//station characteristics
    public StationData m_stationInfo;

    MainStationModule m_cms;
    // Use this for initialization
    void Start () {
        m_cms = FindObjectOfType<MainStationModule>();

        // begin from build scratch, but we must save the station state in future

        m_cms.Release();

    }

    // Update is called once per frame
    void Update () {
	
	}
}
public class StationData {

    public int theHull;
    public int hullWeakening;
    public double energyGWatts;
    public int transportRoutesMax;
    public int transportRoutes;

	public int lifeSupportLimit;
	public int lifeSupport;
	public double credits;
	public double freelanceTaxFactor;
	public double freelanceTaxCreditRate;//credit tax rate

	public int socialApartments;
	public int conventionalApartments;
	public int luxApartments;

	public double stuffWorkers; // citizen for maintenance
	public double freelanceWorkers; // citizen for freelance
	public double luxCivil; // VIP citizen for high business

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
		return (int)(E7-O7);
	}
	public double CalcTaxProfit(){ 
		return freelanceTaxFactor * freelanceTaxCreditRate;
	}
	public int CalcCivil(){
        return (int)(stuffWorkers + freelanceWorkers + luxCivil);
	}
	public int CalcApartmentsTotal(){ 
	 return (int)(socialApartments + 
	 	conventionalApartments +
	 	luxApartments);
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
		wantComeStuffWorkers = (int)(stuffWorkersFactorCome * C7 * freeRooms);
		return wantComeStuffWorkers;
	}
	public int CalcWantComeFreelanceWorkers(){ 
		double C7 = CalcStationAppeal(60);
		double freeRooms = conventionalApartments - freelanceWorkers;
		wantComeFreelanceWorkers =  (int)(freelanceWorkersFactorCome * C7 * freeRooms);
		return wantComeFreelanceWorkers;
	}
	public int CalcWantComeLuxCivil(){ 
		double C7 = CalcStationAppeal(70);
		double freeRooms = luxApartments - luxCivil;
		wantComeLuxCivil = (int)(luxCivilFactorCome * C7 * freeRooms);
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

		newAcceptedStuffWorkers = (int)(AH > H ? AH: H*AH_f);
        newAcceptedFreelanceWorkers = (int)(AJ > H ? AJ : H * AJ_f);
	  	newAcceptedLuxCivil = (int)(AL > H ? AL: H*AL_f);
	}
}
//http://answers.unity3d.com/questions/901770/invoke-method-with-reflection-c.html
