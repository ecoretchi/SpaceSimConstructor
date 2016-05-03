using UnityEngine;
using System.Collections;

public class TestTurret : MonoBehaviour {

	public GameObject rotationBase;
	public GameObject barrel;
	public Transform barrelOutDummy;
	public float rotationSpeed;

	public GameObject shotPrefab;
	public float timeBetweenShots;
	private float timeFromLastShot;

	void Start () {
	
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
