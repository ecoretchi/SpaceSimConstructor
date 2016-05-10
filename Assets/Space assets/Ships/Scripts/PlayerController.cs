using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System;
using System.Text;

namespace Spacecraft {

	public class PlayerController : MonoBehaviour {

        [Header("Links")]
		public Camera cameraToUse;
		public ShipUI shipUI;
		public Transform cameraCenter;
        
		private Vector3 cameraVelocity = Vector3.zero;
		private float cameraDistance = 10f;
		public float maxCameraDistance = 30f;
		public float zoomSensitivity = 30f;

		private SpacecraftGeneric  m_ship; // shortcut to master object

        private float       desiredThrottle		= 0;    // current desired speed
        private float       rememberedThrottle	= 0;	// desired speed for "no override mode"
        private bool        mouseShownByAlt     = false;
		

		// Unity callbacks ////////////////////////////////

		void Awake() {
			m_ship = GetComponentInParent<SpacecraftGeneric>();
			Assert.IsNotNull(m_ship, "PlayerController::Awake: No parent SpacecraftGeneric found!");

			GameController.instance.IsCursorLocked = true;
		}	

		void Update() {
			            
            ProcessCruiseEnginesThrottleControl();

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

			float scrollWheel = Input.GetAxis( "Mouse ScrollWheel" );
			if (scrollWheel != 0f) {
				cameraDistance -= scrollWheel * Time.deltaTime * zoomSensitivity;
				cameraDistance = Mathf.Clamp( cameraDistance, 0, maxCameraDistance );
			}

			m_ship.ControlShipMovement( Input.GetAxis( "Horisontal Strafe" ),        // direction x
								   Input.GetAxis( "Vertical Strafe" ),          // direction y
								   desiredThrottle );                                // direction z

			// Direct ship controls
			m_ship.ControlShipRotations( GameController.instance.IsCursorLocked ? Input.GetAxis( "Vertical" ) : 0,           // rotation x
								   GameController.instance.IsCursorLocked ? Input.GetAxis( "Horizontal" ) : 0,               // rotation y
								   Input.GetAxis( "Roll" ) );                    // rotation z

        }

        void LateUpdate() {
			//cameraToUse.transform.position = Vector3.SmoothDamp( cameraToUse.transform.position, cameraCenter.position + transform.TransformDirection( m_ship.CurrentAcceleration ) * -0.01f, ref cameraVelocity, 0.05f * (1.0f - m_ship.CurrentVelocity.sqrMagnitude / m_ship.maxSupportedSpeed.sqrMagnitude ) );
			cameraToUse.transform.position = cameraCenter.position + cameraCenter.rotation *  Vector3.forward * -cameraDistance;
			cameraToUse.transform.rotation = cameraCenter.rotation;
			
            TestCalc();
		}
		
        private void TestCalc() {

            //get the Rigidbody
            Rigidbody rb = GetComponentInParent<Rigidbody>();
            Assert.IsNotNull( rb, "PlayerController::testCalc: no rigidbody>" );
            StringBuilder sb = new StringBuilder();
            sb.Append( "Ship " ).Append( rb.gameObject.name ).AppendLine();
            sb.Append( "Mass " ).Append( rb.mass ).AppendLine();
			sb.Append( "Max speed (m/s): " ).Append( m_ship.maxSupportedSpeed.ToString() ).AppendLine();
			sb.Append( "Cruise engines factor: " ).Append( m_ship.cruiseEnginesFactor.ToString() ).AppendLine();
			sb.Append( "Max acceleration (m/s²): " ).Append( m_ship.maxLinearAcceleration.ToString() ).AppendLine();
            sb.AppendLine( "=======================" );

            sb.Append("Throttle: ").AppendLine(desiredThrottle.ToString());
            sb.Append( "Position: " ).Append( rb.position.ToString() ).Append( " Time: " ).AppendLine( Time.time.ToString() );

            var vel = rb.velocity;
            sb.Append( "Glb Velocity: " ).Append( vel.ToString() ).Append(" magn: ").AppendLine( vel.magnitude.ToString("000.00") );
			var locVel = transform.InverseTransformDirection( vel );
			sb.Append( "Loc Velocity: " ).Append( locVel.ToString() ).Append( " magn: " ).AppendLine( locVel.magnitude.ToString( "000.00" ) );

			sb.Append( "Ang.Velocity: " ).Append( rb.angularVelocity.ToString() ).Append( " magn: " ).AppendLine( rb.angularVelocity.magnitude.ToString( "000.00" ) );
			sb.Append( "RPM:          " ).Append( (transform.InverseTransformDirection( rb.angularVelocity ) * 9.549296586f ).ToString() ).AppendLine();
			
			Vector3 helper = rb.transform.position + rb.transform.up * 3f;
            Debug.DrawLine( helper, helper + rb.transform.forward * 3f, Color.blue );
            Debug.DrawLine( helper, helper + rb.transform.up * 3f, Color.green );
            Debug.DrawLine( helper, helper + rb.transform.right * 3f, Color.red );
			Debug.DrawLine( helper, helper + vel, Color.yellow );

			var fwdSpeed = Vector3.Dot( vel, transform.forward );
            sb.Append( "Forward speed: " ).Append( fwdSpeed.ToString( "0.00" ) );
            sb.AppendLine();

			sb.Append( "Acceleration: " ).Append( m_ship.CurrentAcceleration.ToString() ).AppendLine();
			
			shipUI.description = sb.ToString();
        }

        

        // methods ////////////////////////////////////////////////////////////////////////
		        
        private void ProcessCruiseEnginesThrottleControl() {
            float tempThrottle = desiredThrottle;

            if (tempThrottle == 0 && Input.GetKeyDown( KeyCode.Z )) {
                tempThrottle = -0.1f;
            } else {
                if (Input.GetKey( KeyCode.Z )) {
                    tempThrottle = tempThrottle - 0.6f * Time.deltaTime;
                    if (tempThrottle < -1f) {
                        tempThrottle = -1f;
                    }
                }
            }

            if (tempThrottle == 0 && Input.GetKeyDown( KeyCode.X )) {
                tempThrottle = 0.1f;
            } else {
                if (Input.GetKey( KeyCode.X )) {
                    tempThrottle = tempThrottle + 0.6f * Time.deltaTime;
                    if (tempThrottle > 1f) {
                        tempThrottle = 1f;
                    }
                }
            }

            if (Input.GetButtonDown( "Forward" )) {
                // temporary override throttle control
                rememberedThrottle = tempThrottle;
                tempThrottle = 1f;
            }

            if (Input.GetButtonDown( "Backward" )) {
                // temporary override throttle control
                rememberedThrottle = tempThrottle;
                tempThrottle = -1f;
            }

            if (Input.GetButtonUp( "Forward" )) {
                // return control to speed widget
                tempThrottle = rememberedThrottle;
            }

            if (Input.GetButtonUp( "Backward" )) {
                // return control to speed widget
                tempThrottle = rememberedThrottle;
            }

			if (Input.GetButtonDown( "SetSpeedToZero" )) { 
				desiredThrottle = 0f;
			}

			// update widget only on changes
			if (!Mathf.Approximately( tempThrottle, desiredThrottle )) {
                desiredThrottle = tempThrottle;
                shipUI.speed = desiredThrottle;
            } else {
                // ship speed is controlled by widget
                desiredThrottle = shipUI.speed;
            }
        }
	}
}