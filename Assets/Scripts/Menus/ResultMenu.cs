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
    internal FadeTransition.Direction fadeDirection;

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
        StartTransition("Start");
    }

    public void Restart()
    {
        StartTransition(SceneManager.GetActiveScene().path);
    }

    public void NextLevel()
    {
        StartTransition(nextSceneName);
    }

    void StartTransition(string sceneName)
    {
        GameStateManager.Instance.fadeTransition.StartFade(fadeDirection, true, GameStateManager.Instance.fadeTime, () => TransitionScene(sceneName));
    }

    void TransitionScene(string sceneName)
    {
        SceneTransition transition = Instantiate(sceneTransitionPrefab);
        transition.sceneName = sceneName;
    }
}