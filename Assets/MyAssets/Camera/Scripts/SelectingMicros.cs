using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AbstractThirdCamera))]
public class SelectingMicros : MonoBehaviour
{    
    AbstractThirdCamera thirdCam;
    Camera camera;
    Transform tmpHitSelected;
    void Start()
    {
        if(!camera)
            camera = GetComponent<Camera>();
        
            thirdCam = camera.GetComponent<AbstractThirdCamera>();
    }

    bool GetHitTransform(out Transform t, string tag)
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        bool res = Physics.Raycast(ray, out hitInfo);
        t = hitInfo.transform;        
        return res && t.tag == tag;
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Transform hitTransform;
            if (GetHitTransform(out hitTransform, "Construction"))
            {
                print("Target selected");
                tmpHitSelected = hitTransform;

            }
            else
                tmpHitSelected = null;
        }
        if (Input.GetMouseButtonUp(0) )
        {
            Transform hitTransform;
            if ( GetHitTransform(out hitTransform, "Construction") && hitTransform==tmpHitSelected)
            {
                print("Target changes");
                thirdCam.SetTarget(tmpHitSelected);
            }
        }
    }
}