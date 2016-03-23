using UnityEngine;
using System.Collections;

public class MovingXZByMouse : HitSelectObjectByTag {

    Transform target;

    [Header("MovingXZByMouse")]
    public int MouseButtonIDMoving = 0;

    Plane curPlane;

    float flowSpeed = 1f;

    [Range(0, 90)]
    public int freeMoveForwardConuseAngle = 35; // turn cam at object if object out of focuse conuse, zero value disable the feature
    [Range(0, 100)]
    public int freeMoveDownConuseAngle = 75;

    [Range(0, 100)]//TODO: not implemented feature
    public int freeMoveDistance = 100; // move cam following to object if object distanced far away, zero value disable the feature


    bool lockTargetMoving = false;

    StrategicCamera strategicCamera;

    Vector3 projectedMousePosOnPlane;
    float curAngle;
    float curMeredianAngle;

    

    override public void OnTargetHitHold(Transform target) {

    }
    override public void OnTargetHitRelease(Transform target) {

        if (strategicCamera.IsMoving() ||
            strategicCamera.IsZooming() ||
            strategicCamera.IsOrbitRotating())
            return;

        if (this.target == target)
            this.target = null;
        else {
            this.target = target;
            curPlane = new Plane(Vector3.up, target.position);
        }

    }
    void Start() {
        tag = "Stackable";
        base.Start();
        strategicCamera = (StrategicCamera)GameObject.FindObjectOfType(typeof(StrategicCamera));

    }
    void Update() {
        base.Update();

        //if (strategicCamera.IsMoving() ||
            //strategicCamera.IsZooming() ||
            //strategicCamera.IsOrbitRotating())
            //return;

        DoMoving(); //move the selected target
        //DoFollow();
        //DoLookFollow();

        DoFlowCam(); //flow cam toward target, if target out from free movement window
    }

    void DoFlowCam() {
        //prepare params
        Transform cam = currCamera.transform;
        Vector3 curCamLookAt = cam.forward;
        Vector3 offset = projectedMousePosOnPlane - cam.position;
        curAngle = Vector3.Angle(curCamLookAt, offset);

        curMeredianAngle = Vector3.Angle(Vector3.up, - offset);
        //print(curMeredianAngle);
        
        DoLookFollow();
    }

    void DoMoving() {
        //is any target on hold
        if (!target || lockTargetMoving)
            return;
        

        Ray ray = currCamera.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (curPlane.Raycast(ray, out rayDistance)) {
            projectedMousePosOnPlane = ray.GetPoint(rayDistance);
            if (flowSpeed < 1) {
                target.position = Vector3.Lerp(target.position, projectedMousePosOnPlane, flowSpeed);
                Vector3 offset = projectedMousePosOnPlane - target.position;
                if (offset.magnitude < 1)
                    flowSpeed = 1;
            }else
                target.position = projectedMousePosOnPlane;
        }
    }

    void DoFollow() {
        if (!target)
            return;

        if (curAngle > freeMoveForwardConuseAngle) {
            float newFlowFactor = curAngle / 40.0f;
            flowSpeed = newFlowFactor;
            strategicCamera.SetDesiredTarget(projectedMousePosOnPlane, newFlowFactor);
        }

    }
    void DoLookFollow() {
        if (!target)
            return;
        print("DoLookFollow");
        if (curMeredianAngle < this.freeMoveDownConuseAngle)
        if (curAngle > freeMoveForwardConuseAngle) {
                float newFlowFactor = curAngle  / curMeredianAngle * 0.75f;
                //float newFlowFactor = curMeredianAngle / 70;
                flowSpeed = newFlowFactor;            
            strategicCamera.OnLerpLookAt(projectedMousePosOnPlane, newFlowFactor);
        }
    }
}
