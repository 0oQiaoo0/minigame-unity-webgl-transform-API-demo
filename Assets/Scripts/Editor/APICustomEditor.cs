using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(APIController))]
public class APICustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var api = (APIController)target;

        if (GUILayout.Button("Generate API"))
        {
            api.Init();
        }
    }
}