using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    ShopManager shop;

    private void Start()
    {
        shop = FindFirstObjectByType<ShopManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        shop.AddCoin(value);

        FindObjectOfType<SFXManager>().Play("Get Coin");
        Destroy(this.gameObject);
    }
}
