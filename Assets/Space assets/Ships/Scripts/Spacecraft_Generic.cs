using System;
using UnityEngine;


namespace Spacecraft {

	/// <summary>
	/// Generic spacecraft class
	/// </summary>
	public class Spacecraft_Generic : MonoBehaviour, ISpaceEntity {


        [SerializeField]
		private  GameObject  engineLights;

		[Header( "=== Сharacteristics ===" )]
		public string shipName = "Test Ship";
		public float nominalMass = 10f;  // Normal ship mass (w/o cargo, actual mass will be stored in rigidbody)
		public Transform centerOfMass;
		
		[Header( "=== Engines ===" )]

		//temp - will be calculated from max accelerations
		//public float maxMainEngineForce = 100f; // Максимальная мощность маршевого двигателя, kN (килоньютоны, если масса в тоннах)
		//public float rotationXForce = 40f;  // Мощность поворотных двигателей (она же половина мощности стрейфа) (ось X, вертикальная, kN (килоньютоны))
		//public float rotationYForce = 30f;  // Мощность поворотных двигателей (она же половина мощности стрейфа) (ось Y, горизонтальная, kN (килоньютоны))
		//public float rotationZForce = 20f;  // Мощность поворотных двигателей (ось Z, ось движения, kN (килоньютоны))

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


        private Rigidbody   m_rigidbody;                // Shortcut for ship's rigidbody
        private Vector3     desiredShipMovementVelocity;   // Direction and engines power (0,1), set by ship controls (linear forces)
		private Vector3		desiredShipRotation;		// Desired ship rotation and engines power (0,1), set by controls (torques along axes)


		private string      _guid;


		public string GUID {
			get { return _guid; }
		}

		public string e_name {
			get { return shipName; }
		}


		void Awake() {
            //prepare physics
			SetupPhysics();

			SetupGUID();
			gameObject.name = shipName + " (" + GUID + ")";
		}

        //void Update() {
			
		//}

		void FixedUpdate() {
			
			var currentVelocity = m_rigidbody.velocity;
			Vector3 moveForce = desiredShipMovementVelocity - transform.InverseTransformDirection( currentVelocity );

			//TODO: Выпилить все это в отдельный модуль, который будет кешировать все нужные ссылки и в нужные моменты включать нужные эффекты
			if (engineLights) {
				foreach (Light l in engineLights.GetComponentsInChildren<Light>()) {
					l.intensity = Mathf.Clamp01( moveForce.z / (maxLinearAcceleration.z * moveForce.z > 0 ? cruiseEnginesFactor : 1) );
				}
			}

			//TODO: move to EngineFX controller ///////////////////////////////////////////////////////////////
			if (moveForce.z > 0.1f) {
				foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>( true )) {
					ParticleSystem.EmissionModule em = p.emission;
					em.enabled = true;
				}
			} else {
				foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>( true )) {
					ParticleSystem.EmissionModule em = p.emission;
					em.enabled = false;
				}
			}
			//////////////////////////////////////////////////////////////////////////////////////////////////


			moveForce.x = Mathf.Clamp( moveForce.x / maxLinearAcceleration.x, -1f, 1f ) * maxLinearAcceleration.x * m_rigidbody.mass;
			moveForce.y = Mathf.Clamp( moveForce.y / maxLinearAcceleration.y, -1f, 1f ) * maxLinearAcceleration.y * m_rigidbody.mass;
			moveForce.z = Mathf.Clamp( moveForce.z / (maxLinearAcceleration.z * moveForce.z > 0 ? cruiseEnginesFactor : 1), -1f, 1f ) * maxLinearAcceleration.z * m_rigidbody.mass * cruiseEnginesFactor;
		   	            			            
			m_rigidbody.AddRelativeForce( moveForce, ForceMode.Force );
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
			desiredShipMovementVelocity.x = x * maxSupportedSpeed.x; //TODO: consider mass changes
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

            //temporary!
            m_rigidbody.drag = 0f;
			m_rigidbody.angularDrag = tempRotationDrag;

			if (centerOfMass) {
				m_rigidbody.centerOfMass = centerOfMass.localPosition;
			}
		}


        //FIXME: move it somewhere to GUID-or-somthing manager?
        static int ships;

        protected void SetupGUID() {
            //classType = ClassTypes.ship;
            _guid = "XXXX-SHIP-000" + ++ships;
        }


    }
}