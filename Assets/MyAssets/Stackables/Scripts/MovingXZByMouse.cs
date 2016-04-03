using UnityEngine;
using System.Collections;

public class MovingXZByMouse : HitSelectObjectByTag {

    Transform target;

    [Header("MovingXZByMouse")]
    public int MouseButtonIDMoving = 0;

    Plane curPlane;

    float flowSpeed = 1f;

    //[Range(0, 90)]
    //public int freeMoveForwardConuseAngle = 35; // turn cam at object if object out of focuse conuse, zero value disable the feature
    //[Range(0, 100)]
    //public int freeMoveDownConuseAngle = 75;


    bool lockTargetMoving = false;

    StrategicCamera strategicCamera;

    Vector3 projectedMousePosOnPlane;
    float curAngle;
    float curMeredianAngle;

    int maxFreeLookBorderV = 100;
    int maxFreeLookBorderH = 100;

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
            Vector3 pos = target.position;
            pos.y = 0;
            curPlane = new Plane(Vector3.up, pos);

        }

    }
    void Start() {
        tag = "Stackable";
        base.Start();
        strategicCamera = (StrategicCamera)GameObject.FindObjectOfType(typeof(StrategicCamera));
        
        strategicCamera.SetDesiredTarget(new Vector3(100, 0, 100),1);

    }
    void Update() {

       base.Update();

        //if (strategicCamera.IsMoving() ||
            //strategicCamera.IsZooming() ||
            //strategicCamera.IsOrbitRotating())
            //return;

        DoMoveObject(); //move the selected target
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
        DoMoveFollow();
    }

    void DoMoveObject() {
        //is any target on hold
        if (!target || lockTargetMoving)
            return;
        

        Ray ray = currCamera.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        RaycastHit hit;
        int layerMask = (1 << 8);//the constructs layer only
        //layerMask = ~layerMask;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {

            Camera cam = strategicCamera.currCamera;
            Vector3 incomingVec = hit.point - cam.transform.position;
            //Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
            //Debug.DrawRay(hit.point, reflectVec, Color.green);

            Debug.DrawLine(cam.transform.position, hit.point, Color.red);
            
            Debug.DrawRay(hit.point, hit.normal * 1000, Color.blue);

            moveTarget(hit.point);
            rotateTarget(hit.normal);

        }
        else
        if (curPlane.Raycast(ray, out rayDistance)) {
            projectedMousePosOnPlane = ray.GetPoint(rayDistance);
            if (flowSpeed < 1) {
                target.position = Vector3.Lerp(target.position, projectedMousePosOnPlane, flowSpeed);
                Vector3 offset = projectedMousePosOnPlane - target.position;
                if (offset.magnitude < 1)
                    flowSpeed = 1;
            }
            else {
                moveTarget( projectedMousePosOnPlane);
            }
        }
    }

    void moveTarget(Vector3 pos) {
        //Rigidbody rg = target.GetComponent<Rigidbody>();
        //if (rg)
        //    rg.MovePosition(pos);
        //else
            target.position = pos;
    }

    void rotateTarget22(Vector3 normal) {
        Vector3 newForward = Vector3.RotateTowards(target.forward, normal,
                                             Mathf.Deg2Rad + 10, 0);
        target.rotation = Quaternion.LookRotation(target.forward, newForward);
    }

    void rotateTarget(Vector3 normal) {
        //if (normal == target.forward)
        //    return;
        Quaternion rotate = Quaternion.FromToRotation(target.forward, normal);
        //Rigidbody rg = target.GetComponent<Rigidbody>();
        //if (rg)
        //    rg.MoveRotation(rotate);
        //else
        //target.rotation = rotate * target.rotation;

        Debug.DrawRay(target.position, target.forward * 1000, Color.blue);
        target.rotation = Quaternion.Slerp(target.rotation, rotate * target.rotation, 0.2f) ;

    }
    void DoMoveFollow() {
        if (!target)
            return;
       
        float posY = Input.mousePosition.y;

        if (posY > (Screen.height - maxFreeLookBorderV ) ) {
            float moveFactor = maxFreeLookBorderV / (Screen.height - posY + 10) * 5;
            flowSpeed = moveFactor;            
            strategicCamera.MoveForwardHorizaontal(-moveFactor);
        }else
        if(posY < maxFreeLookBorderV) {
            float moveFactor = maxFreeLookBorderV / ( posY + 10) * 5;
            flowSpeed = moveFactor;
            strategicCamera.MoveForwardHorizaontal(moveFactor);
        }
        else
            flowSpeed = 1;

    }

    /// -- prev impl -- depricated --
    //void DoLookFollow() {
    //    if (!target)
    //        return;
    //    if (curMeredianAngle < this.freeMoveDownConuseAngle)
    //    if (curAngle > freeMoveForwardConuseAngle) {
    //            float newFlowFactor = curAngle  / curMeredianAngle * 0.75f;
    //            //float newFlowFactor = curMeredianAngle / 70;
    //            flowSpeed = newFlowFactor;            
    //        strategicCamera.OnLerpLookAt(projectedMousePosOnPlane, newFlowFactor);
    //    }
    //}
    void DoLookFollow() {
        if (!target)
            return;
        float posX = Input.mousePosition.x;
        float turnFactor = 0.1f;
        if (posX > (Screen.width - maxFreeLookBorderH)) {
            turnFactor = maxFreeLookBorderH / (Screen.width - posX + 1) * 0.05f;
            strategicCamera.TurnHorizaontal(turnFactor);
        }
        else
        if (posX < maxFreeLookBorderV) {
            turnFactor = maxFreeLookBorderH / (posX + 1) * 0.05f;
            strategicCamera.TurnHorizaontal(-turnFactor);
        }
        flowSpeed = turnFactor;
        
    }
}
