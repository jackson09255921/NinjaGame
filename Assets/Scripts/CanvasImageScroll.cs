using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasImageScroll : MonoBehaviour
{
    public Vector2 scrollSpeed = new(1, 1);
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void OnDestroy()
    {
        image.material.mainTextureOffset = new(0, 0);
    }

    void Update()
    {
        Camera camera = image.canvas.worldCamera;;
        Vector2 size = new(camera.orthographicSize*2, camera.orthographicSize*camera.aspect*2);
        Vector2 position = camera.transform.position;
        Vector2 offset = position/size*scrollSpeed;
        image.material.mainTextureOffset = offset;
    }
}
