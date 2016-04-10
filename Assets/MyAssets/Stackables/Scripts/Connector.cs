using UnityEngine;
using System.Collections;

public class Connector : MonoBehaviour {

    public string type = "small";

    Collider m_currentColl;
    public string GetJointCompatibility() {
        if (type == "small") {
            return "Joint_S";
        }
        if (type == "medium") {
            return "Joint_M";
        }
        if (type == "large") {
            return "Joint_L";
        }
        return "";
    }
    // Use this for initialization
    void Start () {	
	}
	// Update is called once per frame
	void Update () {        
    }
    public bool IsJoined() {
        if (!m_currentColl)
            return false;
        return m_currentColl.tag == "Jointed";
    }
    virtual public void OnConnected(Collider coll) {        
        m_currentColl = coll;
        coll.tag = "Jointed";
        //print(coll.tag);
    }

    virtual public void OnDisconnected() {
        m_currentColl.tag = GetJointCompatibility();
        m_currentColl = null;        
    }
}
