using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Stackables {

    public delegate bool DelegateSocketMatch(Socket s);
    public class ConstructorManager : MonoBehaviour {

        void Start() {
        }
        static bool DelegateAllMatch(Socket s) { return true; }
        public static List<Socket> GetAllSockets() {
            return GetSockets(DelegateAllMatch);
        }
        public static List<Socket> GetSockets(DelegateSocketMatch sMatch) {
            MainStationModule m_cms = FindObjectOfType<MainStationModule>();
            //TODO: assert m_cms not null
            List<Socket> res = new List<Socket>();
            GetSockets(ref res, m_cms, sMatch);
            return res;
        }

        static int GetSockets(ref List<Socket> socks, Stackable st, DelegateSocketMatch sMatch, Socket jEnter = null) {
            Socket[] ss = st.GetComponentsInChildren<Socket>();
            int res = ss.Length;
            if (res == 0)
                return 0;

            foreach (Socket s in ss) {
                if (sMatch(s))
                    socks.Add(s);
                if (s.IsConnected() && s.joined != jEnter) {
                    Stackable stNext = s.joined.GetComponentInParent<Stackable>();
                    res += GetSockets(ref socks, stNext, sMatch, s);
                }
            }// foreach

            return res;
        } // int GetSockets 

    }//class ConstructorManager 

}//namespace Stackables 

