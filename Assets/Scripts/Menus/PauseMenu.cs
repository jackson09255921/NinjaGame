using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public SceneTransition sceneTransitionPrefab;
    public void BackToGameTHeme()
    {
        SceneTransition transition = Instantiate(sceneTransitionPrefab);
        transition.sceneName = "Start";
    }
}