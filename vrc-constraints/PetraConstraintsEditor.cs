using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PetraConstraints))]
public class PetraConstraintsEditor : Editor
{
    private bool showConfirm = false;

    public override void OnInspectorGUI()
    {
        PetraConstraints script = (PetraConstraints)target;

        EditorGUILayout.HelpBox("This nukes the Constraints sub-object and rebuilds it.", MessageType.Warning);
        

        if (showConfirm)
        {
            if (GUILayout.Button("Confirm"))
            {
                showConfirm = false;
                script.FixConstraints();
            }
            if (GUILayout.Button("Abort"))
            {
                showConfirm = false;
            }
        } else
        {
            if (GUILayout.Button("Rebuild Constraints"))
            {
                showConfirm = true;
            }
        }

    }
}
