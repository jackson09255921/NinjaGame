using UnityEngine;

public class Goal : MonoBehaviour
{
    public FadeTransition.Direction fadeDirection;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            GameStateManager.Instance.EnterGoal(player, fadeDirection);
        }
    }
}