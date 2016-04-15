using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class Stackable : MonoBehaviour {

    protected Socket m_hitSocket = null;
    private int m_curCompatibleNum = 0;
    private List<Socket> m_compatibleSockets = new List<Socket>();
    private Socket m_lastCompatibleSocket = null;
    protected RaycastHit m_currentHit;
    public bool IsConvergence() {
        return m_hitSocket;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (m_hitSocket)
                GetNextCompatibleSocket(m_hitSocket);
        }
    }

    virtual public void OnMoveOverConstructon(RaycastHit hit) {
        m_currentHit = hit;
        m_hitSocket = hit.collider.gameObject.GetComponent<Socket>();
        if (m_hitSocket) {
            if (!OnConvergence(m_hitSocket))
                m_hitSocket = null;
        }
        else
            OnDivergence();
    }

    virtual public bool OnConvergence(Socket s) { return false;  }
    virtual public bool OnDivergence() { return false; }

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

    public Socket GetCompatibleSocket(Socket s) {
        if (!m_lastCompatibleSocket || !m_lastCompatibleSocket.IsCompatible(s) ) {
			if (GetCompatibleSockets(s).Count == 0)
				return null;
			m_lastCompatibleSocket = m_compatibleSockets[m_curCompatibleNum];
            AdaptMeshToSocket(m_lastCompatibleSocket);
        }
        return m_lastCompatibleSocket;
    }

    public Socket GetNextCompatibleSocket(Socket s) {
		if (!m_lastCompatibleSocket || !m_lastCompatibleSocket.IsCompatible(s) || m_compatibleSockets.Count == 0 ) {
            if (GetCompatibleSockets(s).Count == 0)
                return null;
            m_curCompatibleNum = 0;            
			m_lastCompatibleSocket = m_compatibleSockets[0];
            AdaptMeshToSocket(m_lastCompatibleSocket);
            return m_lastCompatibleSocket;
        }
        ++m_curCompatibleNum;
        if (m_curCompatibleNum >= m_compatibleSockets.Count)
            m_curCompatibleNum = 0;
        m_lastCompatibleSocket = m_compatibleSockets[m_curCompatibleNum];
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

	public void alignForwardLerp(Transform target, Vector3 normal) {
		alignToNormalLerp (normal, target, target.forward);
    }

    void AdaptMeshToSocket(Socket s) {
        //adapt socket position with mesh to curent transform position
        MeshFilter mf = GetComponentInChildren<MeshFilter>();
        if (mf == null)
            return;
        Transform tMesh = mf.gameObject.transform;
        Transform tSock = s.gameObject.transform;
		//tMesh.transform.localRotation = tSock.localRotation;// * tMesh.transform.rotation;
		//alignForward(tMesh.transform, tSock.forward);
		tMesh.localRotation = tSock.transform.localRotation;
        tMesh.position = tMesh.position - tSock.position + gameObject.transform.position;
		//transform.rotation = tSock.transform.rotation;
    }
}