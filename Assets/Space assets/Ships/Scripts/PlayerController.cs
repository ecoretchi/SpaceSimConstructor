using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System;

namespace Spacecraft {

	public class PlayerController : MonoBehaviour {

		public Camera cameraToUse;

		// Управление камерой
		public  Transform   cameraPositionsSource;
		private Transform[] cameraPositions;
		private int         cameraIndex     = 0; //индекс камеры, которую надо использовать
		private bool        _isDamping      = false;
		private float       _dampSpeed      = 10f;

		private Spacecraft_Generic  _ship;



		// Unity callbacks ////////////////////////////////
		// Use this for initialization
		void Start() {
			_ship = GetComponentInParent<Spacecraft_Generic>();
			Assert.IsNotNull(_ship, "PlayerController::Start > No parent Spacecraft_Generic found!");
		}	

		// Update is called once per frame
		void Update() {

			if (Input.GetKeyUp(KeyCode.F1)) {
				swithCamera();
			}

			//Читаем устройства ввода
			if (GameController.instance.isCursorLocked) {
				_ship.setShipControls(	Input.GetAxis("Horisontal Strafe") * _ship.rotationXForce,        // direction x
										Input.GetAxis("Vertical Strafe") * _ship.rotationYForce,          // direction y
										Input.GetAxis("Speed") * _ship.maxMainEngineForce,                // direction z
										Input.GetAxis("Vertical") * _ship.rotationXForce,                 // rotation x
										Input.GetAxis("Horizontal") * _ship.rotationYForce,               // rotation y
										Input.GetAxis("Roll") * _ship.rotationZForce);                     // rotation z
			}
		 
		}

		void FixedUpdate() {
			// если это перенести в LateUpdate, то камера адово дергается
			if (_isDamping) {
				cameraToUse.transform.position = Vector3.Lerp(cameraToUse.transform.position, cameraPositions[cameraIndex].position, Time.smoothDeltaTime * _dampSpeed);
				cameraToUse.transform.rotation = cameraPositions[cameraIndex].rotation;
			}
		}

		void LateUpdate() {
			// а если это перенести в FixedUpdate, то камера "плавает", как будто пружинящая (видимо, позиции трансформов еще не обновляются к тому моменту)
			if (!_isDamping) {
				cameraToUse.transform.position = cameraPositions[cameraIndex].position;
				cameraToUse.transform.rotation = cameraPositions[cameraIndex].rotation;
			}
		}


		// methods
		public void initialiseCameras() {
			if (cameraToUse == null) {
				cameraToUse = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
				if (cameraToUse == null) throw new Exception("No camera!");
			}

			prepareCameraList();
			swithCamera(0);
		}

		private void prepareCameraList() {
			if (cameraPositionsSource.childCount == 0) {
				cameraPositions = new Transform[1];
				cameraPositions[0] = transform;
			} else {
				cameraPositions = new Transform[cameraPositionsSource.childCount];
				int i = 0;
				foreach (Transform t in cameraPositionsSource) {
					cameraPositions[i++] = t;
				}
			}
		}

		/// <summary>
		///  "Переключает" камеру на одну из позиций из списка GameObject'ов
		///  Если позиций камер не обнаружено, то поставит просто в центр текущего GameObject.
		/// </summary>
		/// <param name="toSwitchIndex">
		///  Индекс позиции камеры. Если <0 - переключает на следующую. Если больше количества позиций - переключит на последнюю.
		/// </param>
		private void swithCamera(int toSwitchIndex = -1) {

			if (toSwitchIndex < 0) {
				if (++cameraIndex > cameraPositions.GetLength(0) - 1) {
					cameraIndex = 0;
				}
			} else {
				cameraIndex = Math.Min(toSwitchIndex, cameraPositions.GetLength(0) - 1);
			}

			CameraPositionPoint cp = cameraPositions[cameraIndex].gameObject.GetComponent<CameraPositionPoint>();
			if (cp) {
				_isDamping = cp.isDamped;
				_dampSpeed = cp.dampingSpeed;
			}

		}


	}
}