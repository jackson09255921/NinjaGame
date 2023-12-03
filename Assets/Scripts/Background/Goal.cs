using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool startFadeFromRight;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            GameStateManager.Instance.EnterGoal(player, startFadeFromRight);
        }
    }
}