using UnityEngine;
using System.Collections;
using Spacecraft;
using System;

public class TestPlasmaShot : MonoBehaviour {

	//private static int LAYER_DEFAULT = 0;
	//private static int LAYER_IGNORE_COLLISION =	9;

	public float speed;  // shot speed
	public float maxDistance; // max travel distance
	public float safeZone;  //distance of no collision detection
	public SpacecraftGeneric owner; // owner of shot
	private Vector3 normalScale;

	//private float maxDistanceSqr; // maxDistance * maxDistance
	private float travelledDistance;

	void Awake() {
		gameObject.layer = LayerMask.NameToLayer( "Ignore Collision" );
		normalScale = transform.localScale;
	}

	void OnEnable() {
		transform.localScale.Set( 1f, 1f, 0.5f );
	}

	// Update is called once per frame
	void Update() {
		Rigidbody rb = GetComponent<Rigidbody>();
		Vector3 curPos = rb.position;

		curPos = curPos + transform.forward * speed * Time.deltaTime;
		travelledDistance += speed * Time.deltaTime;

		if (travelledDistance >= safeZone) {
			//we've passed safe zone, turn on collisions by changing layer
			rb.gameObject.layer = LayerMask.NameToLayer( "Default" );
			transform.localScale = normalScale;
		}

		if (travelledDistance >= maxDistance) {
			//maximum reached, destroy
			selfDestruct();
		}

		rb.position = curPos;
	}

	private void OnCollisionEnter(Collision coll) {
		//TODO: check for generic interface, call it's takeDamage method?  or let it think for himself, we just selfdestroy

		selfDestruct();
	}

	private void selfDestruct() {
		gameObject.SetActive( false );
		Destroy( gameObject );
	}
}
