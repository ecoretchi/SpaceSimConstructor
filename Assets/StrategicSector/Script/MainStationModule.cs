using UnityEngine;
using System.Collections;
using Stackables;

public class MainStationModule : Stackables.Module {

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
