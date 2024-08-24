using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    Vector3 screenCenter;
    BoxCollider2D triggerZone;
    float width = 2f;
    private void Start()
    {
        screenCenter = new Vector3(Screen.width / 2, 0f, 0f);
        triggerZone = GetComponent<BoxCollider2D>();
        triggerZone.size = new Vector2(Screen.width, width);
    }
    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(screenCenter) - new Vector3(0f, width / 2 - 0.05f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        collision.gameObject.GetComponent<PlayerController>().EndGame(new Vector3(collision.transform.position.x, Camera.main.ViewportToWorldPoint(Vector3.zero).y + collision.offset.y / 2 + collision.bounds.size.y));
    }
}
