using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalStake : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        collision.gameObject.GetComponent<PlayerController>().EndGame();
    }
}
