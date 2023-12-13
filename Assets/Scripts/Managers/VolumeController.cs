using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public AudioSource musicAudioSource;

    void Awake()
    {
        SettingsManager.Instance.AddMusicVolumeListener(OnMusicVolumeChanged);
        musicAudioSource.volume = SettingsManager.Instance.MusicVolume;
    }

    void OnDestroy()
    {
        SettingsManager.Instance.RemoveMusicVolumeListener(OnMusicVolumeChanged);
    }

    void OnMusicVolumeChanged(float value)
    {
        musicAudioSource.volume = value;
    }
}
