using System;
using UnityEditor;

public class PersistentData : EditorWindow
{
    private static bool _autoSaveEnabled;
    private static bool _autoRecoverEnabled;
    private static DateTime _lastSavedTime;

    private static DateTime _lastAutoRecoverTime;

    private static int _autoSaveFrequency = 1; // minutes

    private static int _autoRecoverFrequency = 10; // minutes

    public static float EditorTimeInSeconds =>
        (float) (EditorApplication.timeSinceStartup % 1000000);

    public static bool AutoSaveEnabled
    {
        get
        {
            bool isEnabled;
            var strEnabled = GetValue("AutoSaveEnabled");
            if (bool.TryParse(strEnabled, out isEnabled))
            {
                return isEnabled;
            }

            return true;
        }
        set
        {
            _autoSaveEnabled = value;
            SetValue("AutoSaveEnabled", _autoSaveEnabled.ToString());
            AutoSave.IsAutoSaveEnabled = value;
        }
    }

    public static int AutoSaveFrequency
    {
        get
        {
            int tempAutoSaveFrequency;
            var strAutoSaveFrequency = GetValue("AutoSaveFrequency");
            if (int.TryParse(strAutoSaveFrequency, out tempAutoSaveFrequency))
            {
                return tempAutoSaveFrequency;
            }

            return 1;
        }
        set
        {
            _autoSaveFrequency = value;
            SetValue("AutoSaveFrequency", _autoSaveFrequency.ToString());
        }
    }

    public static int AutoRecoverFrequency
    {
        get
        {
            int tempAutoRecoverFrequency;
            var strAutoRecoverFrequency = GetValue("AutoRecoverFrequency");
            if (int.TryParse(strAutoRecoverFrequency, out tempAutoRecoverFrequency))
            {
                return tempAutoRecoverFrequency;
            }

            return 1;
        }
        set
        {
            _autoRecoverFrequency = value;
            SetValue("AutoRecoverFrequency", _autoRecoverFrequency.ToString());
        }
    }

    public static bool AutoRecoverEnabled
    {
        get
        {
            bool isEnabled;
            var strEnabled = GetValue("AutoRecoverEnabled");
            if (bool.TryParse(strEnabled, out isEnabled))
            {
                AutoSave.IsAutoRecoverEnabled = isEnabled;
                return isEnabled;
            }

            return true;
        }
        set
        {
            _autoRecoverEnabled = value;
            SetValue("AutoRecoverEnabled", _autoRecoverEnabled.ToString());
            AutoSave.IsAutoSaveEnabled = value;
        }
    }


    public static DateTime LastSavedTime
    {
        set
        {
            _lastSavedTime = value;
            SetValue("LastSavedTime", string.Format("{0:G}", _lastSavedTime));
        }
    }


    public static DateTime LastAutoRecoverTime
    {
        set
        {
            _lastAutoRecoverTime = value;
            SetValue("LastAutoRecoverTime", string.Format("{0:G}", _lastAutoRecoverTime));
        }
    }

    private static void SetValue(string _key, string _value)
    {
        EditorPrefs.SetString(_key, _value);
    }

    private static string GetValue(string _key)
    {
        if (EditorPrefs.HasKey(_key))
        {
            return EditorPrefs.GetString(_key);
        }

        return "";
    }

    public static bool TryGetLastSavedTime(out DateTime _lastSavedTime)
    {
        var strLastSavedTime = GetValue("LastSavedTime");
        if (strLastSavedTime.Length > 0)
        {
            if (DateTime.TryParse(strLastSavedTime, out _lastSavedTime))
            {
                return true;
            }
        }

        _lastSavedTime = DateTime.Now;
        return false;
    }

    public static string GetLastSaveTimeStr()
    {
        DateTime tempLastSavedTime;
        if (TryGetLastSavedTime(out tempLastSavedTime))
        {
            return tempLastSavedTime.ToString();
        }

        return string.Empty;
    }

    public static bool TryGetLastAutoRecoverTime(out DateTime _lastAutoRecoverTime)
    {
        var strLastAutoRecoverTime = GetValue("LastAutoRecoverTime");
        if (strLastAutoRecoverTime.Length > 0)
        {
            if (DateTime.TryParse(strLastAutoRecoverTime, out _lastAutoRecoverTime))
            {
                return true;
            }
        }

        _lastAutoRecoverTime = DateTime.Now;
        return false;
    }

    public static string GetLastAutoRecoverTimeStr()
    {
        DateTime tempLastRecoverTime;
        if (TryGetLastAutoRecoverTime(out tempLastRecoverTime))
        {
            return tempLastRecoverTime.ToString();
        }

        return string.Empty;
    }
}