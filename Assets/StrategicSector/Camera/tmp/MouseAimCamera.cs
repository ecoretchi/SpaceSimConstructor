using UnityEngine;
using System.Collections;

public class MouseAimCamera : MonoBehaviour {

    public GameObject target;
    public float rotateSpeed = 5;
    Vector3 offset;
    GameObject targetTmp;
    void Start()
    {
        offset = target.transform.position - transform.position;
        targetTmp = new GameObject();
        targetTmp.transform.Rotate(target.transform.eulerAngles);
    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        targetTmp.transform.Rotate(0, horizontal, 0);
        
        float desiredAngle = targetTmp.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = target.transform.position - (rotation * offset);

        transform.LookAt(target.transform);
    }

}
