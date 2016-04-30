using UnityEngine;
using System.Collections;

public class ModuleInfoPopupMenu : MouseHoverInspector {

    Canvas canvasUI;
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
        print("ModuleInfoPopupMenu");
        canvasUI = gameObject.GetComponentInChildren<Canvas>();
    }
    override public void OnTargetHitEnter(Transform target) {
        adjustUIPositionTo(target);
        canvasUI.gameObject.SetActive(true);
    }
    override public void OnTargetHit(Transform target) {
        adjustUIPositionTo(target);
    }
    override public void OnTargetHitRelease(Transform target) {
        canvasUI.gameObject.SetActive(false);
    }

    void adjustUIPositionTo(Transform target) {
        float yRem = gameObject.transform.position.y;
        Vector3 pos = hitInfo.point;// target.position;
        pos.y += 10;// gameObject.transform.position.y;
        gameObject.transform.position = pos;
        gameObject.transform.LookAt(currCamera.transform.position);
    }

}
