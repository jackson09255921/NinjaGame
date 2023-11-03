using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform initialSpawnPoint;
    public Transform currentSpawnPoint;
    // Start is called before the first frame update
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
