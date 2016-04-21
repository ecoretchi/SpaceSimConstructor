using UnityEngine;
using System.Collections;

namespace Stackables {
    public class SocketHandler : MonoBehaviour {
        public Socket sock {get; protected set;}
        public void Init(Socket s) {
            sock = s;
            s.marker = this;
        }
    }

}//namespace Stackables 