using UnityEngine;
using System.Collections;

namespace Stackables {
    public class CollisionInfo {

        public bool m_state { get; set; }
        public Socket m_mother { get; set; }
        public Socket m_father { get; set; }

        public CollisionInfo(bool state, Socket mother, Socket father) {
            m_state = state;
            m_mother = mother;
            m_father = father;
        }
        public bool IsCollision(Socket mother, Socket father) {
            return m_state && m_mother == mother && m_father == father;
        }
    }

}