using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public SceneTransition sceneTransitionPrefab;

    public void PlayGame()
    {
        ResultManager.Instance.ResetAll();
        SceneTransition transition = Instantiate(sceneTransitionPrefab);
        transition.sceneName = "Level1";
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
