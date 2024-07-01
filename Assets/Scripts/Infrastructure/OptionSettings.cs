using System.IO;
using UnityEngine;

public class OptionSettings
{
    private const string _settingsFileName = "OptionSettings.json";

    public static Settings LoadSettings()
    {
        string filePath = Path.Combine(Application.persistentDataPath, _settingsFileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<Settings>(json);
        }
        return null;
    }

    public static void SaveSettings(float sfxVolume, float musicVolume)
    {
        Settings optionSettings = new Settings
        {
            SFXVolume = sfxVolume,
            MusicVolume = musicVolume
        };

        string json = JsonUtility.ToJson(optionSettings);
        string filePath = Path.Combine(Application.persistentDataPath, _settingsFileName);
        File.WriteAllText(filePath, json);
    }
}

[System.Serializable]
public class Settings
{
    public float SFXVolume;
    public float MusicVolume;
}