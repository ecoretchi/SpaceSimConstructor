using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AbstractThirdCamera))]
public class SpinVertical : MonoBehaviour {

    AbstractThirdCamera thirdCam;
    public float rotateFriction = 0.05f;
    public float rotateSpeed = 5;

    [HideInInspector]
    public float torque;

    public int MouseButtonID = 0;

	[Range(0, 90)]
	public int meredianLimit = 0;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        thirdCam = cam.GetComponent<AbstractThirdCamera>();
    }
    // Update is called once per frame
    void Update()
    {
		Transform target = thirdCam.GetTarget();

        Vector3 offset = thirdCam.GetOffset();
        Transform source = thirdCam.GetSource();
        if (Input.GetMouseButton(MouseButtonID))
        {
            torque = Input.GetAxis("Mouse Y") * rotateSpeed;
        }
		if (Input.GetMouseButtonDown (MouseButtonID)) 
			thirdCam.CalcDesiredPosition(false);
		
        if (Input.GetMouseButtonUp(MouseButtonID))
			thirdCam.CalcDesiredPosition(true);
		
        if (torque != 0)
        {
			torque = Mathf.Lerp(torque, 0, rotateFriction);

            int sign = torque > 0 ? -1 : 1;
            Vector3 offsetV = new Vector3(0, offset.magnitude * sign, 0);

			float curAngle = Vector3.Angle(offset, offsetV);
			float resAngle = curAngle-Mathf.Abs (torque);

			if (resAngle > meredianLimit) {
				Vector3 desiredPosition = target.position + offsetV;

				Debug.DrawRay (target.position, desiredPosition - target.position, Color.green);
				Debug.DrawRay (source.position, desiredPosition - source.position, Color.blue);

				Vector3 side1 = source.position - target.position;
				Vector3 side2 = desiredPosition - target.position;
				Vector3 normal = Vector3.Cross (side1, side2);

				Debug.DrawRay (target.position, normal - target.position, Color.red);

				Quaternion rotation = Quaternion.AngleAxis (Mathf.Abs (torque), normal);
				offset = rotation * offset;				
			
				Vector3 newPos = target.position + offset;

				thirdCam.ChangePosition (newPos);
				thirdCam.SetOffset (offset);//remember offset
			} else
				torque = 0;
		}
        }
}
