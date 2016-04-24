using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AbstractThirdCamera))]
public class SelectingMicros : MonoBehaviour
{    
	AbstractThirdCamera thirdCam;
	Camera sceneCamera;
	Transform tmpHitSelected;
	RaycastHit hitInfo;

	protected void Start()
	{
		hitInfo = new RaycastHit();
		if(!sceneCamera)
			sceneCamera = GetComponent<Camera>();

		thirdCam = sceneCamera.GetComponent<AbstractThirdCamera>();
	}

	bool GetHitTransform(out Transform t, string tag)
	{
		
		Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
		bool res = Physics.Raycast(ray, out hitInfo);
		t = hitInfo.transform;        
		return res && t.tag == tag;
	}

	protected void Update()
	{

		if (Input.GetMouseButtonDown(0))
		{
			Transform hitTransform;
			if (GetHitTransform(out hitTransform, tag))
			{
				print("Target selected");
				tmpHitSelected = hitTransform;
				OnTargetHitHold (hitTransform);

			}
			else
				tmpHitSelected = null;
		}
		if (Input.GetMouseButtonUp(0) )
		{
			Transform hitTransform;
			if ( GetHitTransform(out hitTransform, "Construction") && hitTransform==tmpHitSelected)
			{
				print("Target changes");
				OnTargetHitRelease (hitTransform);

			}
		}
	}

	virtual public void OnTargetHitHold(Transform target)
	{
	}
	virtual public void OnTargetHitRelease(Transform target)
	{
		thirdCam.SetTarget(target);
	}
}