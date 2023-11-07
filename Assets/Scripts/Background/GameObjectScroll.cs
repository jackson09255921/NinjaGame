using UnityEngine;

public class GameObjectScroll : MonoBehaviour
{
    public new Camera camera;
    public Vector2 scrollSpeed = Vector2.one;
    Vector2 initialPos;
    Vector3 initialCameraPos;

    void Start()
    {
        camera = camera != null ? camera : Camera.main;
        initialPos = transform.position;
        initialCameraPos = camera.transform.position;
    }

    void Update()
    {
        Vector2 cameraOffset = camera.transform.position-initialCameraPos;
        Vector2 offset = cameraOffset*(Vector2.one-scrollSpeed);
        transform.position = initialPos+offset;
    }
}
