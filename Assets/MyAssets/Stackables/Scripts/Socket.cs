using UnityEngine;
using System.Collections;

public class Socket : MonoBehaviour {

    enum SocketT {
        st_small,
        st_medium,
        st_large,
    };

    public int type = (int)SocketT.st_small;

    public bool IsCompatible(Socket s) {
        return this.type == s.type;
    }

    public void OnJoin(Socket s) {        
        gameObject.layer = 8;
    }

    public void OnDisconnected() {
        gameObject.layer = 0;
    }

}