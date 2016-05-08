using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Spacecraft;
using UnityEngine.Assertions;

public class ShipEngines : MonoBehaviour {

	private SpacecraftGeneric _ship;

	private List<Transform> forwardEngines;
	private List<Transform> backwardEngines;
	private List<Transform> leftEngines;
	private List<Transform> rightEngines;
	private List<Transform> upEngines;
	private List<Transform> downEngines;

	void Start() {
		forwardEngines = new List<Transform>();
		backwardEngines = new List<Transform>();
		leftEngines = new List<Transform>();
		rightEngines = new List<Transform>();
		upEngines = new List<Transform>();
		downEngines = new List<Transform>();

		SearchShip();
		SearchEngines();
	}

	void Update() {

		foreach (Transform t in forwardEngines) {
			foreach (Light l in t.GetComponentsInChildren<Light>()) {
				l.intensity = Mathf.Clamp( _ship.CurrentThrottles.z, 0, 1f );
			}
			foreach (JetEffect j in t.GetComponentsInChildren<JetEffect>()) {
				j.effectSize = Mathf.Clamp( _ship.CurrentThrottles.z, 0, 1f );
			}		
			t.gameObject.SetActive( !Mathfx.approx(_ship.CurrentThrottles.z, 0, 0.1f) );
		}

		foreach (Transform t in backwardEngines) {
			foreach (Light l in t.GetComponentsInChildren<Light>()) {
				l.intensity = -Mathf.Clamp( _ship.CurrentThrottles.z, -1f, 0 );
			}
			foreach (JetEffect j in t.GetComponentsInChildren<JetEffect>()) {
				j.effectSize = -Mathf.Clamp( _ship.CurrentThrottles.z, -1f, 0 );
			}
			t.gameObject.SetActive( !Mathfx.approx( _ship.CurrentThrottles.z, 0, 0.1f ) );
		}

		foreach (Transform t in rightEngines) {
			foreach (Light l in t.GetComponentsInChildren<Light>()) {
				l.intensity = Mathf.Clamp( _ship.CurrentThrottles.x, 0, 1f );
			}
			foreach (JetEffect j in t.GetComponentsInChildren<JetEffect>()) {
				j.effectSize = Mathf.Clamp( _ship.CurrentThrottles.x, 0, 1f );
			}
			t.gameObject.SetActive( !Mathfx.approx( _ship.CurrentThrottles.x, 0, 0.1f ) );
		}

		foreach (Transform t in leftEngines) {
			foreach (Light l in t.GetComponentsInChildren<Light>()) {
				l.intensity = -Mathf.Clamp( _ship.CurrentThrottles.x, -1f, 0 );
			}
			foreach (JetEffect j in t.GetComponentsInChildren<JetEffect>()) {
				j.effectSize = -Mathf.Clamp( _ship.CurrentThrottles.x, -1f, 0 );
			}
			t.gameObject.SetActive( !Mathfx.approx( _ship.CurrentThrottles.x, 0, 0.1f ) );
		}

		foreach (Transform t in upEngines) {
			foreach (Light l in t.GetComponentsInChildren<Light>()) {
				l.intensity = Mathf.Clamp( _ship.CurrentThrottles.y, 0, 1f );
			}
			foreach (JetEffect j in t.GetComponentsInChildren<JetEffect>()) {
				j.effectSize = Mathf.Clamp( _ship.CurrentThrottles.y, 0, 1f );
			}
			t.gameObject.SetActive( !Mathfx.approx( _ship.CurrentThrottles.y, 0, 0.1f ) );
		}

		foreach (Transform t in downEngines) {
			foreach (Light l in t.GetComponentsInChildren<Light>()) {
				l.intensity = -Mathf.Clamp( _ship.CurrentThrottles.y, -1f, 0 );
			}
			foreach (JetEffect j in t.GetComponentsInChildren<JetEffect>()) {
				j.effectSize = -Mathf.Clamp( _ship.CurrentThrottles.y, -1f, 0 );
			}
			t.gameObject.SetActive( !Mathfx.approx( _ship.CurrentThrottles.y, 0, 0.1f ) );
		}

	}

	private void SearchShip() {
		_ship = GetComponentInParent<SpacecraftGeneric>();
		Assert.IsNotNull( _ship, "ShipEngines::Start: No parent SpacecraftGeneric found!" );
	}

	private void SearchEngines() {
		foreach (Transform t in transform) {
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
			if (t.name.Contains( "Up_" )) {
				upEngines.Add( t );
			}
			if (t.name.Contains( "Down_" )) {
				downEngines.Add( t );
			}
			t.gameObject.SetActive( false );
		}
	}
}
