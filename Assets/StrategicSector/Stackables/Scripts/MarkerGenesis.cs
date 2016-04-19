using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Stackables {
    /// <summary>
    /// generate markers  for stackable object group
    ///  (+) add as child game object with special mesh for each socket
    /// </summary>
    /// 
public class MarkerGenesis : MonoBehaviour {

    List<SocketHandler> sockMarkers = new List<SocketHandler> ();
    Stackable stackableObj;

    Vector3 vScalesF = new Vector3(1, 1, 1);

	// Use this for initialization
	void Start () {
        string folder = "StrategicSector/";
        GameObject marker = (GameObject)Resources.Load(folder + "Marker", typeof(GameObject));

        stackableObj = GetComponent<Stackable>();
        if (stackableObj) {
            List<Socket> ss = stackableObj.GetSockets();
            foreach (Socket s in ss) {
                GameObject markerNew = Instantiate(marker);
                SocketHandler sh = markerNew.GetComponent<SocketHandler>();
                if (!sh)
                    continue;
                Transform tSocket = s.GetComponent<Transform>();
                Transform tMarker = sh.GetComponent<Transform>();
                tMarker.position = tSocket.position;
                tMarker.rotation = tSocket.rotation;
                Vector3 scaleV = tMarker.localScale;

                switch (s.dimType) {
                    case Socket.DimensionType.Large:
                        scaleV = scaleV * 2.5f;
                        break;
                    case Socket.DimensionType.Medium:
                        scaleV = scaleV * 2;
                        break;
                    case Socket.DimensionType.Small:
                        //tMarker.localScale += vScalesF;
                        break;
                }
                scaleV.z = tMarker.localScale.z;
                tMarker.localScale = scaleV;

                sh.gameObject.SetActive(false);
                
                sh.Init(s);
                sockMarkers.Add(sh);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        //color marker effect animation
	}

    public void ShowMarkers(Socket.DimensionType type) {
        foreach (SocketHandler sh in sockMarkers) {            
                sh.gameObject.SetActive(sh.sock.dimType == type);            
        }
    }

    public void HideAll() {
        foreach (SocketHandler sh in sockMarkers) {
            sh.gameObject.SetActive(false);
        }

    }
}

}//namespace Stackables