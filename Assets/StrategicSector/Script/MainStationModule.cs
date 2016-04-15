using UnityEngine;
using System.Collections;

public class MainStationModule : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Release() {
        Socket[] socks = gameObject.GetComponentsInChildren<Socket>();
        foreach (Socket s in socks) {
            s.OnRelease();
        }
    }
}
