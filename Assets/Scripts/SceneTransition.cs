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

    void Start()
    {
        SceneManager.LoadScene(sceneName);
        //StartCoroutine(LoadScene());
    }
 
    IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        int displayedProgress = 0;
        int currentProgress;
        while (op.progress < 0.9f)
        {
            currentProgress = (int)(op.progress * 100);
            while (displayedProgress < currentProgress)
            {
                ++displayedProgress;
                progressText.text = $"{displayedProgress}%";
                progressBar.value = displayedProgress / 100f;
                yield return new WaitForEndOfFrame();
            }
        }
        currentProgress = 100;
        while (displayedProgress < currentProgress)
        {
            ++displayedProgress;
            progressText.text = $"{displayedProgress}%";
            progressBar.value = displayedProgress / 100f;
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
    }
}
