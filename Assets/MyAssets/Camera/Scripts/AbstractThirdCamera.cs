using UnityEngine;
using System.Collections;

[AddComponentMenu("AbstractThirdCamera")]
public interface AbstractThirdCamera {

	void CalcdPosition(bool v);//calculate next setep position towards to desire position
    void CalcDesiredPosition(bool v);//calculate smooth position after target are changed
    Transform GetSource();//return camera transform object
	void SetFlowTarget(Transform t);//set flow target be processing
    void SetTarget(Transform t);//set new camera taret look at
    Transform GetTarget();
    void SetOffset(Vector3 v);//offset between camera and target
    Vector3 GetOffset();
    void SetPosition(Vector3 pos);//setup new camera position looks from
    void ChangePosition(Vector3 pos);//smooth changes

}

