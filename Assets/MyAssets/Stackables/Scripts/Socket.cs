using UnityEngine;
using System.Collections;

public class Socket : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnConnected() {        
        gameObject.layer = 8;
    }

    public void OnDisconnected() {
        gameObject.layer = 0;
    }

}