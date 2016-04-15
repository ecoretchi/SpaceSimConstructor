using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Socket))]
public class SocketEditor : Editor {
    public override void OnInspectorGUI() {
        Socket script = (Socket)target;

        DrawDefaultInspector();

        //EditorGUILayout.LabelField("State", script.state.ToString());

        //string[] options = new string[]
        //{
        //    "Small", "Medium", "Large"
        //};
        //int id = (int)script.type;
        //if (id > 0)
        //    --id;
        //script.type = (Socket.Type) ( EditorGUILayout.Popup("Type", id, options) + 1 );
    }
}