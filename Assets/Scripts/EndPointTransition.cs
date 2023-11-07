using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointTransition : MonoBehaviour
{
    public SceneTransition sceneTransitionPrefab;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            SceneTransition transition = Instantiate(sceneTransitionPrefab);
            transition.sceneName = "End";
        }
    }
}
