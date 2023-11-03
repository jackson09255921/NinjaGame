using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform initialSpawnPoint;
    Transform currentSpawnPoint;

    void Start()
    {
        currentSpawnPoint = initialSpawnPoint;
    }

    public void Respawn()
    {
        transform.position = currentSpawnPoint.position;
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        currentSpawnPoint = newSpawnPoint;
    }
}
