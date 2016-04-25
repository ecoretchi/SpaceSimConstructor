using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShipEngines : MonoBehaviour {

	public Vector3 engineForces;

	private List<Transform> forwardEngines;
	private List<Transform> backwardEngines;
	private List<Transform> leftEngines;
	private List<Transform> rightEngines;

	void Start() {
		forwardEngines = new List<Transform>();
		backwardEngines = new List<Transform>();
		leftEngines = new List<Transform>();
		rightEngines = new List<Transform>();

		SearchEngines();
	}

	void Update() {

		foreach (Transform t in forwardEngines) {
			foreach (Light l in t.GetComponentsInChildren<Light>()) {
				l.intensity = Mathf.Clamp( engineForces.z, 0, 1f );
			}
			foreach (JetEffect j in t.GetComponentsInChildren<JetEffect>()) {
				j.effectSize = Mathf.Clamp( engineForces.z, 0, 1f );
			}
		}

		foreach (Transform t in backwardEngines) {
			foreach (Light l in t.GetComponentsInChildren<Light>()) {
				l.intensity = -Mathf.Clamp( engineForces.z, -1f, 0 );
			}
			foreach (JetEffect j in t.GetComponentsInChildren<JetEffect>()) {
				j.effectSize = -Mathf.Clamp( engineForces.z, -1f, 0 );
			}
		}

		foreach (Transform t in leftEngines) {
			foreach (Light l in t.GetComponentsInChildren<Light>()) {
				l.intensity = Mathf.Clamp( engineForces.x, 0, 1f );
			}
			foreach (JetEffect j in t.GetComponentsInChildren<JetEffect>()) {
				j.effectSize = Mathf.Clamp( engineForces.x, 0, 1f );
			}
		}

		foreach (Transform t in rightEngines) {
			foreach (Light l in t.GetComponentsInChildren<Light>()) {
				l.intensity = -Mathf.Clamp( engineForces.x, -1f, 0 );
			}
			foreach (JetEffect j in t.GetComponentsInChildren<JetEffect>()) {
				j.effectSize = -Mathf.Clamp( engineForces.x, -1f, 0 );
			}
		}


	}

	public void SetEngineForces( float x, float y, float z ) {
		engineForces.x = x;
		engineForces.y = y;
		engineForces.z = z;
	}

	private void SearchEngines() {
		foreach (Transform t in GetComponentsInChildren<Transform>( true )) {
			if (t.name.Contains( "Forward_" )) {
				forwardEngines.Add( t );
			}
			if (t.name.Contains( "Backward_" )) {
				backwardEngines.Add( t );
			}
			if (t.name.Contains( "Left_" )) {
				leftEngines.Add( t );
			}
			if (t.name.Contains( "Right_" )) {
				rightEngines.Add( t );
			}
		}
	}

}
