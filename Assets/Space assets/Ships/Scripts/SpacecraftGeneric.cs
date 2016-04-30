using System;
using UnityEngine;


namespace Spacecraft {

	/// <summary>
	/// Generic spacecraft class
	/// </summary>
	public class SpacecraftGeneric : MonoBehaviour, ISpaceEntity {


        //[SerializeField]
		//protected ShipEngines  engines;

		[Header( "=== Сharacteristics ===" )]
		public string shipName = "Test Ship";
		public float nominalMass = 10f;  // Normal ship mass (w/o cargo, actual mass will be stored in rigidbody)
		public Transform centerOfMass;
		
		[Header( "=== Engines ===" )]

		/// <summary>
		/// Maximum speed in m/s at 3 axis, supported by ship avionics (0 means no capability to strafe in that direction)
		/// </summary>
		public Vector3 maxSupportedSpeed;

		/// <summary>
		/// Maximum acceleration in m/s2, can be achieved by engines at nominal mass
		/// </summary>
		public Vector3 maxLinearAcceleration;

		public Vector3 maxRotateAcceleration;       // maximum maneur engines acceleration (is it needed?)
		public float tempRotationDrag;				// temp: rotational drag

		/// <summary>
		/// Cruise engines power factor (2.0 means 2x powerfull then nose engines)
		/// </summary>
		public float cruiseEnginesFactor;


		protected	Vector3 m_CurrentThrottles;
		/// <summary>
		/// Returns current throttles (in local space)
		/// </summary>
		public		Vector3 CurrentThrottles { get { return m_CurrentThrottles; } }

		protected	Vector3 m_CurrentAcceleration;
		/// <summary>
		/// Returns current acceleration (in local space)
		/// </summary>
		public		Vector3 CurrentAcceleration { get { return m_CurrentAcceleration; } }

		protected Vector3 m_CurrentVelocity;
		public Vector3 CurrentVelocity { get { return m_CurrentVelocity; } }

		protected Rigidbody		m_rigidbody;					// Shortcut for ship's rigidbody
        protected Vector3		desiredShipMovementVelocity;
		protected Vector3		desiredShipRotation;


		protected string _guid;
		public string GUID { get { return _guid; } }
		public string e_name { get { return shipName; } }


		//=================================================================== Methods

		void Awake() {
            //prepare physics
			SetupPhysics();

			SetupGUID();
			gameObject.name = shipName + " (" + GUID + ")";
		}

        //void Update() {
			
		//}

		void FixedUpdate() {
			
			// get current velocity
			m_CurrentVelocity = m_rigidbody.velocity;

			// reduce very low speed to 0
			if (Mathfx.approx( m_CurrentVelocity.x, 0, 0.001f )) {
				m_CurrentVelocity.x = 0;
			}
			if (Mathfx.approx( m_CurrentVelocity.y, 0, 0.001f )) {
				m_CurrentVelocity.y = 0;
			}
			if (Mathfx.approx( m_CurrentVelocity.z, 0, 0.001f )) {
				m_CurrentVelocity.z = 0;
			}
			m_rigidbody.velocity = m_CurrentVelocity;

			// correct course
			Vector3 shipCourse = desiredShipMovementVelocity - transform.InverseTransformDirection( m_CurrentVelocity );
			m_CurrentThrottles.x = Mathf.Clamp( shipCourse.x / maxLinearAcceleration.x, -1f, 1f );
			m_CurrentThrottles.y = Mathf.Clamp( shipCourse.y / maxLinearAcceleration.y, -1f, 1f );
			m_CurrentThrottles.z = Mathf.Clamp( shipCourse.z / (maxLinearAcceleration.z * shipCourse.z > 0 ? cruiseEnginesFactor : 1), -1f, 1f );

			//TODO:  take current mass into account
			m_CurrentAcceleration.x = m_CurrentThrottles.x * maxLinearAcceleration.x;
			m_CurrentAcceleration.y = m_CurrentThrottles.y * maxLinearAcceleration.y;
			m_CurrentAcceleration.z = m_CurrentThrottles.z * maxLinearAcceleration.z * cruiseEnginesFactor;
		   	            			            
			// apply forces
			m_rigidbody.AddRelativeForce( m_CurrentAcceleration, ForceMode.Acceleration );
			m_rigidbody.AddRelativeTorque( desiredShipRotation.x * nominalMass * maxRotateAcceleration.x,
				desiredShipRotation.y * nominalMass * maxRotateAcceleration.y,
				desiredShipRotation.z * nominalMass * maxRotateAcceleration.z,
				ForceMode.Force);
		}

		/*
        private void StabiliseRotation(float stability, float speed) {
			
            Vector3 predictedUp = Quaternion.AngleAxis( m_rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
                                                        m_rigidbody.angularVelocity ) * transform.up;

            Vector3 torqueVector = Vector3.Cross( predictedUp, Vector3.up );
            // Uncomment the next line to stabilize on only 1 axis.
            //torqueVector = Vector3.Project(torqueVector, transform.forward);
            m_rigidbody.AddTorque( torqueVector * speed );
			

        }
		*/

		public void ControlShipMovement( float x, float y, float z ) {
			desiredShipMovementVelocity.x = x * maxSupportedSpeed.x; //TODO: consider mass changes?
			desiredShipMovementVelocity.y = y * maxSupportedSpeed.y;
			desiredShipMovementVelocity.z = (z > 0) ? (z * maxSupportedSpeed.z * cruiseEnginesFactor) : (z * maxSupportedSpeed.z);
		}

		public void ControlShipRotations(Vector3 setRotation ) {
			desiredShipRotation = setRotation;
		}
		public void ControlShipRotations( float x, float y, float z ) {
			desiredShipRotation.x = x;
			desiredShipRotation.y = y;
			desiredShipRotation.z = z;
		}

		//////////////////////////////////////////////////////////////////////////////////////
		// private

		protected void SetupPhysics() {
            //setup the rigidbody
            m_rigidbody = GetComponent<Rigidbody>();
			if (!m_rigidbody) {
				m_rigidbody = gameObject.AddComponent<Rigidbody>();
			}

			m_rigidbody.mass = nominalMass;
            m_rigidbody.useGravity = false;

            m_rigidbody.drag = 0f;
			m_rigidbody.angularDrag = tempRotationDrag;

			if (centerOfMass) {
				m_rigidbody.centerOfMass = centerOfMass.localPosition;
			}
		}


        //FIXME: move it somewhere to GUID-or-something manager?
        static int ships;

        protected void SetupGUID() {
            //classType = ClassTypes.ship;
            _guid = "XXXX-SHIP-000" + ++ships;
        }


    }
}