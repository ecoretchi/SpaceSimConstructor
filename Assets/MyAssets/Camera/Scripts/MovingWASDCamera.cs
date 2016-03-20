using UnityEngine;
using System.Collections;

public class MovingWASDCamera : MonoBehaviour {

    AbstractThirdCamera thirdCam;

	public float moveFriction = 0.1f;
    public float moveSpeed = 10;

    public int MouseButtonID = 2;

	GameObject srcCopy;
	public Vector3 moveForward;
	public Vector3 moveLaterally;
	public float torqueForward;
	public float torqueLaterally;

	public float angleAxis;

	public Vector3 rotationE;
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        thirdCam = cam.GetComponent<AbstractThirdCamera>();
		srcCopy = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
		Transform target = thirdCam.GetTarget ();
		Transform source = thirdCam.GetSource();
		rotationE = source.rotation.eulerAngles;
	
		//Test debug info: draw Ray parallell (x,z)plain
		srcCopy.transform.position = source.transform.position;

		srcCopy.transform.Translate(Vector3.forward*100000,Space.World);


		Debug.DrawRay (source.transform.position, srcCopy.transform.position - source.transform.position, Color.green);
		Debug.DrawRay (target.transform.position, srcCopy.transform.position - target.transform.position, Color.blue);

//		srcCopy.transform.Translate(thirdCam.GetOffset());
//
//		Debug.DrawRay (source.transform.position, srcCopy.transform.position - source.transform.position, Color.green);
//
//		Vector3 lookPos = srcCopy.transform.position;
//		lookPos.y = source.position.y;
//
		//		Debug.DrawRay (source.transform.position, lookPos - source.transform.position, Color.red);
//		Debug.DrawRay (target.transform.position, lookPos - target.transform.position, Color.blue);
//
//		srcCopy.transform.LookAt (lookPos);
//
//		srcCopy.transform.Translate (srcCopy.transform.forward*10 );
//		Vector3 remPos = srcCopy.transform.position;

		//Debug.DrawRay (source.position, remPos - source.position, Color.green);

		//angleAxis = Vector3.Angle(source.transform.forward, Vector3.forward);

		//Let array be allways draw parallel

        if (Input.GetMouseButton(MouseButtonID))
        {
			torqueForward = Input.GetAxis ("Mouse Y") * moveSpeed;
			torqueLaterally = Input.GetAxis ("Mouse X") * moveSpeed;

        }

		if (Input.GetMouseButtonDown (MouseButtonID)) {
			thirdCam.CalcDesiredPosition (false);
			thirdCam.CalcdPosition (false);


			//get the move direct
			moveForward = srcCopy.transform.forward;
			moveLaterally = srcCopy.transform.right;
		}


		if (Mathf.Abs (torqueForward) > 1) {			
			torqueForward = Mathf.Lerp(torqueForward, 0, moveFriction);

			//target.transform.position = source.position - thirdCam.GetOffset();
			//thirdCam.SetTarget(target.transform);

			source.transform.Translate( moveForward * torqueForward * Time.deltaTime );
			//thirdCam.SetPosition (source.transform.position);
			//thirdCam.ChangePosition (source.transform.position);

		}
//		if (Mathf.Abs (torqueLaterally) > 1) {			
//			torqueLaterally = Mathf.Lerp(torqueLaterally, 0, moveFriction);
//			target.transform.Translate( - moveLaterally * torqueLaterally * Time.deltaTime );
//		}
			

		if (Input.GetMouseButtonUp (MouseButtonID)) {
			thirdCam.CalcDesiredPosition (true);
			thirdCam.CalcdPosition (true);
		}
    }
}
