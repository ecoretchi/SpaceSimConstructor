using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AbstractThirdCamera))]
public class SpinAround : MonoBehaviour {

    AbstractThirdCamera thirdCam;
    public float rotateFriction = 0.05f;
    public float rotateSpeed = 5;

    [HideInInspector]
    public float torque;

    [HideInInspector]
    public int MouseButtonID = 0;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        thirdCam = cam.GetComponent<AbstractThirdCamera>();
    }

    // Update is called once per frame
    void Update() {
        Transform target = thirdCam.GetTarget();
        Vector3 offset = thirdCam.GetOffset();

        if(Input.GetMouseButton(MouseButtonID))
        {
            torque = Input.GetAxis("Mouse X")*rotateSpeed;
            
        }
        if (Input.GetMouseButtonDown(MouseButtonID))
            thirdCam.CalcDesiredPosition(false);
        if (Input.GetMouseButtonUp(MouseButtonID))
            thirdCam.CalcDesiredPosition(true);

		if (Mathf.Abs(torque) > Mathf.Deg2Rad)
        {
			torque = Mathf.Lerp(torque, 0, rotateFriction); 

            Quaternion rotation = Quaternion.Euler(0, torque, 0);
            offset = rotation * offset;
            Vector3 newPos = target.position + offset;
            thirdCam.ChangePosition(newPos);
            thirdCam.SetOffset(offset);//remember offset
        }


    }

}
