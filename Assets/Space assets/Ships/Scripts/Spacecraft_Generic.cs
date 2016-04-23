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
        public float        angInertiaDamper    = 1f;   // Эффективность гасителя вращательной инерции (0.1 - 5)
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
			SetupPhysics();

			SetupGUID();
			gameObject.name = shipName + " (" + GUID + ")";
		}

        //void Update() {
			
		//}

		void FixedUpdate() {
            // Getting actual ship direction
            //Rigidbody rb = GetComponent<Rigidbody>();
            //Vector3 actualDirection = rb.velocity.normalized;

            //Vector3 desiredDirection = new Vector3( shipControls[0], shipControls[1], shipControls[2] );
            //desiredDirection.Normalize();
            //Vector3 correctionalDirection = desiredDirection - actualDirection;
            //correctionalDirection.Normalize();
            Vector3 correctionalDirection = Vector3.zero;

            float[] forces = new float[6];
            forces[0] = (shipControls[0] + correctionalDirection.x) * rotationXForce;
            forces[1] = (shipControls[1] + correctionalDirection.y) * rotationYForce;
            forces[2] = (shipControls[2] + correctionalDirection.z) * maxMainEngineForce;
            forces[3] = shipControls[3] * rotationXForce;
            forces[4] = shipControls[4] * rotationYForce;
            forces[5] = shipControls[5] * rotationZForce;




            //TODO: move to EngineFX controller ///////////////////////////////////////////////////////////////
            if (shipControls[2] > 0.1f) {
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

            //TODO: Выпилить все это в отдельный модуль, который будет кешировать все нужные ссылки и в нужные моменты включать нужные эффекты
            if (engineLights) {
                foreach (Light l in engineLights.GetComponentsInChildren<Light>()) {
                    l.intensity = Mathf.Clamp01( forces[2] / 30 );
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////

            GetComponent<Rigidbody>().AddRelativeForce(forces[0], forces[1], forces[2], ForceMode.Force);
			GetComponent<Rigidbody>().AddRelativeTorque(forces[3], forces[4], forces[5], ForceMode.Force);
		}

		public void SetShipControls(float hStrafe, float vStrafe, float forward, float pitch, float yaw, float roll) {
			shipControls[0] = hStrafe;
			shipControls[1] = vStrafe;
			shipControls[2] = forward;
			shipControls[3] = pitch;
			shipControls[4] = yaw;
			shipControls[5] = roll;

        }


        //////////////////////////////////////////////////////////////////////////////////////
        // private

   		protected void SetupPhysics() {
            //setup the rigidbody
            Rigidbody rg = GetComponent<Rigidbody>();
			if (!rg) {
				rg = gameObject.AddComponent<Rigidbody>();
			}

			rg.mass = nominalMass;
			rg.useGravity = false;

            //temporary!
			rg.drag = inertiaDamper;
			rg.angularDrag = angInertiaDamper;
		}


        //FIXME: move it somewhere to GUID-or-somthing manager?
        static int ships;

        protected void SetupGUID() {
            //classType = ClassTypes.ship;
            _guid = "XXXX-SHIP-000" + ++ships;
        }


    }
}