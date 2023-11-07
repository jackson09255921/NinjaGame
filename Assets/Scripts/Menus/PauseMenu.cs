using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
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
}
