using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

//FIXME:  Скорее всего потом переименую пространство...
namespace FlyMode {
    public class ShipTest : MonoBehaviour, ISpaceEntity {

        public Camera cameraToUse;

        // Управление камерой
        public	Transform	cameraPositionsSource;
        private Transform[]	cameraPositions;
        private int			cameraIndex		= 0; //индекс камеры, которую надо использовать
		private bool		_isDamping		= false;
		private float		_dampSpeed		= 10f;

		public	GameObject	engineLights;
		public	Slider		enginePowerSlider;
		public  Text        enginePowerText;
		public	Color		enginePowerSliderPositiveColor = Color.blue;
		public	Color		enginePowerSliderNegativeColor = Color.red;

		public	bool		controlledByPlayer	= false;
        public	string		shipName			= "Test Ship";
        // Текущие характеристики конкретного корабля (не общие для класса, т.е. с учетом всех апгрейдов и т.п.)
        public	float		nominalMass			= 10f;  //Номинальная масса корабля (пустой корабль, тонны)
        public	float		inertiaDamper		= 1f;   //Эффективность гасителя инерции (0.1 - 5)
        public	float		maxMainEngineForce	= 100f; //Максимальная мощность маршевого двигателя, kN (килоньютоны)
        public	float		rotationXForce		= 40f;  //Мощность поворотных двигателей (она же половина мощности стрейфа) (ось X, вертикальная, kN (килоньютоны))
        public	float		rotationYForce		= 30f;  //Мощность поворотных двигателей (она же половина мощности стрейфа) (ось Y, горизонтальная, kN (килоньютоны))
        public	float		rotationZForce		= 20f;  //Мощность поворотных двигателей (ось Z, ось движения, kN (килоньютоны))

				float		oldvelocity			= 0f;
				float		vel0to99			= 0f;
				float		vel0to99temp		= 0f;

        public	float		instantVelocity		= 0f;
        public	float		instantAcceleration	= 0f;
        public	float		maxAcceleration		= 0f;

				float[]		forces				= {0f, 0f, 0f, 0f, 0f, 0f};

        private	string		_guid;

        public string GUID {
            get { return _guid; }
        }

        public string e_name {
            get { return shipName; }
        }

        // Use this for initialization
        void Awake() {

            if ( cameraToUse == null) {
                cameraToUse = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
                if (cameraToUse == null) throw new Exception("No camera!");
            }

            prepareCameraList();
            swithCamera( 0 );

            setupPhysics();
            setupGUID();
            gameObject.name = shipName + " (" + GUID + ")";
        }

		static int ships;
		protected void setupGUID() {
			//classType = ClassTypes.ship;
			_guid = "XXXX-SHIP-000" + ++ships;
		}

		private void prepareCameraList() {
            /*List<Transform> camerasList = new List<Transform>();
            
            foreach (Transform child in cameraPositionsSource) { 
                camerasList.Add(child);
            }

            //Если вдруг список получился пустой - добавим себя
            if(camerasList.Count == 0) {
                camerasList.Add(transform);
            }

            cameraPositions = camerasList.ToArray();
            */
            //А теперь более эффективно:

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

        // Update is called once per frame
        void Update() {

			if (Input.GetKeyUp(KeyCode.Escape)) {
				#if UNITY_EDITOR
					UnityEditor.EditorApplication.isPlaying = false;
				#else
					Application.Quit();
				#endif
			}

			//Подготавливаем силы, действующие на корабль

			//Читаем устройства ввода (либо тут работает AI, управляющий кораблем)
			if (controlledByPlayer && GameController.instance.isCursorLocked) {
                forces[0] = Input.GetAxis("Horisontal Strafe") * rotationXForce;        // direction x
                forces[1] = Input.GetAxis("Vertical Strafe") * rotationYForce;          // direction y
                forces[2] = Input.GetAxis("Speed") * maxMainEngineForce;                // direction z
                forces[3] = Input.GetAxis("Vertical") * rotationXForce;                 // rotation x
                forces[4] = Input.GetAxis("Horizontal") * rotationYForce;               // rotation y
                forces[5] = Input.GetAxis("Roll") * rotationZForce;                     // rotation z
            }

			
            instantVelocity = Mathf.Abs(GetComponent<Rigidbody>().velocity.magnitude);
            instantAcceleration = (instantVelocity - oldvelocity) / Time.deltaTime;
            maxAcceleration = Mathf.Max(Mathf.Abs(instantAcceleration), maxAcceleration);

            oldvelocity = instantVelocity;
            if (instantVelocity > 0f && vel0to99temp > Mathf.NegativeInfinity) {
                //начинаем считать время разгона
                vel0to99temp += Time.deltaTime;
            }
            if (instantVelocity > 99.9f && vel0to99temp > Mathf.NegativeInfinity) {
                vel0to99 = vel0to99temp;
                vel0to99temp = Mathf.NegativeInfinity;
            }

			//float speedFactor = forces[2] / maxMainEngineForce;
			//float speedFactor = instantVelocity / 26f; //some magic numbers :)
			float speedFactor = Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward) / 26f;

			if (enginePowerSlider) {
				enginePowerSlider.value = speedFactor;
				//enginePowerSlider.fillRect.GetComponent<Image>().color = Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward) >= 0 ? enginePowerSliderPositiveColor : enginePowerSliderNegativeColor;
				enginePowerSlider.fillRect.GetComponent<Image>().color = speedFactor >= 0 ? enginePowerSliderPositiveColor : enginePowerSliderNegativeColor;
			}
			if(enginePowerText) {
				enginePowerText.text = Math.Round(speedFactor * 26f).ToString();
				enginePowerText.color = speedFactor >= 0 ? enginePowerSliderPositiveColor : enginePowerSliderNegativeColor;
			}

			//TODO: Выпилить все это в отдельный модуль, который будет кешировать все нужные ссылки и в нужные моменты включать нужные эффекты
			if (engineLights) {
				foreach (Light l in engineLights.GetComponentsInChildren<Light>()) {
					l.intensity = Mathf.Abs(Input.GetAxis("Speed"));
				}
			}

			if (forces[2] > 0.1f) {
				foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>(true)) {
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

			//FIXME: Хм.. где это должно бы быть?.. скорее всего, уйдет в какой-то PlayerPilotingShipController
			if (Input.GetKeyUp(KeyCode.F1)) {
				swithCamera();
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

            if(toSwitchIndex < 0) {
                if( ++cameraIndex > cameraPositions.GetLength(0) - 1) {
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

        void FixedUpdate() {
            //Применяем силы
            GetComponent<Rigidbody>().AddRelativeForce(forces[0], forces[1], forces[2], ForceMode.Force);
            GetComponent<Rigidbody>().AddRelativeTorque(forces[3], forces[4], forces[5], ForceMode.Force);

			if (_isDamping) {
				cameraToUse.transform.position = Vector3.Lerp(cameraToUse.transform.position, cameraPositions[cameraIndex].position, Time.smoothDeltaTime * _dampSpeed);
				cameraToUse.transform.rotation = cameraPositions[cameraIndex].rotation;
			}
		}

		void LateUpdate() {
			if (!_isDamping) {
				cameraToUse.transform.position = cameraPositions[cameraIndex].position;
				cameraToUse.transform.rotation = cameraPositions[cameraIndex].rotation;
			}
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

        void OnTriggerEnter(Collider other) {
            Debug.Log("OnTriggerEnter");
        }

        void OnCollisionEnter(Collision collision) {
            foreach (ContactPoint contact in collision.contacts) {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }

            if (collision.relativeVelocity.magnitude > 2)
                Debug.Log("Strong hit");
        }

		
        /*
	void Awake() {
		//Awake is called when the script instance is being loaded.
	}

	void Start() {
		//Start is called just before any of the Update methods is called the first time.
	}
	void Update() {
		//Update is called every frame, if the MonoBehaviour is enabled.
	}

	void OnMouseOver() {
		//OnMouseOver is called every frame while the mouse is over the GUIElement or Collider.
	}

	void OnMouseUp() {
		//OnMouseUp is called when the user has released the mouse button.
	}

	void OnMouseDrag() {
		//OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.
	}

	void OnBecameVisible() {
		//OnBecameVisible is called when the renderer became visible by any camera.
	}

	void OnBecameInvisible() {
		//OnBecameInvisible is called when the renderer is no longer visible by any camera.
	}

	void OnEnable() {
		//This function is called when the object becomes enabled and active.
	}

	void OnDisable() {
		//This function is called when the behaviour becomes disabled () or inactive.
	}

	void OnDestroy() {
		//This function is called when the MonoBehaviour will be destroyed.
	}

	void OnGUI() {
		//OnGUI is called for rendering and handling GUI events.
	}

	void OnDrawGizmosSelected() {
		//Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected.
	}

	void OnDrawGizmos() {
		//Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
	}

	*/

    }
}