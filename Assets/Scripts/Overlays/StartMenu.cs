using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public SceneTransition sceneTransitionPrefab;

    public void PlayGame()
    {
        SceneTransition transition = Instantiate(sceneTransitionPrefab);
        transition.sceneName = "Level1";
        ResultManager.Instance.Reset();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
