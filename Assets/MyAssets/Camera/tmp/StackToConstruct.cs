using UnityEngine;
using System.Collections;

public class StackToConstruct : MonoBehaviour {

    StrategicCamera strategicCamera;

    // Use this for initialization
    void Start () {
        strategicCamera = (StrategicCamera)GameObject.FindObjectOfType(typeof(StrategicCamera));

    }

    // Update is called once per frame
    void Update () {
	
	}


    void OnTriggerEnter(Collider other) {

        //other.transform.position

        print("StackToConstruct: OnTriggerEnter");



    }

    void OnTriggerStay(Collider other) {
        RaycastHit hit;
        Camera cam = strategicCamera.currCamera;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            
            Vector3 incomingVec = hit.point - cam.transform.position;
            Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
            Debug.DrawLine(cam.transform.position, hit.point, Color.red);
            Debug.DrawRay(hit.point, reflectVec, Color.green);
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.MovePosition(hit.point);

            //.position.y = hit.point.y;

        }
    }
}
