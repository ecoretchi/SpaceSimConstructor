using UnityEngine;
using System.Collections;

public class StationManager : MonoBehaviour {

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
