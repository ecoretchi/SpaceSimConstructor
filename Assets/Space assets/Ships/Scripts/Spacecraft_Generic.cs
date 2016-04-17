using System;
using UnityEngine;


namespace Spacecraft {

	/// <summary>
	/// Generic spacecraft class
	/// </summary>
	public class Spacecraft_Generic : MonoBehaviour, ISpaceEntity {


        [SerializeField]
		private  GameObject  engineLights;

		public  string      shipName            = "Test Ship";
		
		[Header("Physics")]
        
		public  float       nominalMass         = 10f;  // Номинальная масса корабля (пустой корабль, тонны)
		public  float       inertiaDamper       = 1f;   // Эффективность гасителя инерции (0.1 - 5)
		public  float       maxMainEngineForce  = 100f; // Максимальная мощность маршевого двигателя, kN (килоньютоны)
		public  float       rotationXForce      = 40f;  // Мощность поворотных двигателей (она же половина мощности стрейфа) (ось X, вертикальная, kN (килоньютоны))
		public  float       rotationYForce      = 30f;  // Мощность поворотных двигателей (она же половина мощности стрейфа) (ось Y, горизонтальная, kN (килоньютоны))
		public  float       rotationZForce      = 20f;  // Мощность поворотных двигателей (ось Z, ось движения, kN (килоньютоны))

		private float[]     shipControls        = {0f, 0f, 0f, 0f, 0f, 0f};

		private string      _guid;


		public string GUID {
			get { return _guid; }
		}

		public string e_name {
			get { return shipName; }
		}


		void Awake() {
            //prepare physics
			setupPhysics();

			setupGUID();
			gameObject.name = shipName + " (" + GUID + ")";
		}

        void Update() {
			float speedFactor = Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward) / 26f;
			var localVelocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
			float forwardSpeed = Mathf.Max(0, localVelocity.z);
        			
			//TODO: Выпилить все это в отдельный модуль, который будет кешировать все нужные ссылки и в нужные моменты включать нужные эффекты
			if (engineLights) {
				foreach (Light l in engineLights.GetComponentsInChildren<Light>()) {
					l.intensity = shipControls[2];
				}
			}

			//TODO: move to EngineFX controller
			if (shipControls[2] > 0.1f) {
				foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>(true)) {
					ParticleSystem.EmissionModule em = p.emission;
					em.enabled = true;
				}
			} else {
				foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>(true)) {
					ParticleSystem.EmissionModule em = p.emission;
					em.enabled = false;
				}
			}
		}

		void FixedUpdate() {
			// Applying our forces
			GetComponent<Rigidbody>().AddRelativeForce(shipControls[0] * rotationXForce, shipControls[1] * rotationYForce, shipControls[2] * maxMainEngineForce, ForceMode.Force);
			GetComponent<Rigidbody>().AddRelativeTorque(shipControls[3] * rotationXForce, shipControls[4] * rotationYForce, shipControls[5] * rotationZForce, ForceMode.Force);
		}

		public void setShipControls(float hStrafe, float vStrafe, float forward, float pitch, float yaw, float roll) {
			shipControls[0] = hStrafe;
			shipControls[1] = vStrafe;
			shipControls[2] = forward;
			shipControls[3] = pitch;
			shipControls[4] = yaw;
			shipControls[5] = roll;
		}


        //////////////////////////////////////////////////////////////////////////////////////
        // private

   		protected void setupPhysics() {
            //setup the rigidbody
            Rigidbody rg = GetComponent<Rigidbody>();
			if (!rg) {
				rg = gameObject.AddComponent<Rigidbody>();
			}

			rg.mass = nominalMass;
			rg.useGravity = false;

            //temporary!
			rg.drag = inertiaDamper;
			rg.angularDrag = inertiaDamper * 2f;
		}


        //FIXME: move it somewhere to GUID-or-somthing manager?
        static int ships;

        protected void setupGUID() {
            //classType = ClassTypes.ship;
            _guid = "XXXX-SHIP-000" + ++ships;
        }


    }
}