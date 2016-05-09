using UnityEngine;
using System.Collections;

public class MouseHoverInspector : MonoBehaviour {


    public Camera currCamera { get; set; }

    Transform tmpHitTransform = null;
    protected RaycastHit hitInfo;

    [Header("MouseHoverInspector")]

    public bool through_hit = true;
    public bool gybrit_through_hit = true;
    protected string hitTag { get; set; }

    // ================ pure INTERFACES ===================
    virtual public void OnUpdate() { }
    virtual public void OnStart() { }
    virtual public void OnTargetHitEnter(Transform target) { }
    virtual public void OnTargetHit(Transform target) { }
    virtual public void OnTargetHitRelease(Transform target) { }

    //==================  AWAKE  ==================
    void Awake() {
        hitInfo = new RaycastHit();
    }
    //==================  START  ==================
    void Start() {
        print("MouseHoverInspector");
        if (!currCamera)
            currCamera = Camera.main;
        OnStart();
    }

    //==================  UPDATE  ==================
    protected void Update() {

        Transform hitTransform;
        if (GetHitTransform(out hitTransform)) {
            if (!tmpHitTransform) {
                OnTargetHitEnter(hitTransform);
                tmpHitTransform = hitTransform;
            }
            OnTargetHit(hitTransform);

        } else if (tmpHitTransform) {
            OnTargetHitRelease(tmpHitTransform);
            tmpHitTransform = null;
        }

        OnUpdate();
    }

    bool GetHitTransform(out Transform t) {
        t = null;
        if (!currCamera) {
            return false;        
        }

        int mask = GetHitTransformMask();
        Ray ray = currCamera.ScreenPointToRay(Input.mousePosition);
        if (!through_hit) {
            bool res = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask);
            if (res) {
                t = hitInfo.transform;
                if (OnCheckTagName(t.tag))
                    return true;
            }
            if (!gybrit_through_hit)
                return false;
        }
                
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, mask);
        for (int i = 0; i < hits.Length; ++i) {
            hitInfo = hits[i];
            t = hitInfo.transform;
            if (OnCheckTagName(t.tag))
                return true;
        }
        return false;
    }
    virtual public int GetHitTransformMask() {
        return 1 << LayerMask.NameToLayer("Construction"); ;
    }

    virtual public bool OnCheckTagName(string tagName) {
        return tagName == hitTag;        
    }
}
