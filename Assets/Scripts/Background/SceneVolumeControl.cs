using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneVolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
   
    private void OnEnable()
    {
        BroadcastMessage("OnVolumeChanged", SendMessageOptions.DontRequireReceiver);   
    }

    private void OnVolumeChanged(float value)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.volume = value;
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = value;
        }
    }
}
