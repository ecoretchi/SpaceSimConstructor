using UnityEngine;
using System.Collections;
namespace Stackables {
    public class Connector : Stackable {

        override public bool OnConvergence(Socket hitSock) {

            Socket compatS = base.GetCompatibleSocket(hitSock);
            if (!compatS) { //compatible socket not found
                OnDivergence();
                return false;
            }

            //Do stick to found compatible socket

            gameObject.transform.position =
                gameObject.transform.position -
                compatS.gameObject.transform.position +
                hitSock.gameObject.transform.position;

            Quaternion rotate = hitSock.transform.rotation;            
            transform.rotation = rotate;
            transform.Rotate(Vector3.up * 180);

            return base.OnConvergence(hitSock);//set stick state as result
        }
        override public Transform GetDraggingTransform() {
            Socket[] ss = this.GetComponentsInChildren<Socket>();
            if (ss.Length > 0)
                return ss[0].gameObject.transform;
            return transform; 
        }
        override protected void OnReleased(Socket mother, Socket released) {
            released.orientedType = mother.orientedType;
        }
        override protected void OnDisabled(Socket mother, Socket released) {
            released.orientedType = Socket.OrientationType.Hybrid;//all connector`s socket is hybrid during disconnected
        }
    }
} //namespace Stackables