using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Stackables {
    public class Stackable : MonoBehaviour {

        protected Socket m_hitSocket = null;
        private int m_curCompatibleNum = 0;
        private List<Socket> m_sockets;
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
        CollisionInfo m_collisionInfo;
        virtual public Transform GetDraggingTransform() { return null; }
        virtual public void OnCaptured() {
            Socket[] ss = this.GetComponentsInChildren<Socket>();
            foreach (Socket s in ss) {
                s.OnDisable();
            }
            if (m_hitSocket)
                m_hitSocket.OnRelease();

            AdaptMeshToPoint(GetDraggingTransform());
        }
        virtual public void OnMoveOverConstructon(RaycastHit hit) {
            m_currentHit = hit;
            Socket hitSock = hit.collider.gameObject.GetComponent<Socket>();
            if (m_collisionInfo == null)
                m_collisionInfo = new CollisionInfo(false, hitSock, m_lastCompatibleSocket);
            if (hitSock) {
                if (m_collisionInfo.IsCollision(hitSock, m_lastCompatibleSocket) == false)
                    OnConvergence(hitSock);//could be sticked to socket                    
            }
            else
                OnDivergence();

            if (!m_hitSocket || m_collisionInfo.IsCollision(hitSock, m_lastCompatibleSocket)) {
                //DoMoveOverConstruction
                transform.position = m_currentHit.point;//moveTarget
                alignForwardLerp(-m_currentHit.normal);
            }
            
        }        
        virtual public bool OnConvergence(Socket hitSock) {

            m_collisionInfo.m_state = CheckCollision(hitSock, this);
            if (m_collisionInfo.m_state) {
                OnDivergence();
            } else {
                //Do set Stick states
                hitSock.OnStick(m_lastCompatibleSocket);
                m_lastCompatibleSocket.OnStick(hitSock);
            }
            m_hitSocket = hitSock;
            return true; 
        }
        virtual public bool OnDivergence() {
            if (m_hitSocket)
                m_hitSocket.OnRelease();
            if (m_lastCompatibleSocket)
                m_lastCompatibleSocket.OnDisable();

            if (m_collisionInfo != null) {
                m_collisionInfo.m_mother = m_hitSocket;
                m_collisionInfo.m_father = m_lastCompatibleSocket;
            }
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
        public List<Socket> GetSockets() {         
            if(m_sockets != null)
                return m_sockets;
            m_sockets = new List<Socket>();
            Socket[] ss = this.GetComponentsInChildren<Socket>();
            if (ss.Length == 0)
                return m_sockets;
            foreach (Socket s in ss) {
                m_sockets.Add(s);
            }
            return m_sockets;
        }
        List<Socket> GetCompatibleSockets(Socket s) {
            m_compatibleSockets.Clear();
            GetSockets();
            foreach (Socket curS in m_sockets) {
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
                AdaptMeshToPoint(m_lastCompatibleSocket);
            }
            return m_lastCompatibleSocket;
        }
        public Socket GetNextCompatibleSocket(Socket s) {
            if (!m_lastCompatibleSocket || !m_lastCompatibleSocket.IsCompatible(s) || m_compatibleSockets.Count == 0) {
                if (GetCompatibleSockets(s).Count == 0)
                    return null;
                
                SetCompatibleSocket(0);
                AdaptMeshToPoint(m_lastCompatibleSocket);
                return m_lastCompatibleSocket;
            }            
            SetCompatibleSocket(m_curCompatibleNum+1);
            AdaptMeshToPoint(m_lastCompatibleSocket);
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
        void AdaptMeshToPoint(Socket s) {
            AdaptMeshToPoint(s.gameObject.transform);
        }
        void AdaptMeshToPoint(Transform tPoint) {
            if (tPoint == null)
                return;
            //adapt socket position with mesh to current transform position
            MeshFilter mf = GetComponentInChildren<MeshFilter>();
            if (mf == null)
                return;
            Transform tMesh = mf.gameObject.transform;
            Transform tSock = tPoint;
            //tMesh.transform.localRotation = tSock.localRotation;// * tMesh.transform.rotation;
            //alignForward(tMesh.transform, tSock.forward);            
            tMesh.localRotation = Quaternion.Inverse(tSock.transform.localRotation);
            tMesh.position = tMesh.position - tSock.position + gameObject.transform.position;
            //transform.rotation = tSock.transform.rotation;
        }
        bool CheckCollision(Socket enterSocket, Stackable orign, int curRecursionDeep = 0) {
            Stackable nextSt = enterSocket.GetComponentInParents<Stackable>();
            if (curRecursionDeep > 0) {
                if (CheckCollision(orign, nextSt))
                    return true;
            }
            List<Socket> sockets = nextSt.GetSockets();
            foreach (Socket s in sockets) {
                if (s == enterSocket) //avoid regression
                    continue;
                if (s.IsConnected() && s.joined) {
                    if (CheckCollision(s.joined, orign, curRecursionDeep + 1))
                        return true;
                }
            }
            return false;
        }
        public bool CheckCollision(Stackable obj1, Stackable obj2) {

            MeshFilter mf1 = obj1.GetComponentInChildren<MeshFilter>();
            MeshFilter mf2 = obj2.GetComponentInChildren<MeshFilter>();

            Renderer r1 = mf1.GetComponentInChildren<Renderer>();
            Renderer r2 = mf2.GetComponentInChildren<Renderer>();

            Bounds bounds1 = r1.bounds;
            Bounds bounds2 = r2.bounds;


            ShowMeshBounds.DrawBounds( bounds1, Color.green);
            ShowMeshBounds.DrawBounds( bounds2, Color.green);

            if (bounds1.Intersects(bounds2)) {

                Collider[] cols1 = r1.gameObject.GetComponents<Collider>();
                Collider[] cols2 = r2.gameObject.GetComponents<Collider>();
                foreach (Collider c1 in cols1) {
                    if (obj2.m_collisions != null && obj2.m_collisions.Exists(x => x == c1)) {
                        return true;
                    }
                }
                foreach (Collider c2 in cols2) {
                    if (obj1.m_collisions != null && obj1.m_collisions.Exists(x => x == c2)) {
                        return true;
                    }
                }
               
            }

           
            return false;
        }

        public List<Collider> m_collisions {get;protected set;}
        public void OnTriggerEnter(Collider other) {
            if (m_collisions == null)
                m_collisions = new List<Collider>() ;
            print("OnTriggerEnter:" + this + " with " + other);

            if (!m_collisions.Exists(x => x == other))
                m_collisions.Add(other);

        }
        public void OnTriggerExit(Collider other) {

            print("OnTriggerExit:" + this + " with " + other);

            m_collisions.Remove(other);
        }
    }

}// namespace Stackables