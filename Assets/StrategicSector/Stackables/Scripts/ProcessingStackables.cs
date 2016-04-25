using UnityEngine;
using System.Collections;

namespace Stackables {

    public class ProcessingStackables : HitSelectObjectByTag {

        Transform target = null;

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

        int maxFreeLookBorderV = 100;
        int maxFreeLookBorderH = 100;

        bool m_convergence;
        Collider m_jointsColl;

        bool m_camOnAction = false;

        MainStationModule m_mainStation;

        public bool IsTarget() {
            return target != null;
        }
        public void OnInstantiateByGUIButton(GameObject st) {
            MeshFilter mf = st.GetComponentInChildren<MeshFilter>();
            if (!mf)
                return;
            Transform tr = mf.gameObject.transform;
            m_camOnAction = true;
            OnTargetCapture(tr);
            Cursor.lockState = CursorLockMode.Locked;

        }
        public bool IsMoving() { //user hold the target and could move it or release or etc.
            return target;
        }
        override public void OnTargetHitHold(Transform target) {
        }
        override public void OnHitRelease() {
            if (m_convergence) {
                DoJoin();
                return;
            }else
            if (this.target) {
                if (m_camOnAction) {
                    m_camOnAction = false;
                    Cursor.lockState = CursorLockMode.None;
                } else {
                    Stackable s = this.target.GetComponentInParent<Stackable>();
                    if(s)
                        Destroy(s.gameObject);
                    OnTargetRelease();
                }
            }
            base.OnHitRelease();
        }
        //disabled method
        public bool disableMethod_OnTargetHitRelease = false;
        override public void OnTargetHitRelease(Transform target) {

            if (disableMethod_OnTargetHitRelease)
                return;

            if (strategicCamera.IsMoving() ||
                strategicCamera.IsZooming() ||
                strategicCamera.IsOrbitRotating()){

                if(this.target)
                    return;
                
                //avoid annoying drag cam during try to select the object
                // (-) duplicate hit release if call sequence incorrect
                strategicCamera.OnHitRelease();
                strategicCamera.StopMove();
            }     
            else if (!this.target) {
                OnTargetCapture(target);
                m_camOnAction = false;
            }
        }
        void DoJoin() {
            Stackable s = target.GetComponentInParent<Stackable>();
            if (s) {
                print("Stackables.Stackable.OnConnect");
                s.OnConnect();
                s.OnMoveOutConstruction();
            }
            OnTargetRelease();            
        }
        void OnTargetRelease() {
            strategicCamera.prohibitTargetHit = false;
            target.GetComponentInChildren<Collider>().gameObject.layer = LayerMask.NameToLayer("Construction");
            this.target = null;
            m_convergence = false;
            Cursor.visible = true;
            OnTargetReleased();
        }
        public void OnTargetCapture(Transform target) {
            strategicCamera.prohibitTargetHit = true;
            target.GetComponentInChildren<Collider>().gameObject.layer = 0;
            Cursor.visible = false;
            this.target = target;
            Vector3 pos = target.position;
            pos.y = 0;
            curPlane = new Plane(Vector3.up, pos);

            Stackable s = target.GetComponentInParent<Stackable>();
            if (s) {
                s.OnCapture();
                OnTargetCaptured();
            }
        }
        protected void OnTargetCaptured() {
            if(m_mainStation) {
                MarkerGenesis mg = m_mainStation.gameObject.GetComponentInParent<MarkerGenesis>();
                Stackable st = target.gameObject.GetComponentInParent<Stackable>();
                if (mg && st) 
                    mg.ShowMarkers(st);                
            }
        }
        protected void OnTargetReleased() {
            if (m_mainStation) {
                MarkerGenesis mg = m_mainStation.gameObject.GetComponentInParent<MarkerGenesis>();
                if (mg) {
                    mg.HideAll();
                }
            }
        }

        //==================  START  ==================
        override public void OnStart() {
            hitTag = "Stackable";
            strategicCamera = GameObject.FindObjectOfType<StrategicCamera>();
            strategicCamera.SetDesiredTarget(new Vector3(100, 0, 100), 1);
            m_mainStation = GameObject.FindObjectOfType<MainStationModule>();
        }
        //==================  UPDATE  ==================
        override public void OnUpdate() {        
            if (!m_camOnAction)
                m_camOnAction = strategicCamera.IsOnActionByMouse(MouseButtonIDMoving);

            DoMoveObject(); //move the selected target
            //DoFlowCam(); //flow cam toward target, if target out from free movement window
        }
        //float curAngle;
        //float curMeredianAngle;
        //void DoFlowCam() {
        //    ////prepare params
        //    //Transform cam = currCamera.transform;
        //    //Vector3 curCamLookAt = cam.forward;
        //    //Vector3 offset = projectedMousePosOnPlane - cam.position;
        //    //curAngle = Vector3.Angle(curCamLookAt, offset);

        //    //curMeredianAngle = Vector3.Angle(Vector3.up, -offset);
        //    //print(curMeredianAngle);

        //    //DoLookFollow();
        //    //DoMoveFollow();
        //}

        void DoMoveObject() {
            //is any target on hold
            if (!target || lockTargetMoving)
                return;

            Ray ray = currCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            int layerMask = (1 << 8);//the constructs layer only
            bool isOnConstructionCast = Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
            if (isOnConstructionCast) {
                DoMoveOverConstructon(hit);
            } else {
                OnMoveOutConstructon();
                DoMoveOverPlane(ray);
            }

        }
        void DoMoveOverPlane(Ray ray) {
            float rayDistance;
            if (!curPlane.Raycast(ray, out rayDistance))
                return;
            projectedMousePosOnPlane = ray.GetPoint(rayDistance);
            if (flowSpeed < 1) {
                target.position = Vector3.Lerp(target.position, projectedMousePosOnPlane, flowSpeed);
                Vector3 offset = projectedMousePosOnPlane - target.position;
                if (offset.magnitude < 1)
                    flowSpeed = 1;
            }
            else {
                Stackable s = target.GetComponentInParent<Stackable>();
                if(s)
                    s.gameObject.transform.position = projectedMousePosOnPlane;//moveStackable
            }
        }
        void DoMoveOverConstructon(RaycastHit hit) {

            ////Debug: Show reflection ray:
            //Camera cam = strategicCamera.currCamera;
            //Vector3 incomingVec = hit.point - cam.transform.position;
            //Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
            //Debug.DrawRay(hit.point, reflectVec, Color.green);
            //Debug.DrawLine(cam.transform.position, hit.point, Color.red);
            //Debug.DrawRay(hit.point, hit.normal * 1000, Color.blue);

            Stackable s = target.GetComponentInParent<Stackable>();
            s.OnMoveOverConstruction(hit);
            m_convergence = s.IsConvergence();

        }
        void OnMoveOutConstructon() {
            m_convergence = false;
            Stackable s = target.GetComponentInParent<Stackable>();
            s.OnMoveOutConstruction();
            s.OnDivergence();
        }
        void rotateTarget(Transform t, Vector3 normal) {
            Quaternion rotate = Quaternion.FromToRotation(t.forward, normal);
            t.rotation = Quaternion.Slerp(t.rotation, rotate * t.rotation, 0.2f);
        }

        //    Vector3 pos;
        //    Vector3 normal;
        //    ProcessConnection(hit, out pos, out normal);
        //    moveTarget(pos);
        //    rotateTarget(normal);
        //}

        //void ProcessConnection(RaycastHit hit, out Vector3 pos, out Vector3 normal) {

        //    pos = hit.point;
        //    normal = hit.normal;

        //    Connector c = target.GetComponent<Connector>();
        //    if (!c)
        //        return;
        //    Collider coll = hit.collider;
        //    connected = coll.tag == c.GetJointCompatibility();
        //    if (connected) {
        //        m_jointsColl = coll;
        //        pos = coll.transform.position;
        //        normal = coll.transform.forward;                        
        //    }
        //    //else
        //    //if (hit.collider.tag == "Construction") {
        //    //}

        //}
        //void moveTarget(Vector3 pos) {
        //    //Rigidbody rg = target.GetComponent<Rigidbody>();
        //    //if (rg)
        //    //    rg.MovePosition(pos);
        //    //else
        //    target.position = pos;
        //}

        //void rotateTarget22(Vector3 normal) {
        //    Vector3 newForward = Vector3.RotateTowards(target.forward, normal,
        //                                         Mathf.Deg2Rad + 10, 0);
        //    target.rotation = Quaternion.LookRotation(target.forward, newForward);
        //}

        void rotateTarget(Vector3 normal) {
            //if (normal == target.forward)
            //    return;
            Quaternion rotate = Quaternion.FromToRotation(target.forward, normal);
            //Rigidbody rg = target.GetComponent<Rigidbody>();
            //if (rg)
            //    rg.MoveRotation(rotate);
            //else
            //target.rotation = rotate * target.rotation;

            //Debug.DrawRay(target.position, target.forward * 1000, Color.blue);

            target.rotation = Quaternion.Slerp(target.rotation, rotate * target.rotation, 0.2f);
        }
        void DoMoveFollow() {
            if (!target)
                return;

            float posY = Input.mousePosition.y;

            if (posY > (Screen.height - maxFreeLookBorderV)) {
                float moveFactor = maxFreeLookBorderV / (Screen.height - posY + 10) * 5;
                flowSpeed = moveFactor;
                strategicCamera.MoveForwardHorizaontal(-moveFactor);
            }
            else
            if (posY < maxFreeLookBorderV) {
                float moveFactor = maxFreeLookBorderV / (posY + 10) * 5;
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

} // namespace Stackables