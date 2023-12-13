using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicVolumeSlider;

    void Start()
    {
        musicVolumeSlider.value = SettingsManager.Instance.MusicVolume;
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMusicVolume(float value)
    {
        SettingsManager.Instance.SetMusicVolume(value);
    }
}
