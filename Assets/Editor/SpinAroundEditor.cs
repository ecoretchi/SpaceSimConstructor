using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpinAround))]
public class SpinAroundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SpinAround script = (SpinAround)target;

        DrawDefaultInspector();
        
        string[] options = new string[]
        {
            "LeftMouseButton", "RightMouseButton", "MiddleMouseButton"
        };
        script.MouseButtonID = EditorGUILayout.Popup("Mouse Button ID", script.MouseButtonID, options);
        EditorGUILayout.LabelField("Torque", script.torque.ToString());
    }
}