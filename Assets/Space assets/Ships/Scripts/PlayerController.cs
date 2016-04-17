using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System;

namespace Spacecraft {

	public class PlayerController : MonoBehaviour {

        [Header("Links")]
		public Camera cameraToUse;
        public Transform cameraPositionsSource;
        public ShipUI shipUI;


        // Camera controls
		private Transform[] cameraPositions;
		private int         cameraIndex     = 0; // camera_to_use index
		private bool        _isDamping      = false;
		private float       _dampSpeed      = 10f;

		private Spacecraft_Generic  _ship; // shortcut to master

        private float       desiredSpeed = 0;    // current desired speed
        private float       rememberedSpeed = 0; // desired speed for "no override mode"
        

		// Unity callbacks ////////////////////////////////
		// Use this for initialization
		void Start() {
			_ship = GetComponentInParent<Spacecraft_Generic>();
			Assert.IsNotNull(_ship, "PlayerController::Start > No parent Spacecraft_Generic found!");

            initialiseCameras();
		}	

		// Update is called once per frame
		void Update() {

            if (Input.GetKeyUp( KeyCode.F1 )) {
                swithCamera();
            }

            processSpeedControl();

            if (Input.GetKeyDown( KeyCode.LeftAlt )) {
                GameController.instance.isCursorLocked = false;
            }

            // на всякий случай, будем следить не за getkeyup, а за не нажатым состоянием - по идее, после альт-таба может так быть
            if (GameController.instance.isNoCursorMode && !GameController.instance.isCursorLocked && !Input.GetKey( KeyCode.LeftAlt )) {
                GameController.instance.isCursorLocked = true;
            }

            // Reading input
            _ship.setShipControls( Input.GetAxis( "Horisontal Strafe" ),        // direction x
                                   Input.GetAxis( "Vertical Strafe" ),          // direction y
                                   desiredSpeed,                                // direction z
                                   GameController.instance.isCursorLocked ? Input.GetAxis( "Vertical" ) : 0,                 // rotation x
                                   GameController.instance.isCursorLocked ? Input.GetAxis( "Horizontal" ) : 0,               // rotation y
                                   Input.GetAxis( "Roll" ) );                    // rotation z
            

        }

        private void processSpeedControl() {
            float tempSpeed = desiredSpeed;

            if (tempSpeed == 0 && Input.GetKeyDown( KeyCode.Z )) {
                tempSpeed = -0.1f;
            } else {
                if (Input.GetKey( KeyCode.Z )) {
                    tempSpeed = tempSpeed - 0.6f * Time.deltaTime;
                    if (tempSpeed < -0.4f) {
                        tempSpeed = -0.4f;
                    }
                }
            }

            if (tempSpeed == 0 && Input.GetKeyDown( KeyCode.X )) {
                tempSpeed = 0.1f;
            } else {
                if (Input.GetKey( KeyCode.X )) {
                    tempSpeed = tempSpeed + 0.6f * Time.deltaTime;
                    if (tempSpeed > 1f) {
                        tempSpeed = 1f;
                    }
                }
            }
            
            if (Input.GetButtonDown( "Forward" )) {
                // temporary override speed control
                rememberedSpeed = tempSpeed;
                tempSpeed = 1f;
            }

            if (Input.GetButtonDown( "Backward" )) {
                // temporary override speed control
                rememberedSpeed = tempSpeed;
                tempSpeed = -0.4f;
            }

            if (Input.GetButtonUp( "Forward" )) {
                // return control to speed widget
                tempSpeed = rememberedSpeed;
            }

            if (Input.GetButtonUp( "Backward" )) {
                // return control to speed widget
                tempSpeed = rememberedSpeed;
            }

            // update widget only on changes
            if (!Mathf.Approximately( tempSpeed, desiredSpeed )) {
                desiredSpeed = tempSpeed;
                shipUI.speed = desiredSpeed;
            } else {
                desiredSpeed = shipUI.speed;
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


        // methods ////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///  "Переключает" камеру на одну из позиций из списка GameObject'ов
        ///  Если позиций камер не обнаружено, то поставит просто в центр текущего GameObject.
        /// </summary>
        /// <param name="switchToIndex">
        ///  Индекс позиции камеры. Если <0 - переключает на следующую. Если больше количества позиций - переключит на последнюю.
        /// </param>
        public void swithCamera( int switchToIndex = -1 ) {

            if (switchToIndex < 0) {
                if (++cameraIndex > cameraPositions.GetLength( 0 ) - 1) {
                    cameraIndex = 0;
                }
            } else {
                cameraIndex = Math.Min( switchToIndex, cameraPositions.GetLength( 0 ) - 1 );
            }

            CameraPositionPoint cp = cameraPositions[cameraIndex].gameObject.GetComponent<CameraPositionPoint>();
            if (cp) {
                _isDamping = cp.isDamped;
                _dampSpeed = cp.dampingSpeed;
            }

        }

        private void initialiseCameras() {
			if (cameraToUse == null) {
                cameraToUse = Camera.main;
                Assert.IsNotNull( cameraToUse, "PlayerController::initialiseCameras > main camera not found!" );
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
	}
}