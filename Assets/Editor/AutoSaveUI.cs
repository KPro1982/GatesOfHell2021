using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSaveUI : EditorWindow
{
    public static string AutoSaveMessage { get; set; }
    public static string AutoRecoverMessage { get; set; }

    private void OnGUI()
    {
        GUILayout.Space(5);

        PersistentData.AutoSaveEnabled =
            EditorGUILayout.BeginToggleGroup("AutoSave", PersistentData.AutoSaveEnabled);
        PersistentData.AutoSaveFrequency = (int) EditorGUILayout.Slider("AutoSave (minutes):",
            PersistentData.AutoSaveFrequency, 1, 30);
        GUILayout.Label($"Last Saved: {PersistentData.GetLastSaveTimeStr()}");
        GUILayout.Label($"{SceneManager.GetActiveScene().name} is {IsDirtyMessage()}.");
        GUILayout.Label(AutoSaveMessage);
        EditorGUILayout.EndToggleGroup();

        GUILayout.Space(20);

        PersistentData.AutoRecoverEnabled =
            EditorGUILayout.BeginToggleGroup("AutoRecover", PersistentData.AutoRecoverEnabled);
        PersistentData.AutoRecoverFrequency = (int) EditorGUILayout.Slider("AutoRecover (minutes):",
            PersistentData.AutoRecoverFrequency, 1, 60);
        GUILayout.Label($"Last AutoRecover: {PersistentData.GetLastAutoRecoverTimeStr()}");
        GUILayout.Space(5);
        GUILayout.Label(AutoRecoverMessage);
        EditorGUILayout.EndToggleGroup();
    }

    private string IsDirtyMessage() => SceneManager.GetActiveScene().isDirty ? "dirty" : "clean";

    [MenuItem("Window/AutoSave")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        var inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
        EditorWindow window = GetWindow<AutoSaveUI>("AutoSave", inspectorType);
        window.Show();
    }
}