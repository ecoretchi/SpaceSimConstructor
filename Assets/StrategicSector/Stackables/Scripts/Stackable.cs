using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Stackables {
    public class Stackable : MonoBehaviour {

        protected Socket m_hitSocket = null;
        private int m_curCompatibleNum = 0;
        private List<Socket> m_compatibleSockets = new List<Socket>();
        private Socket m_lastCompatibleSocket = null;
        protected RaycastHit m_currentHit;
        public bool IsConvergence() {
            return m_hitSocket;
        }
        int debugSockNum =0 ;
        void Update() { 
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (m_hitSocket) {
                    GetNextCompatibleSocket(m_hitSocket);
                }
                else {                    
                    //Socket[] ss = this.GetComponentsInChildren<Socket>();
                    //++debugSockNum;
                    //if(debugSockNum>=ss.Length)
                    //   debugSockNum = 0;
                    //Socket s = ss[debugSockNum];
                    //AdaptMeshToSocket(s);
                }
            }
        }
        virtual public void OnMoveOverConstructon(RaycastHit hit) {
            m_currentHit = hit;
            Socket hitSock = hit.collider.gameObject.GetComponent<Socket>();
            if (hitSock) {
                if(OnConvergence(hitSock))
                    m_hitSocket = hitSock;
            }
            else
                OnDivergence();            
        }
        virtual public bool OnConvergence(Socket hitSock) {

            if (CheckCollision(hitSock)) {
                hitSock.OnDisable();
            } else {
                hitSock.OnStick(m_lastCompatibleSocket);
                m_lastCompatibleSocket.OnStick(hitSock);
            }
            return true; 
        }
        virtual public bool OnDivergence() {
            if (m_hitSocket)
                m_hitSocket.OnRelease();
            if (m_lastCompatibleSocket)
                m_lastCompatibleSocket.OnDisable();

            transform.position = m_currentHit.point;//moveTarget
            alignForwardLerp(-m_currentHit.normal);
            m_hitSocket = null;
            return true;
        }
        //
        // Summary:
        //     user release the dragged object during convergence
        //      all sockets will switch to enable state except sticked,
        //      sticked socket will switch to connected state
        //
        virtual public void OnConnect() {
            Socket[] ss = this.GetComponentsInChildren<Socket>();
            foreach (Socket s in ss) {
                if(s.IsDisabled())
                    s.OnRelease();
                else
                    s.OnConnect();
            }
            if (m_hitSocket)
                m_hitSocket.OnConnect();
            m_hitSocket = null;
        }
        virtual public void OnDisconnect() {
            Socket[] ss = this.GetComponentsInChildren<Socket>();
            foreach (Socket s in ss) {
                s.OnDisable();
            }
            if (m_hitSocket)
                m_hitSocket.OnRelease();
        }
        List<Socket> GetCompatibleSockets(Socket s) {
            m_compatibleSockets.Clear();
            Socket[] ss = this.GetComponentsInChildren<Socket>();
            if (ss.Length == 0)
                return m_compatibleSockets;
            foreach (Socket curS in ss) {
                if (curS.IsCompatible(s))
                    m_compatibleSockets.Add(curS);
            }
            return m_compatibleSockets;
        }
        void SetCompatibleSocket(int n = -1) {
            if(n>-1)
                m_curCompatibleNum = n;
            if (m_curCompatibleNum >= m_compatibleSockets.Count)
                m_curCompatibleNum = 0;
            if (m_lastCompatibleSocket)
                m_lastCompatibleSocket.OnDisable();
            m_lastCompatibleSocket = m_compatibleSockets[m_curCompatibleNum];
        }
        public Socket GetCompatibleSocket(Socket s) {
            if (!m_lastCompatibleSocket || !m_lastCompatibleSocket.IsCompatible(s)) {
                if (GetCompatibleSockets(s).Count == 0)
                    return null;
                SetCompatibleSocket();
                AdaptMeshToSocket(m_lastCompatibleSocket);
            }
            return m_lastCompatibleSocket;
        }
        public Socket GetNextCompatibleSocket(Socket s) {
            if (!m_lastCompatibleSocket || !m_lastCompatibleSocket.IsCompatible(s) || m_compatibleSockets.Count == 0) {
                if (GetCompatibleSockets(s).Count == 0)
                    return null;
                
                SetCompatibleSocket(0);
                AdaptMeshToSocket(m_lastCompatibleSocket);
                return m_lastCompatibleSocket;
            }            
            SetCompatibleSocket(m_curCompatibleNum+1);
            AdaptMeshToSocket(m_lastCompatibleSocket);
            return m_lastCompatibleSocket;
        }
        public void alignToNormal(Vector3 normal, Transform target, Vector3 targetNormal) {
            Quaternion rotate = Quaternion.FromToRotation(targetNormal, normal);
            target.rotation = rotate * target.rotation;
        }
        public void alignToNormalLerp(Vector3 normal, Transform target, Vector3 targetNormal, float factor = 0.2f) {
            Quaternion rotate = Quaternion.FromToRotation(targetNormal, normal);
            target.rotation = Quaternion.Slerp(target.rotation, rotate * target.rotation, factor);
        }
        public void alignUp(Vector3 normal, Transform target = null) {
            if (target == null)
                target = gameObject.transform;
            alignToNormal(normal, target, target.up);
        }
        public void alignForward(Vector3 normal, Transform target = null) {
            if (target == null)
                target = gameObject.transform;
            alignToNormal(normal, target, target.forward);
        }
        public void alignRight(Vector3 normal, Transform target = null) {
            if (target == null)
                target = gameObject.transform;
            alignToNormal(normal, target, target.right);
        }

        public void alignForwardLerp(Vector3 normal, Transform target = null) {
            if (target == null)
                target = gameObject.transform;
            alignToNormalLerp(normal, target, target.forward);
        }
        void AdaptMeshToSocket(Socket s) {
            //adapt socket position with mesh to current transform position
            MeshFilter mf = GetComponentInChildren<MeshFilter>();
            if (mf == null)
                return;
            Transform tMesh = mf.gameObject.transform;
            Transform tSock = s.gameObject.transform;
            //tMesh.transform.localRotation = tSock.localRotation;// * tMesh.transform.rotation;
            //alignForward(tMesh.transform, tSock.forward);            
            tMesh.localRotation = Quaternion.Inverse(tSock.transform.localRotation);
            tMesh.position = tMesh.position - tSock.position + gameObject.transform.position;
            //transform.rotation = tSock.transform.rotation;
        }
        bool CheckCollision(Socket hitSock) {

            return false;
        }
    }

}// namespace Stackables