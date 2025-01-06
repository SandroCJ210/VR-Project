using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MemoryState))]
public class MemoryStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MemoryState memoryState = (MemoryState)target;

        GUILayout.Label("Memory State Inspector", EditorStyles.boldLabel);

        GUILayout.Label("Stored Data", EditorStyles.largeLabel);

        if (memoryState.GetAllData().Count == 0)
        {
            GUILayout.Label("No data stored.");
        }
        else
        {
            foreach (var key in memoryState.GetAllData().Keys)
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.Label(key, GUILayout.Width(100));
                GUILayout.Label(memoryState.GetData<object>(key)?.ToString() ?? "null");


                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    memoryState.RemoveData(key);
                    EditorUtility.SetDirty(memoryState);
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Open Memory State Editor"))
        {
            MemoryStateEditorWindow.OpenWindow(memoryState);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(memoryState);
        }
    }
}
