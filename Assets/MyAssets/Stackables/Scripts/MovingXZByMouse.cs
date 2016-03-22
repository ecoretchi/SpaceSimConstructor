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

	override public void OnTargetHitHold(Transform target)
	{
	}
	override public void OnTargetHitRelease(Transform target)
	{
		
	}

}
