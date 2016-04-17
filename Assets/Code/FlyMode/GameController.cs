using UnityEngine;
using System.Collections;
using System;

public class GameController : Singleton<GameController> {

	// Unity callbacks /////////////////////////////////////////////////////////////////////////////////////////
	void Start () {
	
	}
		
	void Update() {
		// Выход. Пока вот такой вот простой
		if (Input.GetButtonUp( "Cancel" )) {
			quitApplication();
		}

	}

	void OnEnable() {
		isNoCursorMode = true;
	}

	void OnDisable() {
		isNoCursorMode = false;
	}


	// Properties //////////////////////////////////////////////////////////////////////////////////////////////
	public bool isCursorLocked {
		set {
			Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !value;
		}
		get {
			return !Cursor.visible;
		}
		
	}
	
	/// <summary>
	/// true если мы в режиме убранной мыши
	/// </summary>
	public bool isNoCursorMode { get; set; }


	// Public methods //////////////////////////////////////////////////////////////////////////////////////////
	public void quitApplication() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

}
