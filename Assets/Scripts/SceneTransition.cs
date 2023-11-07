using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public TextMeshProUGUI progressText;
    public Slider progressBar;
    internal string sceneName;
    AsyncOperation op;
    int displayedProgress = 0;
    int currentProgress = 0;

    void Start()
    {
        op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
    }

    void Update()
    {
        if (op.progress < 0.9f)
        {
            currentProgress = (int)(op.progress * 100);
        }
        else
        {
            currentProgress = 100;
        }
        if (displayedProgress < currentProgress)
        {
            ++displayedProgress;
            progressText.text = $"{displayedProgress}%";
            progressBar.value = displayedProgress / 100f;
        }
        else if (displayedProgress == 100)
        {
            op.allowSceneActivation = true;
        }
    }
}
