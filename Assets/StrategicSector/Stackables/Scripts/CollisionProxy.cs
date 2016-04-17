using UnityEngine;
using System.Collections;

namespace Stackables {
    public class CollisionProxy : MonoBehaviour {

        Stackable m_stackable;

        void Awake() {
            m_stackable = GetComponentInParent<Stackable>();
        }

        void OnTriggerEnter(Collider other) {
            if (m_stackable && other.GetComponent<Socket>() == null)
                m_stackable.OnTriggerEnter(other);
        }
        void OnTriggerExit(Collider other) {

            if (m_stackable && other.GetComponent<Socket>() == null)
                m_stackable.OnTriggerExit(other);
        }
    }

}