using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Environment : MonoBehaviour {

    public Material skybox;
    
	void Start () {
        RenderSettings.skybox = skybox;
	}
}
