using UnityEngine;
using System.Collections;

public class MovingWASDCamera : MonoBehaviour {

    AbstractThirdCamera thirdCam;

	public float moveFriction = 0.1f;
    public float moveSpeed = 10;

    public int MouseButtonID = 2;

	GameObject targetCpy;

	public Vector3 moveForward;
	public Vector3 moveLaterally;
	public float torqueForward;
	public float torqueLaterally;


	public Vector3 rotationE;
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        thirdCam = cam.GetComponent<AbstractThirdCamera>();
		targetCpy = new GameObject ();
    }

    // Update is called once per frame
    void Update()
    {
		Transform target = thirdCam.GetTarget ();
		Transform source = thirdCam.GetSource();

        if (Input.GetMouseButton(MouseButtonID)) {
			torqueForward = Input.GetAxis ("Mouse Y") * moveSpeed;
			torqueLaterally = Input.GetAxis ("Mouse X") * moveSpeed;

			Debug.DrawRay (source.transform.position, moveForward*moveSpeed, Color.green);
        }

		if (Input.GetMouseButtonDown (MouseButtonID)) {
			
			thirdCam.CalcDesiredPosition (false);
			thirdCam.CalcdPosition (false);

			GameObject srcCopy = new GameObject();
			srcCopy.transform.position = source.transform.position;
			srcCopy.transform.rotation = source.transform.rotation; 
			//get the move direct
			moveForward = Vector3.Cross (srcCopy.transform.right, Vector3.up);
			moveLaterally = srcCopy.transform.right;
		}

		Vector3 newPos = Vector3.zero;
		bool isTorqueF = Mathf.Abs (torqueForward) > 1;
		bool isTorqueL = Mathf.Abs (torqueLaterally) > 1;
		if (isTorqueF) {			
			torqueForward = Mathf.Lerp(torqueForward, 0, moveFriction);
			newPos = -moveForward * torqueForward * Time.deltaTime;
		}

		if (isTorqueL) {			
			torqueLaterally = Mathf.Lerp(torqueLaterally, 0, moveFriction);
			newPos += -moveLaterally * torqueLaterally * Time.deltaTime;
		}

		if(isTorqueF || isTorqueL ) {
			source.transform.position = source.transform.position + newPos;

			targetCpy.transform.position = source.transform.position - thirdCam.GetOffset();
			targetCpy.transform.rotation = target.transform.rotation;


			thirdCam.SetTarget (targetCpy.transform);
			thirdCam.SetFlowTarget (targetCpy.transform);
			thirdCam.SetPosition (source.transform.position);
			thirdCam.ChangePosition (source.transform.position);

		}
		else if(!Input.GetMouseButtonDown (MouseButtonID))
		{
			
			thirdCam.CalcDesiredPosition (true);
			thirdCam.CalcdPosition (true);
		}
    }
}
