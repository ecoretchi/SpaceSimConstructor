using UnityEngine;
using System.Collections;

public class ModuleInfoPopupMenu : MouseHoverInspector {

    StrategicCamera strategicCamera;

    Canvas canvasUI;
    Transform curTarget;
    Transform fadeTarget;
    
    Vector3 pos;
    public float popupSpeed = 10;
    public float popupRotateSpeed = 5;
    public float fadeScaleSpeed = 1;
    
    override public bool OnCheckTagName(string tagName) {
        if (tagName == "Construction")
            return true;
        if (tagName == "Stackable")
            return true;
        return false;
    }
    override public void OnUpdate() {
    }
    override public void OnStart() {
        strategicCamera = (StrategicCamera)GameObject.FindObjectOfType(typeof(StrategicCamera));

        canvasUI = gameObject.GetComponentInChildren<Canvas>();
        Vector3 pos = canvasUI.transform.localPosition;
        RectTransform rt = canvasUI.GetComponent<RectTransform>();
        //float f = Screen.width / rt.sizeDelta.x;

        //pos.x = (Screen.width - rt.sizeDelta.x)/4f;
        canvasUI.transform.localPosition = pos; 
    }
    override public void OnTargetHitEnter(Transform target) {
        curTarget = target;
        gameObject.transform.position = target.position;
        
        //float r =  (currCamera.transform.position - target.position).magnitude * 0.3f;
        pos = currCamera.transform.position + currCamera.transform.forward * 50;// -currCamera.transform.right * Screen.width / 10;s
        
        adjustUIPositionToCam(target);
        canvasUI.gameObject.SetActive(true);
    }
    override public void OnTargetHit(Transform target) {
        if (curTarget != target) {
            fadeTarget = curTarget;
        }
        curTarget = target;
        adjustUIPositionToCam(target);
    }
    override public void OnTargetHitRelease(Transform target) {
        curTarget = target;
        canvasUI.gameObject.SetActive(false);
    }

    /// HARD ADJUST
    //void adjustUIPositionTo(Transform target) {
    //    float yRem = gameObject.transform.position.y;

        //Vector3 pos = hitInfo.point;// target.position;
        //pos.y += 10;// gameObject.transform.position.y;        
        //gameObject.transform.position = pos;
        
        //Vector3 lookPos = currCamera.transform.position - gameObject.transform.position;
        //lookPos.y = 0;
        //lookPos = gameObject.transform.position + lookPos;
        //gameObject.transform.LookAt(lookPos);

    //LERP ADJAUST
    void adjustUIPositionToCam(Transform target) {

        if (strategicCamera.IsOnAction()) {
            fadeTarget = target;
        }
        if (fadeTarget!=null) {
            fadeUIPositionToTarget(ref fadeTarget);
            return;
        }

        Vector3 camPos = currCamera.transform.position - currCamera.transform.forward;// *10000;
        Vector3 lookPos = camPos - gameObject.transform.position;        
        //lookPos.y = 0;
        
        Quaternion newRot = Quaternion.LookRotation(lookPos);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, newRot, Time.deltaTime*popupRotateSpeed);
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, pos, Time.deltaTime*popupSpeed);

    }
    void fadeUIPositionToTarget(ref Transform fadeTarget){
        if (fadeTarget) {
            Vector3 lookPos = currCamera.transform.position - gameObject.transform.position;
            Quaternion newRot = Quaternion.LookRotation(lookPos);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, newRot, Time.deltaTime * popupRotateSpeed);
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, fadeTarget.position, Time.deltaTime * popupSpeed*2);
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, Vector3.zero, Time.deltaTime * fadeScaleSpeed*2);
        }

        if (!strategicCamera.IsOnAction() && (gameObject.transform.position - fadeTarget.position).magnitude < 1 ) {
            fadeTarget = null;
            canvasUI.gameObject.SetActive(false);
            gameObject.transform.localScale = Vector3.one;
            OnTargetHitEnter(curTarget);            
            //print("---------------------------!!!!---------------------------");
        }
    }

}
