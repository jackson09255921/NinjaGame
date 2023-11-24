using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider voulmeSlider;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (voulmeSlider == null || audioSource == null)
        {
            Debug.LogError("VolumeSlider or AudioSource is not assigned in the inspector!");
            return;
        }
        voulmeSlider.onValueChanged.AddListener(onValueChanged);
    }

    private void onValueChanged(float value)
    {
        audioSource.volume = value;

        BroadcastMessage("OnVolumeChanged", value, SendMessageOptions.DontRequireReceiver);  
    }
}
