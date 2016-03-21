using UnityEngine;
using System.Collections;

public class MovingXZByMouse : SelectingMicros {
	
	void Start()
	{
		tag = "stackable";
		base.Start();
	}

	void Update()
	{
		base.Update ();

	}

	virtual public void OnTargetHitHold(Transform target)
	{
	}
	virtual public void OnTargetHitRelease(Transform target)
	{
		
	}

}
