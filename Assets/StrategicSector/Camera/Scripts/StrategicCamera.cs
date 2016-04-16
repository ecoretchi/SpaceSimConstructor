using UnityEngine;
using System.Collections;
// Common strategic camera implemented: 
// 1) moving the camera by XZ plain 
// 2) following to hitted object by tag name
// 3) orbital rotating around the selected target, or invisible GameObject created by current script
// 4) zoom: move closer to target and out away 

public class StrategicCamera : HitSelectObjectByTag {

	[Header("Strategic Camera")]
	[Tooltip("common properties")]
	// -------------- common properties --------------
	public int MouseButtonIDMoving = 0;
	public int MouseButtonIDRotateH = 1;
	public int MouseButtonIDRotateV = 1;
	public int MouseButtonZoomID = 2;

	GameObject targetObj;

	Transform target; //current look to that position
	Transform source; //source look from that position

	public Vector3 offset { get; set; } //current offset between source and target

	Vector3 desiredTarget; //source(as camera) desire look to that position

	//------------ properties for flow processing ------------
	[Header("Motion flow")]

	public float flowSpeed = 0.1f;
	public float flowDamping = 0.1f;

	//protected Transform flowTarget; //virtual look at position

	//------------ properties for vertical rotate processing ------------
	[Header("Vertical rotate ")]

	public float rotateVFriction = 0.05f;
	public float rotateVSpeed = 5;

	[HideInInspector]
	public float torqueVertical; //vertical power torque

	[Range(0, 90)]
	public int meredianLimit = 0; //vertical rotation limit 

	//------------ properties for horizontal rotate processing ------------
	[Header("Horizontal rotate")]

	public float rotateHFriction = 0.05f;
	public float rotateHSpeed = 5;

	[HideInInspector]
	public float torqueHorizontal;

	//------------ properties for moving over xz plane processing ------------
	[Header("Horizont Moving")]

	[Range(1, 5)]
	public int expanentMove = 2; // zoom speed increased over far distance 

	public float moveFriction = 0.01f;
	public float moveSpeed = 10;

	Vector3 moveForward;
	Vector3 moveLaterally;

	[HideInInspector]
	public float torqueForward;

	[HideInInspector]
	public float torqueLaterally;

	//------------ properties for zoom processing ------------
	[Header("Zoom")]

	[Range(0, 1)]
	public float expanentZoom = 0.05f; // zoom speed increased over far distance 

	public float minOffset = 2;
	public float maxOffset = 100;
	public float zoomFriction = 0.1f;
	public float zoomSpeed = 0.1f;

	[HideInInspector]
	public float torqueZoomForward;
	[HideInInspector]
	public float torqueZoomWheel;

    float remFlowSpeed;
    float remFlowDamping;
    //------------ properties for moving border limitation------------
    // after select some target user could move target of camera not longer then predefined distance value
    public int moveDistanceLimit = 200;
    Vector3 lastDesiredTarget; // the position that was selected by hit, see external using of the method SetDesiredTarget and OnTargetHitRelease

    override public int GetHitTransformMask() {
        return ( 1 << 8 ) | 1;
    }
    // ------------ Public Interface Implementation Begin ------------     
    public bool IsOnActionByMouse(int buttonId) {
	    if(buttonId == MouseButtonIDMoving)
            return IsMoving();
	    if(buttonId == MouseButtonIDRotateH)
            return IsRotatingHorizontal();
	    if(buttonId == MouseButtonIDRotateV)
            return IsRotatingVertical();
	    if(buttonId == MouseButtonZoomID)
            return IsZooming();
        return false;
    }
    public void SetTarget(Vector3 pos) {
        print("SetTaret");
        target.position = pos;
        offset = source.position - target.position;
        source.LookAt(target.position);

    }
    public void SetDesiredTarget(Vector3 pos, float flowFactor) {

        this.flowSpeed = this.remFlowSpeed * flowFactor;
        this.flowDamping = this.remFlowDamping * flowFactor;

        desiredTarget = pos;
        lastDesiredTarget = pos;
    }
    public void OnLerpLookAt(Vector3 pos, float flowFactor) {
        
        this.flowSpeed = this.remFlowSpeed * flowFactor;
        this.flowDamping = this.remFlowDamping * flowFactor;
        
        pos = target.position + (pos - target.position) * flowFactor / 5;

        offset = source.position - pos;
        desiredTarget = pos;
    }
    public void MoveForwardHorizaontal(float moveFactor) {
        torqueForward = moveFactor;
        moveForward = Vector3.Cross(source.right, Vector3.up);
    }
    //turn horizontal function allow to rotate camera left or right same as `free look` methods, but without change angle look between XZ plane and camera looking raycast
    public void TurnHorizaontal(float turnAngle) {
        
        //TODO: not working, have to fix it ...

        ////torqueHorizontal = turnAngle;
        //turnAngle = Mathf.Deg2Rad;
        //Vector3 offsetXZ = offset;
        //offsetXZ.y = 0;

        //Quaternion rotation = Quaternion.Euler(0, turnAngle, 0);
        //offsetXZ = rotation * offsetXZ;

        //Vector3 rotationPoint = target.position + offsetXZ;

        //target.position = Vector3.RotateTowards(target.position, rotationPoint, Mathf.Abs(turnAngle), offsetXZ.magnitude);
        //offset = source.position - target.position;
    }
    
    // ------------ Main Implementation Begin ------------ 
    // Use this for initialization
    void Awake() {
        print("Strategic camera Awake");
        targetObj = new GameObject();
        if (!target) {
            target = targetObj.transform;
        }
    }
    override public void OnStart() {
		source = base.currCamera.transform;
		offset = source.position - target.position;
        source.LookAt(target.position);
        remFlowSpeed = flowSpeed;
        remFlowDamping = flowDamping;
    }
	// Update is called once per frame
    override public void OnUpdate() {	
        bool ret = CalcTorques();
        if (ret)
            SetupExValueByDefault();
		Debug.DrawLine(target.position, source.position);
		DoRotateVertical();			
		DoRotateHorizontal ();
		DoZooming ();
		DoMoving ();
		DoFlow ();
	}
    void SetupExValueByDefault() {
        flowSpeed = remFlowSpeed;
        flowDamping = remFlowDamping;
    }
    // ------------ overriding class methods HitSelectObjectByTag ------------
    public bool prohibitTargetHit { get; set; }

    override public void OnTargetHitHold(Transform target)	{
	}
	override public void OnTargetHitRelease(Transform target)	{
        if(!prohibitTargetHit)
            SetDesiredTarget(target.position, 1);
	}
	// ------------ Main Implementation End ------------ 
	public bool IsOrbitRotating(){
		return IsRotatingVertical() || IsRotatingHorizontal();
	}
	bool CalcTorques()	{
        bool ret = false;
        if (Input.GetMouseButton(MouseButtonIDRotateV)) {
            torqueVertical = Input.GetAxis("Mouse Y") * rotateVSpeed;
            ret = true;
        }
        if (Input.GetMouseButton(MouseButtonIDRotateH)) {
            torqueHorizontal = Input.GetAxis("Mouse X") * rotateHSpeed;
            ret = true;
        }
		if (Input.GetMouseButton (MouseButtonIDMoving)) {
            ret = true;

            float moveSpeed_ = moveSpeed;

			if (expanentMove>0)
				moveSpeed = offset.magnitude * expanentMove;
			
			torqueForward = Input.GetAxis ("Mouse Y") * moveSpeed_;
			torqueLaterally = Input.GetAxis ("Mouse X") * moveSpeed_;
				
			if (Input.GetMouseButtonDown (MouseButtonIDMoving)) {				
				//flowTarget.position = source.position + offset;
				//flowTarget.rotation = source.rotation; //it is importat get copy rotation to get proper move forward vector
				//setup the move directions
				moveForward = Vector3.Cross (source.right, Vector3.up);
				moveLaterally = source.right;
			}
		}

		torqueZoomWheel = Input.GetAxis ("Mouse ScrollWheel") * 0.1f;


		if (IsZooming()) {
            ret = true;
            float zoomSpeed_ = zoomSpeed;
			if (expanentZoom > 0) {
				zoomSpeed_ = offset.magnitude * expanentZoom;
			}
			if (torqueZoomWheel > 0) {
				torqueZoomForward += zoomSpeed_;
			} else if (torqueZoomWheel < 0) {
				torqueZoomForward -= zoomSpeed_;
			} else if (Input.GetMouseButton (MouseButtonZoomID)) {
				torqueZoomForward = Input.GetAxis ("Mouse Y") * zoomSpeed_;
				//Debug.DrawRay (source.position, moveForward*moveSpeed, Color.green);
			}
		}
        return ret;
	}
	// --------------- Motion Flow Begin Implementation ---------------
	void DoFlow (){

		Debug.DrawLine (target.position, source.position, Color.red);

		Vector3 desiredPosition = desiredTarget + offset;

		source.position = Vector3.Lerp (source.position, desiredPosition, flowDamping);

		target.position = Vector3.Lerp(target.position , desiredTarget, flowSpeed );
        //flowTransform.position = Vector3.RotateTowards(flowTransform.position, targetTransform.position, 0.0001f, 0f);
        source.LookAt(target.position);
    }		
	// --------------- Vertical Rotate Begin Implementation ---------------
	void DoRotateVertical () {

		if (!IsRotatingVertical ())
			return;
			
		torqueVertical = Mathf.Lerp(torqueVertical, 0, rotateVFriction);

        if (Mathf.Abs(torqueVertical) < Mathf.Deg2Rad)
            torqueVertical = 0;


        int sign = torqueVertical > 0 ? -1 : 1;
		Vector3 offsetV = new Vector3(0, offset.magnitude * sign, 0);

		float curAngle = Vector3.Angle(offset, offsetV);
		float resAngle = curAngle-Mathf.Abs (torqueVertical);

		if (resAngle > meredianLimit) {
			Vector3 desiredPosition = target.position + offsetV;

			//Debug.DrawLine (target.position, desiredPosition , Color.green);
			//Debug.DrawLine(source.position, desiredPosition , Color.blue);

			Vector3 side1 = source.position - target.position;
			Vector3 side2 = desiredPosition - target.position;
			Vector3 normal = Vector3.Cross (side1, side2);

			Debug.DrawRay (source.position, normal, Color.red);

			Quaternion rotation = Quaternion.AngleAxis (Mathf.Abs (torqueVertical), normal);
			offset = rotation * offset;				

			desiredPosition = target.position + offset;

			source.position = Vector3.RotateTowards (source.position, desiredPosition, Mathf.Abs(torqueVertical), offset.magnitude);
			source.LookAt (target.position);

		} else
			torqueVertical = 0;
		
	}
	bool IsRotatingVertical (){
		return Mathf.Abs (torqueVertical) > 0;
	}
	// --------------- DoRotateV End Implementation ---------------
	// --------------- DoRotateH Begin Implementation ---------------
	void DoRotateHorizontal(){
		if (!IsRotatingHorizontal ())
			return;
        torqueHorizontal = Mathf.Lerp(torqueHorizontal, 0, rotateHFriction);

        if (Mathf.Abs(torqueHorizontal) < Mathf.Deg2Rad)
            torqueHorizontal = 0;

        Quaternion rotation = Quaternion.Euler(0, torqueHorizontal, 0);
		offset = rotation * offset;
		Vector3 desiredPosition = target.position + offset;

		source.position = Vector3.RotateTowards (source.position, desiredPosition, Mathf.Abs(torqueHorizontal), offset.magnitude);
		source.LookAt (target.position);

		//Debug.DrawRay (source.position, desiredPosition - source.position, Color.red);
		//Debug.DrawRay (target.position, desiredPosition - target.position, Color.green);
	}
	bool IsRotatingHorizontal(){
		return Mathf.Abs (torqueHorizontal) > Mathf.Deg2Rad;
	}
	// --------------- DoRotateH End Implementation ---------------
	// --------------- Moving Begin Implementation ---------------
	void DoMoving(){

		Vector3 newPos = Vector3.zero;
		if (IsMovingForward()) {			
			torqueForward = Mathf.Lerp(torqueForward, 0, moveFriction);
			newPos = -moveForward * torqueForward * Time.deltaTime;
		}

		if (IsMovingLaterally()) {			
			torqueLaterally = Mathf.Lerp(torqueLaterally, 0, moveFriction);
			newPos += -moveLaterally * torqueLaterally * Time.deltaTime;
		}

		if(IsMoving()) {
            Vector3 newSpos = source.position + newPos;            
            Vector3 newTpos = newSpos - offset; 
            
            Vector3 movingOffset = lastDesiredTarget - newTpos;
            if (movingOffset.magnitude < moveDistanceLimit) {
                source.position = newSpos;
                target.position = newTpos;
                desiredTarget = target.position;
            } else {
                torqueLaterally = 0;
                torqueForward = 0;

            }
                
        }
	}

	bool IsMovingForward(){
		return  Mathf.Abs (torqueForward) > 1;
	}
	bool IsMovingLaterally(){
		return Mathf.Abs (torqueLaterally) > 1;
	}
    public bool IsMoving(){
		return IsMovingForward () || IsMovingLaterally ();
	}
	// --------------- Moving End Implementation ---------------
	// --------------- Zooming Begin Implementation ---------------
	void DoZooming() {		

		if (!IsZooming ())
			return;

		torqueZoomForward = Mathf.Lerp(torqueZoomForward, 0, zoomFriction);

		float curDistance = offset.magnitude;

		if (//avoid changing distance near the target only during zooming in
			(curDistance > torqueZoomForward + minOffset && torqueZoomForward > 0) ||
			//avoid changing distance far away from target only during zooming out
			(curDistance < maxOffset && torqueZoomForward < 0)) {
			source.Translate (Vector3.forward * torqueZoomForward);
			offset = source.position - target.position;
		} else {
			torqueZoomForward = 0;
		}
	}
    public bool IsZooming(){
		bool ret = Mathf.Abs (torqueZoomForward) > 0.1f;
		if(!ret)
			ret = torqueZoomWheel > 0 || torqueZoomWheel < 0 || Input.GetMouseButton (MouseButtonZoomID);
		return ret;
	}
	// --------------- Zooming End Implementation ---------------

		
}
