using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject infoMenu;
    public GameObject settingsMenu;
    public SceneTransition sceneTransitionPrefab;

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

    public void Exit()
    {
        GameStateManager.Instance.UpdateEscape();
    }

    internal void Reset()
    {
        pauseMenu.SetActive(true);
        infoMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }
}
