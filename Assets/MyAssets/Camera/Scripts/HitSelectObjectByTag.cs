using UnityEngine;
using System.Collections;


public class HitSelectObjectByTag : MonoBehaviour 
{
	[Header("HitSelectObjectByTag")]

	[HideInInspector]
	public Camera camera;
	int MouseHitID = 0;
	Transform tmpHitSelected;
	RaycastHit hitInfo;
	public string tag = "Construction";

	protected void Start()
	{
		hitInfo = new RaycastHit();
		if(!camera)
			camera = GetComponent<Camera>();

	}

	bool GetHitTransform(out Transform t, string tag)
	{

		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		bool res = Physics.Raycast(ray, out hitInfo);
		t = hitInfo.transform;        
		return res && t.tag == tag;
	}

	protected void Update()
	{

		if (Input.GetMouseButtonDown(MouseHitID))
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
		if (Input.GetMouseButtonUp(MouseHitID) )
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
	}
}
