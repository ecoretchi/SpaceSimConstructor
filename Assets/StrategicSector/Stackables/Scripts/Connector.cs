using UnityEngine;
using System.Collections;

public class Connector : Stackable {

    override public bool OnConvergence(Socket s) {
        print("OnConvergence");
        Socket compatS = base.GetCompatibleSocket(s);
        if (!compatS) {            
            OnDivergence();
            return false;
        }

		//transform.rotation = Quaternion.Slerp(transform.rotation, s.transform.rotation * transform.rotation, 0.2f);
		//transform.rotation = s.transform.rotation * transform.rotation;



        gameObject.transform.position =             
            gameObject.transform.position -
            compatS.gameObject.transform.position +
            s.gameObject.transform.position;

		transform.rotation = s.transform.rotation;
		transform.Rotate (Vector3.up * 180);
		//base.alignUp(s.transform.up);
		//base.alignRight(s.transform.right);
		//base.alignForward(-s.transform.forward);
		//transform.Rotate( s.transform.rotation.eulerAngles - compatS.transform.localRotation.eulerAngles );

        return true;
    }
    override public bool OnDivergence() {
		
		transform.position = m_currentHit.point;//moveTarget
		base.alignForward(-m_currentHit.normal);
		//transform.Rotate (Vector3.up * 180);

        return true;
    }
}
