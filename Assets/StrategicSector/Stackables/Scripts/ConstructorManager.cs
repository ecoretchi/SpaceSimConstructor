using System;
using System.Collections;

namespace Stackables {

	delegate bool SocketMatch(Socket s);
	public class ConstructorManager : MonoBehaviour {

		void Start ()
		{
		}
		static bool allMatch (Socket s) { return true; }
		public static List<Socket> GetAllSockets (){ 
			return GetSockets(allMatch);
		}
		public static List<Socket> GetSockets (SocketMatch sMatch){
			MainStationModule m_cms = FindObjectOfType<MainStationModule> ();
			//TODO: assert m_cms not null
			List<Socket> res = new List<Socket> ();
			GetSockets(res, m_cms, sMatch);
			return res;
		}

		static int GetSockets (ref List<Socket> socks, Stackable st, SocketMatch sMatch, Socket jEnter=null){
			Socket[] ss = st.GetComponentsInChildren<Socket>();
            if (ss.Length == 0)
                return 0;
            foreach (Socket s in ss) {
				if(sMatch(s))
					socks.Add(s);
				if(s.IsConnected() && s.joined != jEnter ){
					Stackable stNext = s.joined.GetComponentInParents<Stackable>();
					GetSockets(socks, stNext, sMatch, s);
				}
            }// foreach
		} // int GetSockets 

	}//class ConstructorManager 

}//namespace Stackables 

