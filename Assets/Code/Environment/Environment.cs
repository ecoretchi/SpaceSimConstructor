using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Environment : MonoBehaviour {

    public Material skybox;
    
	void Start () {
        RenderSettings.skybox = skybox;

#if UNITY_EDITOR
		if (Camera.main) {
			Component cameraSkybox = Camera.main.GetComponent<Skybox>();
			if (cameraSkybox) {
				if (Application.isEditor && Application.isPlaying == false) {
					Debug.LogWarning("Environment::Start: Skybox on MainCamera detected - will be deleted in playMode.", Camera.main);
				} else {
					Debug.Log("Environment::Start: Deleting Skybox component from Main Camera.", Camera.main);
					Destroy(cameraSkybox);
				}
			}
		}
#endif //UNITY_EDITOR

	}
}
