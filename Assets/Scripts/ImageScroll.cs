using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroll : MonoBehaviour
{
    public Vector2 scrollSpeed = new(1, 1);
    Vector2 initialPos;
    Vector3 initialCameraPos;
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        initialPos = image.rectTransform.anchoredPosition;
        initialCameraPos = image.canvas.worldCamera.transform.position;
    }

    void OnDestroy()
    {
        image.material.mainTextureOffset = new(0, 0);
    }

    void Update()
    {
        Camera camera = image.canvas.worldCamera;
        Vector2 cameraPSize = camera.pixelRect.size;
        Vector2 cameraWSize = new(camera.orthographicSize*camera.aspect*2, camera.orthographicSize*2);
        Vector2 wOffset = camera.transform.position-initialCameraPos;
        Vector2 pOffset = wOffset*(cameraPSize/cameraWSize)*scrollSpeed;
        image.rectTransform.anchoredPosition = initialPos-pOffset;
    }
}
