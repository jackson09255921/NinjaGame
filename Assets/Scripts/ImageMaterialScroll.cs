using UnityEngine;
using UnityEngine.UI;

public class CanvasImageScroll : MonoBehaviour
{
    public Vector2 scrollSpeed = new(1, 1);
    Image image;
    Vector3 initialCameraPos;
    Vector2 textureScale;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        initialCameraPos = image.canvas.worldCamera.transform.position;
        textureScale = image.material.mainTextureScale;
    }

    void OnDestroy()
    {
        image.material.mainTextureOffset = new(0, 0);
    }

    void Update()
    {
        Camera camera = image.canvas.worldCamera;
        Vector2 imagePSize = image.rectTransform.rect.size;
        Vector2 cameraPSize = camera.pixelRect.size;
        Vector2 cameraWSize = new(camera.orthographicSize*camera.aspect*2, camera.orthographicSize*2);
        Vector2 wOffset = camera.transform.position-initialCameraPos;
        Vector2 iOffset = wOffset*(cameraPSize/cameraWSize)/imagePSize*textureScale*scrollSpeed;
        image.material.mainTextureOffset = iOffset;
    }
}
