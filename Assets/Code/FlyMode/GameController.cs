using UnityEngine;
using System.Collections;
using System;

public class GameController : Singleton<GameController> {

	// Unity callbacks /////////////////////////////////////////////////////////////////////////////////////////
	void Start () {
	
	}
		
	void Update() {
		// Reading input //////////////////////////////
		if (Input.GetKeyDown(KeyCode.LeftAlt) ){
			isCursorLocked = false;
		}

		// на всякий случай, будем следить не за getkeyup, а за не нажатым состоянием - по идее, после альт-таба может так быть
		if( isNoCursorMode && !isCursorLocked && !Input.GetKey(KeyCode.LeftAlt) ) {
			isCursorLocked = true;
		}

		// Выход. Пока вот такой вот простой
		if (Input.GetKeyUp(KeyCode.Escape)) {
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

	[SerializeField]
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
