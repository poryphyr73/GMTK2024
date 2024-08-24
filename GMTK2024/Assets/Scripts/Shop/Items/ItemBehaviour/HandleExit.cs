using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleExit : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y <= Camera.main.ViewportToWorldPoint(Vector3.zero).y - 5) Destroy(gameObject);
    }
}
