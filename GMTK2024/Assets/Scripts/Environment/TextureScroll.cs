using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    [SerializeField] float followPercentage;

    private void Update()
    {
        Camera cam = Camera.main;
        float top = cam.ViewportToWorldPoint(new Vector3(0f, 1f)).y;
        float bottom = cam.ViewportToWorldPoint(Vector3.zero).y;
        float height = top - bottom;

        transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y * followPercentage - height / 2);
    }
}
