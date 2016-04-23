﻿using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System;
using System.Text;

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

		private Spacecraft_Generic  _ship; // shortcut to master object

        private float       desiredSpeed        = 0;    // current desired speed
        private float       rememberedSpeed     = 0; // desired speed for "no override mode"
        private bool        mouseShownByAlt     = false;


        // Unity callbacks ////////////////////////////////

        void Start() {
			_ship = GetComponentInParent<Spacecraft_Generic>();
			Assert.IsNotNull(_ship, "PlayerController::Start: No parent Spacecraft_Generic found!");

            InitialiseCameras();
		}	

		void Update() {

            if (Input.GetKeyUp( KeyCode.F1 )) {
                SwithCamera();
            }

            ProcessSpeedControl();

            if (!mouseShownByAlt && Input.GetKeyDown( KeyCode.LeftAlt )) {
                GameController.instance.IsCursorLocked = false;
                mouseShownByAlt = true;
            }

            // на всякий случай, будем следить не за getkeyup, а за не нажатым состоянием - по идее, после альт-таба может так быть
            if (mouseShownByAlt && GameController.instance.IsNoCursorMode && !GameController.instance.IsCursorLocked && !Input.GetKey( KeyCode.LeftAlt )) {
                GameController.instance.IsCursorLocked = true;
                mouseShownByAlt = false;
            }

            if (Input.GetKeyDown( KeyCode.Space )) {
                GameController.instance.IsCursorLocked = !GameController.instance.IsCursorLocked;
            }

            _ship.SetShipControls( Input.GetAxis( "Horisontal Strafe" ),        // direction x
                                   Input.GetAxis( "Vertical Strafe" ),          // direction y
                                   desiredSpeed,                                // direction z
                                   GameController.instance.IsCursorLocked ? Input.GetAxis( "Vertical" ) : 0,                 // rotation x
                                   GameController.instance.IsCursorLocked ? Input.GetAxis( "Horizontal" ) : 0,               // rotation y
                                   Input.GetAxis( "Roll" ) );                    // rotation z
            

        }

        void FixedUpdate() {
			// если это перенести в LateUpdate, то камера адово дергается
			if (_isDamping) {
				cameraToUse.transform.position = Vector3.Lerp(cameraToUse.transform.position, cameraPositions[cameraIndex].position, Time.smoothDeltaTime * _dampSpeed);
				cameraToUse.transform.rotation = cameraPositions[cameraIndex].rotation;
			}

            TestCalc();
		}

		void LateUpdate() {
			// а если это перенести в FixedUpdate, то камера "плавает", как будто пружинящая (видимо, позиции трансформов еще не обновляются к тому моменту)
			if (!_isDamping) {
				cameraToUse.transform.position = cameraPositions[cameraIndex].position;
				cameraToUse.transform.rotation = cameraPositions[cameraIndex].rotation;
			}

            //TestCalc();
		}

        private void TestCalc() {

            //get the Rigidbody
            Rigidbody rb = GetComponentInParent<Rigidbody>();
            Assert.IsNotNull( rb, "PlayerController::testCalc: no rigidbody>" );
            StringBuilder sb = new StringBuilder();
            sb.Append( "Ship " ).Append( rb.gameObject.name ).AppendLine();
            sb.Append( "Mass " ).Append( rb.mass ).AppendLine();
            sb.AppendLine( "=======================" );

            sb.Append("speed control: ").AppendLine(desiredSpeed.ToString());
            sb.Append( "Position: " ).Append( rb.position.ToString() ).Append( " Time: " ).AppendLine( Time.time.ToString() );

            var vel = rb.velocity;
            sb.Append( "Velocity: " ).Append( vel.ToString() ).Append(" magn: ").Append( vel.magnitude.ToString("000.00") );
            //sb.AppendLine( "\n" );
            sb.AppendLine();

            Vector3 helper = rb.transform.position + rb.transform.up * 3f;
            Debug.DrawLine( helper, helper + vel, Color.yellow );
            Debug.DrawLine( helper, helper + rb.transform.forward * 3f, Color.blue );
            Debug.DrawLine( helper, helper + rb.transform.up * 3f, Color.green );
            Debug.DrawLine( helper, helper + rb.transform.right * 3f, Color.red );

            var fwdSpeed = Vector3.Dot( vel, transform.forward );
            sb.Append( "Forward speed: " ).Append( fwdSpeed.ToString( "0.00" ) );
            sb.AppendLine();

            Vector3 instantAcceleration = Vector3.zero;
            if(Math3d.LinearAcceleration(out instantAcceleration, rb.position, 15, true )) {
                sb.Append( "Acceleration: " ).Append( instantAcceleration.ToString() ).Append(" magn:").Append(instantAcceleration.magnitude.ToString("000.00"));
                sb.AppendLine();
                sb.Append( "F = " ).Append( (instantAcceleration.magnitude * rb.mass * Time.fixedDeltaTime).ToString( "000.00" ) );
            }


            shipUI.description = sb.ToString();
        }

        

        // methods ////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///  "Переключает" камеру на одну из позиций из списка GameObject'ов
        ///  Если позиций камер не обнаружено, то поставит просто в центр текущего GameObject.
        /// </summary>
        /// <param name="switchToIndex">
        ///  Индекс позиции камеры. Если <0 - переключает на следующую. Если больше количества позиций - переключит на последнюю.
        /// </param>
        public void SwithCamera( int switchToIndex = -1 ) {

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

        private void ProcessSpeedControl() {
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
                // ship speed is controlled by widget
                desiredSpeed = shipUI.speed;
            }
        }

        private void InitialiseCameras() {
			if (cameraToUse == null) {
                cameraToUse = Camera.main;
                Assert.IsNotNull( cameraToUse, "PlayerController::initialiseCameras: main camera not found!" );
			}

			PrepareCameraList();
			SwithCamera(0);
		}

		private void PrepareCameraList() {
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