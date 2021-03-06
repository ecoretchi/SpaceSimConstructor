﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Stackables {

    public delegate void DelegateOnSocket(ref List<Socket> ss, Socket s);
    public class ConstructorManager : MonoBehaviour {

        void Start() {
        }
        static void DelegateAll(ref List<Socket> ss, Socket s) { ss.Add(s); }
        public static List<Socket> GetAllSockets() {
            return GetSockets( DelegateAll );
        }
        public static List<Socket> GetSockets(DelegateOnSocket sMatch) {
            MainStationModule m_cms = FindObjectOfType<MainStationModule>();
            //TODO: assert m_cms not null
            List<Socket> res = new List<Socket>();
            GetSockets(ref res, m_cms, sMatch);
            return res;
        }
        static int GetSockets(ref List<Socket> socks, Stackable st, DelegateOnSocket onSocket, Socket jEnter = null) {
            Socket[] ss = st.GetComponentsInChildren<Socket>();
            int res = ss.Length;
            if (res == 0)
                return 0;

            foreach (Socket s in ss) {
                onSocket(ref socks, s);
                if (s.IsConnected() && s.joined != jEnter) {
                    Stackable stNext = s.joined.GetComponentInParent<Stackable>();
                    res += GetSockets(ref socks, stNext, onSocket, s);
                }
            }// foreach
            return res;
        } // int GetSockets 

    }//class ConstructorManager 

}//namespace Stackables 

