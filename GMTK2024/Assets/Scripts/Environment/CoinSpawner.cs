using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] coinPrefabs;
    [SerializeField] float spawnRate;
    [SerializeField] SpriteRenderer bark;
    float nextDrop;

    private void Start()
    {
        nextDrop = Camera.main.ViewportToWorldPoint(Vector3.one).y + 1f;
    }
    private void Update()
    {
        float screenTop = Camera.main.ViewportToWorldPoint(Vector3.one).y + 1f;
        if (screenTop < nextDrop) return;

        float minimum = -bark.bounds.size.x / 2 + 1;
        float maximum = -minimum - 1;
        float randomHorizontal = Random.Range(minimum, maximum);

        Vector3 targetPosition = new Vector3(randomHorizontal, screenTop);

        Instantiate(coinPrefabs[Random.Range(0, coinPrefabs.Length)], targetPosition, Quaternion.identity);
        nextDrop += spawnRate;
    }
}
