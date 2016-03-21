using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AbstractThirdCamera))]
public class ZoomingThirdCam : MonoBehaviour {

	AbstractThirdCamera thirdCam;

	public float minOffset = 2;
	public float maxOffset = 100;
	public float zoomFriction = 0.1f;
	public float zoomSpeed = 10;

	public int MouseButtonID = 2;

	public float torqueForward;
	public float torqueWheel;


	public Vector3 rotationE;
	void Start()
	{
		Camera cam = GetComponent<Camera>();
		thirdCam = cam.GetComponent<AbstractThirdCamera>();

	}

	// Update is called once per frame
	void Update()
	{
		torqueWheel = Input.GetAxis ("Mouse ScrollWheel");
		if(torqueWheel>0) {
			torqueForward+=zoomSpeed;
		}
		else
		if(torqueWheel<0) {
			torqueForward-=zoomSpeed;
		}
		else
		if (Input.GetMouseButton(MouseButtonID) ){
			torqueForward = Input.GetAxis ("Mouse Y") * zoomSpeed;
			//Debug.DrawRay (source.transform.position, moveForward*moveSpeed, Color.green);
		}

		bool isTorqueF = Mathf.Abs (torqueForward) > 0;

		if (isTorqueF) {
			torqueForward = Mathf.Lerp(torqueForward, 0, zoomFriction);
			Transform source = thirdCam.GetSource ();
			float curDistance = thirdCam.GetOffset ().magnitude;
				
			if (//avoid changing distance near the target only during zooming in
				( curDistance > torqueForward + minOffset && torqueForward > 0 ) || 
				//avoid changing distance far away from target only during zooming out
				(curDistance < maxOffset && torqueForward < 0)) {
				source.Translate (Vector3.forward * torqueForward);
				thirdCam.SetOffset (source.position - thirdCam.GetTarget ().position);
			}			
		}
					
	}
}
