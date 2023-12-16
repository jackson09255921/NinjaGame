using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultMenu : MonoBehaviour
{
    public SceneTransition sceneTransitionPrefab;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;
    public Button restartButton;
    public Button nextLevelButton;
    internal FadeTransition.Direction fadeDirection;

    void OnEnable()
    {
        if (nextLevelButton.gameObject.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(nextLevelButton.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
        }
    }

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
        GameStateManager.Instance.RestartResults();
    }

    public void NextLevel()
    {
        StartTransition(GameStateManager.Instance.nextSceneName);
        GameStateManager.Instance.ClearResults();
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