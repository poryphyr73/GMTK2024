using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float velocity = 0f, acceleration = 0.001f;
    [SerializeField] TextMeshProUGUI scoreCounter;
    bool startedMoving;
    Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        scoreCounter.text = transform.position.y.ToString("00000.00") + " cm";
        if (!startedMoving && player.position.y <= 0) return;
        startedMoving = true;

        velocity += acceleration * Time.deltaTime;

        float difference = player.position.y - Camera.main.ViewportToWorldPoint(new Vector3(1, 1)).y + player.localScale.y * 2;

        float tempVelocity = velocity;

        if (difference > 0) tempVelocity += difference * 2;

        Vector3 target = new Vector3(0f, 1000000f, 0f) + transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target, tempVelocity * Time.deltaTime);
    }
}
