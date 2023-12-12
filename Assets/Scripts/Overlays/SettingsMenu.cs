using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Slider lightSlider;

    void Start()
    {
        musicVolumeSlider.value = SettingsManager.Instance.MusicVolume;
        lightSlider.value = SettingsManager.Instance.Light;
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        lightSlider.onValueChanged.AddListener(SetLight);
    }

    public void SetMusicVolume(float value)
    {
        SettingsManager.Instance.SetMusicVolume(value);
    }

    public void SetLight(float value)
    {
        SettingsManager.Instance.SetLight(value);
    }
}
