using UnityEngine;
using System.Collections;


/// <summary>
/// Distant sun hundler.
/// Перемещает солнце на нужные сферические координаты и небольшое отдаление от положения камеры.
/// </summary>
public class DistantSun : MonoBehaviour {

	public float longitude;
	public float latitude;
	public float distanceFromCamera;
	public Camera cameraPosition; //Камера, по которой центрируется вид

	private Vector3 oldCamPosition;
	
	// Use this for initialization
	void Start () {
		oldCamPosition = Vector3.zero;
		if (!cameraPosition) {
			cameraPosition = Camera.main;
		}
	}

	// Update is called once per frame
	void Update() {
		if (Vector3.SqrMagnitude(cameraPosition.transform.position - oldCamPosition) > 0.001f ) {
			oldCamPosition = cameraPosition.transform.position;

			GameObject axis = new GameObject("Axis");

			axis.transform.rotation = Quaternion.Euler( new Vector3(-latitude, longitude, 0f) );

			Vector3 position = axis.transform.TransformDirection( new Vector3(0, 0, distanceFromCamera ) ) + cameraPosition.transform.position;

			if (Application.isPlaying) {
				Object.Destroy(axis);
			} else {
				Object.DestroyImmediate(axis);
			}

			transform.position = position;
			transform.LookAt(cameraPosition.transform);
		}
	}
}
