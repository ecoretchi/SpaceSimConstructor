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

    List<SocketMarker> sockMarkers = new List<SocketMarker> ();
    List<SocketMarker> lastAllMarkers;
    Stackable stackableObj;

    Vector3 vScalesF = new Vector3(1, 1, 1);

    bool recursionShow = true;
	// Use this for initialization
	void Start () {

        string folder = "StrategicSector/";
        GameObject marker = (GameObject)Resources.Load(folder + "Marker", typeof(GameObject));

        stackableObj = GetComponent<Stackable>();
        if (stackableObj) {
            List<Socket> ss = stackableObj.GetSockets();
            foreach (Socket s in ss) {
                GameObject markerNew = Instantiate(marker);
                
                SocketMarker sh = markerNew.GetComponent<SocketMarker>();
                if (!sh)
                    continue;

                Transform tSocket = s.GetComponent<Transform>();
                Transform tMarker = sh.GetComponent<Transform>();

                Vector3 scaleV = tMarker.localScale;

                switch (s.dimType) {
                    case Socket.DimensionType.Large:
                        scaleV = scaleV * 2.5f;
                        break;
                    case Socket.DimensionType.Medium:
                        scaleV = scaleV * 1.9f;
                        break;
                    case Socket.DimensionType.Small:
                        scaleV = scaleV * 0.9f;
                        break;
                }

                sh.gameObject.SetActive(false);
                
                sh.Init(s);
                sockMarkers.Add(sh);

                scaleV.z = tMarker.localScale.z;
                tMarker.localScale = scaleV;

                CalibrateMarker(sh);

                sh.transform.parent = s.gameObject.transform;
                //sh.transform.localPosition = Vector3.zero;
                
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        //color marker effect animation
	}
    /// <summary>
    /// set marker transform the same as socket handler transform
    /// </summary>
    /// <param name="sh">Socket handler for marker at moste</param>
    static protected void CalibrateMarker(SocketMarker sh) {
        Socket s = sh.sock;
        Transform tSocket = s.GetComponent<Transform>();
        Transform tMarker = sh.GetComponent<Transform>();
        tMarker.localPosition = Vector3.zero;// tSocket.position;
        //tMarker.position = tSocket.position;
        tMarker.rotation = tSocket.rotation;

    }

    SocketMarker collisionMarker = null;

    static public void ShowMarker(SocketMarker sm, Color c) {
        sm.gameObject.GetComponent<MeshRenderer>().material.color = c;
        sm.gameObject.SetActive(true);
        CalibrateMarker(sm);        
    }
    public void ShowMarkers(Stackable st) {       
        List<Socket> socks = ConstructorManager.GetSockets(( ref List<Socket> ss, Socket s) => {
            if (s.IsRejected())
                collisionMarker = s.marker;
            else
            if (s.IsEnabled() && s.IsCompatible(st))
                ss.Add(s);            
        });
        if (collisionMarker!=null) {
            collisionMarker.sock.OnRelease();
            ShowMarker(collisionMarker, Color.green);
            collisionMarker = null;
        }
        foreach (Socket s in socks) {
            ShowMarker(s.marker, Color.green);
        }
    }
    public void HideAll() {

        collisionMarker = null;
        List<Socket> socks = ConstructorManager.GetAllSockets();
        foreach (Socket s in socks) {
            s.marker.gameObject.SetActive(false);
            //CalibrateMarker(s.marker);
        }

    }

    public static void MarkerRelease(SocketMarker marker) {        
        ShowMarker(marker, Color.green);
    }
    public static void MarkerReject(SocketMarker marker) {
        ShowMarker(marker, Color.red);
    }

}

}//namespace Stackables