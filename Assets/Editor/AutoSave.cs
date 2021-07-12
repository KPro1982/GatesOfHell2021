using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class AutoSave : EditorWindow
{
    public delegate bool DelegateIsValidMethod(out DateTime _lastSaved);

    static AutoSave()
    {
        EditorApplication.update += HandleUpdate;
        EditorApplication.playModeStateChanged += HandlePlayModeState;
        EditorSceneManager.sceneSaved += HandleSceneSaved;
        IsAutoSaveEnabled = PersistentData.AutoSaveEnabled;
        IsAutoRecoverEnabled = PersistentData.AutoRecoverEnabled;
        AutoSaveUI.AutoSaveMessage = "Saving...";
    }

    public static bool IsAutoSaveEnabled { get; set; }

    public static bool IsAutoRecoverEnabled { get; set; }


    public static void HandleUpdate()
    {
        var elapsedSaveTime = -1;
        var elapsedRecoverTime = -1;

        if (IsAutoSaveEnabled)
        {
            if (CheckLastSaveTime(PersistentData.AutoSaveFrequency * 60, out elapsedSaveTime))
            {
                SaveAll();
            }

            AutoSaveUI.AutoSaveMessage =
                $"Next save in {PersistentData.AutoSaveFrequency * 60 - elapsedSaveTime} seconds.";
        }


        if (IsAutoRecoverEnabled)
        {
            if (CheckLastAutoRecoverTime(PersistentData.AutoRecoverFrequency * 60,
                out elapsedRecoverTime))
            {
                if (!Application.isPlaying)
                {
                    SaveAutoRecover();
                }
            }

            AutoSaveUI.AutoRecoverMessage =
                $"Next auto recover in {PersistentData.AutoRecoverFrequency * 60 - elapsedRecoverTime} seconds.";
        }
    }

    private static void HandleSceneSaved(Scene scene)
    {
        if (IsAutoSaveEnabled)
        {
            PersistentData.LastSavedTime = DateTime.Now;
        }
    }


    private static void HandlePlayModeState(PlayModeStateChange state)
    {
        if (IsAutoSaveEnabled)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                SaveAll();
            }
        }
    }

    private static void TimedSave()
    {
        SaveAll();
    }

    public static void SaveAfter(int seconds)
    {
        if (CheckLastSaveTime(seconds))
        {
            SaveAll();
        }
    }

    private static bool CheckLast(int frequency, DelegateIsValidMethod _IsValid) =>
        CheckLast(frequency, out var elapsed, PersistentData.TryGetLastAutoRecoverTime);

    private static bool CheckLastSaveTime(int _frequency) =>
        CheckLast(_frequency, PersistentData.TryGetLastSavedTime);

    private static bool CheckLastSaveTime(int autoSaveFrequency, out int _elapsedTime) =>
        CheckLast(autoSaveFrequency, out _elapsedTime, PersistentData.TryGetLastSavedTime);

    private static bool CheckLastAutoRecoverTime(int seconds) =>
        CheckLast(seconds, PersistentData.TryGetLastAutoRecoverTime);

    private static bool CheckLastAutoRecoverTime(int seconds, out int _elapsedTime) =>
        CheckLast(seconds, out _elapsedTime, PersistentData.TryGetLastAutoRecoverTime);

    private static bool CheckLast(int _frequency, out int _elapsedTime,
        DelegateIsValidMethod _IsValid)
    {
        var currentTime = DateTime.Now;
        _elapsedTime = -1;
        DateTime lastSaved;
        TimeSpan elapsedSpan;

        if (!_IsValid(out lastSaved))
        {
            return true;
        }

        elapsedSpan = currentTime.Subtract(lastSaved);
        _elapsedTime = (int) elapsedSpan.TotalSeconds;

        if (_elapsedTime >= _frequency || _elapsedTime < 0)
        {
            return true;
        }

        return false;
    }

    public static void SaveAll()
    {
        if ((float) (EditorApplication.timeSinceStartup % 1000000) > 30)
        {
            PersistentData.LastSavedTime = DateTime.Now;

            if (SceneManager.GetActiveScene().isDirty)
            {
                if (!EditorApplication.ExecuteMenuItem("File/Save Project") ||
                    !EditorApplication.ExecuteMenuItem("File/Save"))
                {
                    Debug.Log("Error: Save not successful");
                    Debug.Assert(EditorApplication.ExecuteMenuItem("File/Save Project") &&
                                 EditorApplication.ExecuteMenuItem("File/Save"));
                }
            }
        }
        else
        {
            Debug.Log("Skipping initial save on application open.");
        }
    }

    private static void SaveAutoRecover()
    {
        PersistentData.LastAutoRecoverTime = DateTime.Now;
        if (!Directory.Exists(Application.dataPath + "/" + "AutoRecover"))
        {
            Directory.CreateDirectory(Application.dataPath + "/" + "AutoRecover");
            AssetDatabase.Refresh();
        }

        var sceneName = SceneManager.GetActiveScene().name;
        var savePath = Application.dataPath + "/" + "AutoRecover/" + sceneName + "_" +
                       DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + ".unity";

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), savePath, true);
    }
}