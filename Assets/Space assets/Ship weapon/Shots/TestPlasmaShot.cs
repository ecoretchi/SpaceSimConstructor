using UnityEngine;
using System.Collections;
using Spacecraft;
using System;

public class TestPlasmaShot : MonoBehaviour {

	public float speed;  // shot speed
	public float maxDistance; // max travel distance
	public float safeZone;  //distance of no collision detection
	public SpacecraftGeneric owner; // owner of shot
	private Vector3 normalScale;

	public float interval = 0.3f;
	private bool hasHit = false;
	private Vector3 begin;
	private float timer = 0.0f;
	private float timeTillImpact = 0.0f;
	private RaycastHit hit;

	private float travelledDistance;

	void Awake() {
		gameObject.layer = LayerMask.NameToLayer( "Ignore Collision" );
		normalScale = transform.localScale;
	}

	void OnEnable() {
		transform.localScale.Set( 1f, 1f, 0.5f );
		timer = 0f;
		hasHit = false;
		travelledDistance = 0f;
		begin = transform.position;
		timer = interval + 1f;
	}

	// Update is called once per frame
	void Update() {

		// don't allow an interval smaller than the frame.
		var usedInterval = interval;

		if (Time.deltaTime > usedInterval) {
			usedInterval = Time.deltaTime;
		}

		// every interval, we cast a ray forward from where we were at the start of this interval
		// to where we will be at the start of the next interval
		if (!hasHit && timer >= usedInterval) {
			timer = 0;
			var distanceThisInterval = speed * usedInterval;

			if (Physics.Raycast( begin, transform.forward, out hit, distanceThisInterval )) {
				if (hit.rigidbody != GetComponent<Rigidbody>()) {
					hasHit = true;
					if (speed != 0) {
						timeTillImpact = hit.distance / speed;
					}
				}
			}

			begin += transform.forward * distanceThisInterval;
		}

		timer += Time.deltaTime;
		travelledDistance += speed * Time.deltaTime;

		// after the Raycast hit something, wait until the bullet has traveled
		// about as far as the ray traveled to do the actual hit
		if (hasHit && timer > timeTillImpact) {
			// Do hit
			Debug.Log( "HIT!" );
			selfDestruct();

		} else {
			transform.position += transform.forward * speed * Time.deltaTime;
		}

		if (travelledDistance >= safeZone) {
			//we've passed safe zone, turn on collisions by changing layer
			GetComponent<Rigidbody>().gameObject.layer = LayerMask.NameToLayer( "Default" );
			transform.localScale = normalScale;
		}

		if (travelledDistance >= maxDistance) {
			//maximum reached, destroy
			selfDestruct();
		}
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
