﻿using UnityEngine;
using System.Collections;
namespace Stackables {
    public class Connector : Stackable {

        override public bool OnConvergence(Socket hitSock) {
            //print("OnConvergence");
            //Module m = s.GetComponentInParents<Module>(2);
            //if (!m) {
            //    OnDivergence();
            //    return false;
            //}
            Socket compatS = base.GetCompatibleSocket(hitSock);
            if (!compatS) {
                OnDivergence();
                return false;
            }

            //transform.rotation = Quaternion.Slerp(transform.rotation, s.transform.rotation * transform.rotation, 0.2f);
            //transform.rotation = s.transform.rotation * transform.rotation;

            gameObject.transform.position =
                gameObject.transform.position -
                compatS.gameObject.transform.position +
                hitSock.gameObject.transform.position;

            Quaternion rotate = hitSock.transform.rotation;
            //Quaternion.Euler(Vector3.up * 180);
            //Vector3 e = rotate.eulerAngles;

            transform.rotation = rotate;
            transform.Rotate(Vector3.up * 180);
            

            //base.alignUp(s.transform.up);
            //base.alignRight(s.transform.right);
            //base.alignForward(-s.transform.forward);
            //transform.Rotate( s.transform.rotation.eulerAngles - compatS.transform.localRotation.eulerAngles );

            return base.OnConvergence(hitSock);
        }

    }
} //namespace Stackables