using UnityEngine;
using System.Collections;


public class HitSelectObjectByTag : MonoBehaviour 
{

	public Camera currCamera { get; set; }

	int MouseHitID = 0;
	Transform tmpHitSelected;
	RaycastHit hitInfo;

    [Header("HitSelectObjectByTag")]

    public bool through_hit = true;
    protected new string tag = "Construction";
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
        int mask = GetHitTransformMask();
		Ray ray = currCamera.ScreenPointToRay(Input.mousePosition);
        if (!through_hit) {
            bool res = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask);
            t = hitInfo.transform;
            return res && t.tag == tag;
        }

        t = null;
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, mask);
        for(int i = 0; i < hits.Length; ++i) {
            hitInfo = hits[i];
            t = hitInfo.transform;
            if (t.tag == tag)
                return true;
        }        		
		return false;
	}

	protected void Update()
	{

		if (Input.GetMouseButtonDown(MouseHitID))
		{
            OnHitHold();
		}
		if (Input.GetMouseButtonUp(MouseHitID) )
		{
            OnHitRelease();
		}
	}
    virtual public int GetHitTransformMask() {
        return 1 << 8;
    }
    virtual public void OnHitHold() {
        Transform hitTransform;
        if (GetHitTransform(out hitTransform, tag)) {
            print("Target selected");
            tmpHitSelected = hitTransform;
            OnTargetHitHold(hitTransform);

        }
        else
            tmpHitSelected = null;
    }
    virtual public void OnHitRelease() {
        Transform hitTransform;
        if (GetHitTransform(out hitTransform, tag) && hitTransform == tmpHitSelected) {
            print("Target changes");
            OnTargetHitRelease(hitTransform);
        }
    }

    virtual public void OnTargetHitHold(Transform target)
	{
	}
	virtual public void OnTargetHitRelease(Transform target)
	{
	}
}
