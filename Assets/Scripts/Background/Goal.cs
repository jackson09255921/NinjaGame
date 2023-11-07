using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            GameStateManager.Instance.ShowResults(player);
        }
    }
}