using UnityEngine;
using System.Collections;

public class ShowModuleMenuInfo : MouseHoverInspector {

    StrategicCamera strategicCamera;
    MenuManager menuManager; 

    Transform curTarget;


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
        menuManager = (MenuManager)GameObject.FindObjectOfType(typeof(MenuManager));
    }
    override public void OnTargetHitEnter(Transform target) {
        curTarget = target;
        gameObject.transform.position = target.position;

        menuManager.ShowModuleInfoMenu(true);
    }
    override public void OnTargetHit(Transform target) {
        if (curTarget != target) {
            // next module selected, have to change info
        }
    }
    override public void OnTargetHitRelease(Transform target) {
        curTarget = null;

        menuManager.ShowModuleInfoMenu(false);
    }



}