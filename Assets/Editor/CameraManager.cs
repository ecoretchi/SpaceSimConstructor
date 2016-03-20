using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SelectingMicros))]
public class CameraManager : Editor
{
    public override void OnInspectorGUI() 
    {
        //SelectingMicros script = (SelectingMicros)target;

        //Object obj = EditorGUILayout.ObjectField("AbstractThirdCamera", (Object)script.thirdCam, typeof(AbstractThirdCamera), true);
        //if(obj)
        //    script.thirdCam = (AbstractThirdCamera)obj;
        DrawDefaultInspector();
    }
}