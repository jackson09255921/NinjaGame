using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultMenu : MonoBehaviour
{
    public SceneTransition sceneTransitionPrefab;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;
    public Button nextLevelButton;
    public string nextSceneName;
    internal bool startFadeFromRight;

    internal void PlayerDied()
    {
        titleText.text = "Game Over";
        messageText.text = "You Died...";
    }
    
    internal void PlayerIncomplete(string message)
    {
        titleText.text = "Game Over";
        messageText.text = message;
    }
    
    internal void PlayerComplete(string message)
    {
        nextLevelButton.gameObject.SetActive(true);
        titleText.text = "Level Complete";
        messageText.text = message;
    }

    public void Quit()
    {
        SceneTransition transition = Instantiate(sceneTransitionPrefab);
        transition.sceneName = "Start";
    }

    public void Restart()
    {
        SceneTransition transition = Instantiate(sceneTransitionPrefab);
        transition.sceneName = SceneManager.GetActiveScene().path;
    }

    public void NextLevel()
    {
        SceneTransition transition = Instantiate(sceneTransitionPrefab);
        transition.sceneName = nextSceneName;
    }
}