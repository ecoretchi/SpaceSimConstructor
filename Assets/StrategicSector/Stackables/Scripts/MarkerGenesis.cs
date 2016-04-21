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
    /// <summary>
    /// set marker transform the same as socket handler transform
    /// </summary>
    /// <param name="sh">Socket handler for marker at moste</param>
    protected void CalibrateMarker(SocketMarker sh) {
        Socket s = sh.sock;
        Transform tSocket = s.GetComponent<Transform>();
        Transform tMarker = sh.GetComponent<Transform>();
        tMarker.position = tSocket.position;
        tMarker.rotation = tSocket.rotation;

    }

    static bool matchEnabled(Socket s) { return s.IsEnabled(); }

    SocketMarker collisionMarker = null;

    //static void OnSocket()
    public void ShowMarkers(Stackable st) {
       
        List<Socket> socks = ConstructorManager.GetSockets(( ref List<Socket> ss, Socket s) => {
            if (s.IsRejected())
                collisionMarker = s.marker;
            else
            if (s.IsEnabled() && s.IsCompatible(st))
                ss.Add(s);            
        });
        if (collisionMarker!=null) {
            //collisionMarker.GetComponent<>;
            collisionMarker.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            //collisionMarker.gameObject.GetComponent<Material>().color = Color.red;
            collisionMarker.gameObject.SetActive(true);
            CalibrateMarker(collisionMarker);
        }
        foreach (Socket s in socks) {
            s.marker.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            s.marker.gameObject.SetActive(true);
            CalibrateMarker(s.marker);
        }

    }

    ///First version:
    //public void ShowMarkers(Stackable st) {
    //    lastAllMarkers = new List<SocketHandler>();
    //    GetCompatibleMarkers(ref lastAllMarkers, st);
    //    List<Socket> uniqSocks = st.GetTypedSockets();
    //    foreach (Socket s in uniqSocks) {   
    //        foreach (SocketHandler sh in lastAllMarkers) {
    //            if (!sh.sock.IsConnected()) {
    //                if (sh.sock.dimType == s.dimType)
    //                    if (sh.sock.IsOrientedEachOther(s))
    //                        sh.gameObject.SetActive(true);
    //                CalibrateMarker(sh);
    //            }
    //        }
    //    }
    //}
    //public void GetCompatibleMarkers(ref List<SocketHandler> markers, Stackable st, Socket enterSocket = null) {
    //    Stackable curSt;
    //    if (enterSocket) {
    //        curSt = enterSocket.GetComponentInParents<Stackable>();            
    //    }else
    //        curSt = this.GetComponent<Stackable>();
    //    bool compatible = curSt.IsCompatible(st);            
    //    foreach (SocketHandler sh in sockMarkers) {
    //        if(!sh.sock.IsConnected()){
    //            if (compatible)
    //                markers.Add(sh);
    //        } else if (enterSocket != sh.sock && sh.sock.joined) {
    //            MarkerGenesis mg = sh.sock.joined.GetComponentInParents<MarkerGenesis>();
    //            if(recursionShow)
    //                mg.GetCompatibleMarkers(ref markers, st, sh.sock.joined);
    //        }                    
    //    }
    //}
    public void HideAll() {

        List<Socket> socks = ConstructorManager.GetAllSockets();
        foreach (Socket s in socks) {
            s.marker.gameObject.SetActive(false);
            //CalibrateMarker(s.marker);
        }

    }
}

}//namespace Stackables