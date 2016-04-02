using UnityEngine;
using System.Collections;


public class HitSelectObjectByTag : MonoBehaviour 
{

	public Camera currCamera { get; set; }

	int MouseHitID = 0;
	Transform tmpHitSelected;
	RaycastHit hitInfo;

	[Header("HitSelectObjectByTag")]
	public string tag = "Construction";
    void Awake() {
        hitInfo = new RaycastHit();
    }
	protected void Start()
	{		
		if(!currCamera)
			currCamera = (Camera) GameObject.FindObjectOfType(typeof(Camera));
	}

	bool GetHitTransform(out Transform t, string tag)
	{
		if (!currCamera) {
			t = null;
			return false;
		}
		Ray ray = currCamera.ScreenPointToRay(Input.mousePosition);
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
			if ( GetHitTransform(out hitTransform, tag) && hitTransform==tmpHitSelected)
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
