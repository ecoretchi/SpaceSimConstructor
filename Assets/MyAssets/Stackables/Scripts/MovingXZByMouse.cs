using UnityEngine;
using System.Collections;

public class MovingXZByMouse : HitSelectObjectByTag {

	Transform target;

	[Header("MovingXZByMouse")]
	public int MouseButtonIDMoving = 0;

    Plane curPlane;
    
    override public void OnTargetHitHold(Transform target)
	{
		
	}
	override public void OnTargetHitRelease(Transform target)
	{

        if (this.target == target)
            this.target = null;
        else {
            this.target = target;
            curPlane = new Plane(Vector3.up, target.position);
        }

	}
	void Start()
	{
		tag = "Stackable";
		base.Start();

	}
    void Update()
    {
        base.Update();
        DoMoving();
    }

    void DoMoving()
    {
        //is any target on hold
        if (!target)
            return;
        
        Ray ray = currCamera.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (curPlane.Raycast(ray, out rayDistance))
        {
            target.position = ray.GetPoint(rayDistance);
        }
    }
    


}
