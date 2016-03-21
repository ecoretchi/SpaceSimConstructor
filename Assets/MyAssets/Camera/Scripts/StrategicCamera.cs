using UnityEngine;
using System.Collections;
// Common strategic camera implemented: 
// 1) moving the camera by XZ plain 
// 2) following to hitted object by tag name
// 3) orbital rotating around the selected target, or invisible GameObject creted by current script
// 4) zoom: move closer to target and out away 

public class StrategicCamera : MonoBehaviour {

	// -------------- common properties --------------
	public int MouseButtonIDMoving = 0;
	public int MouseButtonIDRotateH = 1;
	public int MouseButtonIDRotateV = 1;
	public int MouseButtonZoomID = 2;


	public float flowSpeed = 0.01F;
	public float flowDamping = 0.1f;

	public Transform target;// destination look at position
	protected Transform source;// source look from position

	Vector3 offset;
	Vector3 desiredPosition; //source(as camera) desire move to that position

	protected Transform flowTarget;// virtual look at position

	//------------ properties for vertical rotate processing ------------
	public float rotateVFriction = 0.05f;
	public float rotateVSpeed = 5;

	[HideInInspector]
	public float torqueVertical; //vertical power torque

	[Range(0, 90)]
	public int meredianLimit = 0; //vertical rotation limit 

	//------------ properties for horizontal rotate processing ------------

	public float rotateHFriction = 0.05f;
	public float rotateHSpeed = 5;

	[HideInInspector]
	public float torqueHorizontal;

	//------------ properties for moving over xz plane processing ------------
	public float moveFriction = 0.1f;
	public float moveSpeed = 10;

	public Vector3 moveForward;
	public Vector3 moveLaterally;
	public float torqueForward;
	public float torqueLaterally;

	//------------ properties for zoom processing ------------

	public float minOffset = 2;
	public float maxOffset = 100;
	public float zoomFriction = 0.1f;
	public float zoomSpeed = 10;

	public float torqueZoomForward;
	public float torqueZoomWheel;

	// ------------ Main Implementation Begin ------------ 
	// Use this for initialization
	void Start () {
		source = transform;
		flowTarget = new GameObject() .transform; 
		if (!target) {
			target = flowTarget;
		}
		offset = source.position - target.position;
	}

	// Update is called once per frame
	void Update () {
		CalcTorques();

		if (IsOrbitRotating ()) {
			if (IsRotatingVertical ())
				DoRotateVertical ();			
			if (IsRotatingHorizontal ())
				DoRotateHorizontal ();
		}
		else
		{
			DoMoving ();
			DoFlow ();
			DoZooming ();
		}			
	}
	// ------------ Main Implementation End ------------ 
	bool IsOrbitRotating(){
		return IsRotatingVertical() || IsRotatingHorizontal();
	}
	void CalcTorques()
	{
		if (Input.GetMouseButton(MouseButtonIDRotateV))
			torqueVertical = Input.GetAxis("Mouse Y") * rotateVSpeed;
		if (Input.GetMouseButton(MouseButtonIDRotateH))
			torqueHorizontal = Input.GetAxis("Mouse X") * rotateHSpeed;
		if (Input.GetMouseButton (MouseButtonIDMoving)) {
			torqueForward = Input.GetAxis ("Mouse Y") * moveSpeed;
			torqueLaterally = Input.GetAxis ("Mouse X") * moveSpeed;
			if (Input.GetMouseButtonDown (MouseButtonIDMoving)) {
				flowTarget.position = source.transform.position;
				flowTarget.rotation = source.transform.rotation; //it is importat get copy rotation to get proper move forward vector
				//setup the move directions
				moveForward = Vector3.Cross (flowTarget.right, Vector3.up);
				moveLaterally = flowTarget.right;
			}
		}

		torqueZoomWheel = Input.GetAxis ("Mouse ScrollWheel");
		if(torqueZoomWheel>0) {
			torqueForward+=zoomSpeed;
		}
		else
			if(torqueZoomWheel<0) {
				torqueForward-=zoomSpeed;
			}
			else
				if (Input.GetMouseButton(MouseButtonZoomID) ){
					torqueForward = Input.GetAxis ("Mouse Y") * zoomSpeed;
					//Debug.DrawRay (source.transform.position, moveForward*moveSpeed, Color.green);
				}
	}
	// --------------- Motion Flow Begin Implementing ---------------
	void DoFlow (){

		CalcDesirePosition ();
		source.position = CalcFlowPosition ();

		flowTarget.position = Vector3.Lerp(flowTarget.position, target.position, flowSpeed);
		source.LookAt(flowTarget.position);
	}

	void CalcDesirePosition ()
	{
		desiredPosition = target.position + offset;
	}

	Vector3 CalcFlowPosition()
	{
		return Vector3.Lerp (source.position, desiredPosition, Time.deltaTime * flowDamping);
	}
	// --------------- Motion Flow End Implementing ---------------

	// Rotate Flow Implementing 
	Vector3 CalcOrbitRotatePosition()
	{
		float magnitute = offset.magnitude - Vector3.Distance (target.position, source.position);
		if (magnitute > 0) {
			return Vector3.RotateTowards (source.position, desiredPosition, flowDamping, offset.magnitude);
		}
		return source.position;
	}
		
	// --------------- Verical Rotate Begin Implementing ---------------
	void DoRotateVertical ()
	{

		torqueVertical = Mathf.Lerp(torqueVertical, 0, rotateVFriction);

		int sign = torqueVertical > 0 ? -1 : 1;
		Vector3 offsetV = new Vector3(0, offset.magnitude * sign, 0);

		float curAngle = Vector3.Angle(offset, offsetV);
		float resAngle = curAngle-Mathf.Abs (torqueVertical);

		if (resAngle > meredianLimit) {
			Vector3 desiredPosition = target.position + offsetV;

			Debug.DrawRay (target.position, desiredPosition - target.position, Color.green);
			Debug.DrawRay (source.position, desiredPosition - source.position, Color.blue);

			Vector3 side1 = source.position - target.position;
			Vector3 side2 = desiredPosition - target.position;
			Vector3 normal = Vector3.Cross (side1, side2);

			Debug.DrawRay (target.position, normal - target.position, Color.red);

			Quaternion rotation = Quaternion.AngleAxis (Mathf.Abs (torqueVertical), normal);
			offset = rotation * offset;				

			desiredPosition = target.position + offset;

			source.position = CalcOrbitRotatePosition ();

		} else
			torqueVertical = 0;
		
	}
	bool IsRotatingVertical (){
		return Mathf.Abs (torqueVertical) > 0;
	}
	// --------------- DoRotateV End Implementing ---------------
	// --------------- DoRotateH Begin Implementing ---------------
	void DoRotateHorizontal(){
		if (IsRotatingHorizontal())
		{
			torqueHorizontal = Mathf.Lerp(torqueHorizontal, 0, rotateHFriction); 

			Quaternion rotation = Quaternion.Euler(0, torqueHorizontal, 0);
			offset = rotation * offset;
			desiredPosition = target.position + offset;
			source.position = CalcOrbitRotatePosition ();

		}
	}
	bool IsRotatingHorizontal(){
		return Mathf.Abs (torqueHorizontal) > Mathf.Deg2Rad;
	}
	// --------------- DoRotateH End Implementing ---------------
	// --------------- Moving Begin Implementing ---------------
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
			source.transform.position = source.transform.position + newPos;

			flowTarget.position = source.transform.position - offset;

			target = flowTarget;
			desiredPosition = source.transform.position;
		}
	}

	bool IsMovingForward(){
		return  Mathf.Abs (torqueForward) > 1;
	}
	bool IsMovingLaterally(){
		return Mathf.Abs (torqueLaterally) > 1;
	}
	bool IsMoving(){
		return IsMovingForward () || IsMovingLaterally ();
	}
	// --------------- Moving End Implementing ---------------
	// --------------- Zooming Begin Implementing ---------------
	void DoZooming()
	{		
		bool isTorqueF = Mathf.Abs (torqueZoomForward) > 0;

		if (isTorqueF) {
			torqueZoomForward = Mathf.Lerp(torqueZoomForward, 0, zoomFriction);

			float curDistance = offset.magnitude;

			if (//avoid changing distance near the target only during zooming in
				( curDistance > torqueZoomForward + minOffset && torqueZoomForward > 0 ) || 
				//avoid changing distance far away from target only during zooming out
				(curDistance < maxOffset && torqueZoomForward < 0)) {
				source.Translate (Vector3.forward * torqueZoomForward);
				offset = source.position - target.position;
			}			
		}
	}
	// --------------- Zooming End Implementing ---------------

		
}
