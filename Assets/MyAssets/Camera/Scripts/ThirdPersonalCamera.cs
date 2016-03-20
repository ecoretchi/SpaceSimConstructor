using UnityEngine;
using System.Collections;
using System;

public class ThirdPersonalCamera : MonoBehaviour, AbstractThirdCamera
{
    public float damping = 0.1f;
    Vector3 offset;
    public Transform targetTransform;
    private GameObject flowTarget;
    private Transform flowTransform;
    public float flow_speed = 0.01F;
    bool calcDesiredPosition = true;
	bool calcPosition = true;
    Vector3 desiredPosition;

    void AbstractThirdCamera.SetTarget(Transform t)
    {
        targetTransform = t;        
    }
    void AbstractThirdCamera.SetOffset(Vector3 v)
    {		
        offset = v;
    }
    Vector3 AbstractThirdCamera.GetOffset()
    {
        return offset;
    }
    Transform AbstractThirdCamera.GetTarget()
    {
        return targetTransform;
    }
    Transform AbstractThirdCamera.GetSource()
    {
        return transform;
    }
    void AbstractThirdCamera.SetPosition(Vector3 pos)
    {
        transform.position = pos;

    }
    public void ChangePosition(Vector3 pos)
    {
        desiredPosition = pos;
    }

    public void CalcDesiredPosition(bool v)
    {
        calcDesiredPosition = v;
    }
	public void CalcdPosition(bool v)
	{
		calcPosition = v;
	}
    // Use this for initialization
    void Start()
    {
        offset = transform.position - targetTransform.position;
        flowTarget = new GameObject();
        flowTransform = flowTarget.transform;
        flowTransform.position = targetTransform.position;
    }

    void Update()
    {

        //Debug.DrawRay(transform.position, desiredPosition - transform.position, Color.red);

        //Debug.DrawRay(transform.position, targetTransform.position - transform.position, Color.green);

        //if (flowTransform.position == targetTransform.position)

		if (calcPosition) {

			if (calcDesiredPosition) {
				desiredPosition = targetTransform.position + offset;
			}

			Vector3 position;//= Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * damping);
			position = Vector3.Lerp (transform.position, desiredPosition, Time.deltaTime * damping);
		        
			if (!calcDesiredPosition) {
				float magnitute = offset.magnitude - Vector3.Distance (targetTransform.position, position);
				if (magnitute > 0) {
					position = Vector3.RotateTowards (transform.position, desiredPosition, damping, offset.magnitude);
				}
			}

			transform.position = position;

	        if (flowTransform)
	        {            
	            flowTransform.position = Vector3.Lerp(flowTransform.position, targetTransform.position, flow_speed);
	            //flowTransform.position = Vector3.RotateTowards(flowTransform.position, targetTransform.position, 0.0001f, 0f);
	            transform.LookAt(flowTransform.position);
	        }    
		}

    }

}