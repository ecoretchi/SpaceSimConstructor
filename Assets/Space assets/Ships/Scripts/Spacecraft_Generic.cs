using System;
using UnityEngine;


namespace Spacecraft {

	/// <summary>
	/// Generic spacecraft class
	/// </summary>
	public class Spacecraft_Generic : MonoBehaviour, ISpaceEntity {


		public  GameObject  engineLights;

		public  bool        controlledByPlayer  = false;
		public  string      shipName            = "Test Ship";
		// Текущие характеристики конкретного корабля (не общие для класса, т.е. с учетом всех апгрейдов и т.п.)

		[Header("Physics")]
		public  float       nominalMass         = 10f;  //Номинальная масса корабля (пустой корабль, тонны)
		public  float       inertiaDamper       = 1f;   //Эффективность гасителя инерции (0.1 - 5)
		public  float       maxMainEngineForce  = 100f; //Максимальная мощность маршевого двигателя, kN (килоньютоны)
		public  float       rotationXForce      = 40f;  //Мощность поворотных двигателей (она же половина мощности стрейфа) (ось X, вертикальная, kN (килоньютоны))
		public  float       rotationYForce      = 30f;  //Мощность поворотных двигателей (она же половина мощности стрейфа) (ось Y, горизонтальная, kN (килоньютоны))
		public  float       rotationZForce      = 20f;  //Мощность поворотных двигателей (ось Z, ось движения, kN (килоньютоны))

		float[]     forces              = {0f, 0f, 0f, 0f, 0f, 0f};

		private string      _guid;

		public string GUID {
			get { return _guid; }
		}

		public string e_name {
			get { return shipName; }
		}

		// Use this for initialization
		void Awake() {

			//prepare cameras
			FindObjectOfType<PlayerController>().initialiseCameras();

			setupPhysics();
			setupGUID();
			gameObject.name = shipName + " (" + GUID + ")";
		}

		static int ships;
		protected void setupGUID() {
			//classType = ClassTypes.ship;
			_guid = "XXXX-SHIP-000" + ++ships;
		}



		// Update is called once per frame
		void Update() {

						
			float speedFactor = Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward) / 26f;
			var localVelocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
			float forwardSpeed = Mathf.Max(0, localVelocity.z);

			
			//TODO: Выпилить все это в отдельный модуль, который будет кешировать все нужные ссылки и в нужные моменты включать нужные эффекты
			if (engineLights) {
				foreach (Light l in engineLights.GetComponentsInChildren<Light>()) {
					l.intensity = Mathf.Abs(Input.GetAxis("Speed"));
				}
			}

			//TODO: перенести в управление эффектами движков
			if (forces[2] > 0.1f) {
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
						
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		}



		void FixedUpdate() {
			// Applying our forces
			GetComponent<Rigidbody>().AddRelativeForce(forces[0], forces[1], forces[2], ForceMode.Force);
			GetComponent<Rigidbody>().AddRelativeTorque(forces[3], forces[4], forces[5], ForceMode.Force);
		}

		public void setShipControls(float hStrafe, float vStrafe, float forward, float pitch, float yaw, float roll) {
			forces[0] = hStrafe;
			forces[1] = vStrafe;
			forces[2] = forward;
			forces[3] = pitch;
			forces[4] = yaw;
			forces[5] = roll;
		}

		protected void setupPhysics() {
			//setup the rigidbody
			if (!GetComponent<Rigidbody>()) {
				gameObject.AddComponent<Rigidbody>();
			}
			GetComponent<Rigidbody>().mass = nominalMass;
			GetComponent<Rigidbody>().useGravity = false;
			GetComponent<Rigidbody>().drag = inertiaDamper;
			GetComponent<Rigidbody>().angularDrag = inertiaDamper * 2f;
		}

	}
}