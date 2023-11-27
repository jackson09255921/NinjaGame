using System;
using UnityEngine;

public class SettingsManager
{
    private static SettingsManager instance;
    public static SettingsManager Instance
    {
        get
        {
            return instance ??= new SettingsManager();
        }
    }

    public float MusicVolume { get; private set; }
    public float Light { get; private set; }

    Action<float> musicVolumeListener;
    Action<float> lightListener;

    SettingsManager()
    {
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        Light = PlayerPrefs.GetFloat("Light", 0f);
    }

    public void AddMusicVolumeListener(Action<float> listener)
    {
        musicVolumeListener += listener;
    }

    public void RemoveMusicVolumeListener(Action<float> listener)
    {
        musicVolumeListener -= listener;
    }

    public void AddLightListener(Action<float> listener)
    {
        lightListener += listener;
    }

    public void RemoveLightListener(Action<float> listener)
    {
        lightListener -= listener;
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        MusicVolume = value;
        musicVolumeListener?.Invoke(value);
    }

    public void SetLight(float value)
    {
        PlayerPrefs.SetFloat("Light", value);
        PlayerPrefs.Save();
        Light = value;
        lightListener?.Invoke(value);
    }
}