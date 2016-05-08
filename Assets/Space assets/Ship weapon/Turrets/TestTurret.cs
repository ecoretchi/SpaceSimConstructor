using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class TestTurret : MonoBehaviour {

	public	GameObject	rotationBase		= null;
	public	GameObject	barrel				= null;
	public	Transform	barrelOutDummy		= null;
	public	float		rotationSpeed		= 1f;

	public	GameObject	shotPrefab			= null;

	public	float		timeBetweenShots	= 0.1f;
	private	float		timeFromLastShot	= 0;

	void Awake () {
		if (!barrelOutDummy) {
			Debug.Log( "Searching for barrel out..." );
			// search for barrel dummy
			foreach(Transform t in transform.GetComponentsInChildren<Transform>(false)) {
				if (t.name.Contains( "Out" )) {
					barrelOutDummy = t;
					Debug.Log( "Barrel auto search found '" + t.name + "'.", gameObject );
					break;
				}
			}
		}

		Assert.IsNotNull( barrelOutDummy, "Test turret::Awake: No barrel out found!" );
		Assert.IsNotNull( shotPrefab, "Test turret::Awake: No shot prefab!" );
	}
	
	void Update () {
		if (Input.GetKey( KeyCode.Mouse0 )) {
			//TODO: make random time variation
			//TODO: add accuracy disperse

			if (Time.time - timeFromLastShot >= timeBetweenShots) {
				Instantiate( shotPrefab, barrelOutDummy.position, barrelOutDummy.rotation );
				timeFromLastShot = Time.time;
			}
		}
	}
}
