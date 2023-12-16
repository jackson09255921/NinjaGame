using System;
using UnityEngine;

public class SettingsManager
{
    static SettingsManager instance;
    public static SettingsManager Instance
    {
        get
        {
            return instance ??= new SettingsManager();
        }
    }

    public float MusicVolume { get; private set; }

    Action<float> musicVolumeListener;

    SettingsManager()
    {
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
    }

    public void AddMusicVolumeListener(Action<float> listener)
    {
        musicVolumeListener += listener;
    }

    public void RemoveMusicVolumeListener(Action<float> listener)
    {
        musicVolumeListener -= listener;
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        MusicVolume = value;
        musicVolumeListener?.Invoke(value);
    }
}