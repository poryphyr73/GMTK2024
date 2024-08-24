using UnityEngine;

public class TextureLoop : MonoBehaviour
{
    private float length;

    private void Start()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.y / 3;
    }

    private void Update()
    {
        if (Camera.main.transform.position.y >= transform.position.y + length) transform.position = new Vector3(transform.position.x, transform.position.y + length);
    }
}