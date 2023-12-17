using TMPro;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public TextMeshProUGUI totalTimeText;
    public TextMeshProUGUI completionText;
    public SceneTransition sceneTransitionPrefab;

    void Start()
    {
        float totalTime = ResultManager.Instance.GetTotalTimeAll();
        totalTimeText.text = $"{(int)totalTime/60:00}:{totalTime%60:00.00}";
        int collectedItems = ResultManager.Instance.GetCollectedItemsAll();
        int totalItems = ResultManager.Instance.GetTotalItemsAll();
        completionText.text = $"{(totalItems > 0 ? collectedItems*100/totalItems : 0)}%";
    }

    public void Return()
    {
        SceneTransition transition = Instantiate(sceneTransitionPrefab);
        transition.sceneName = "Start";
    }
}
