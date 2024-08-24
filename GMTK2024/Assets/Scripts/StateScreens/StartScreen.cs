using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    [SerializeField] RawImage blackScreen;
    [SerializeField] float timeToFade;

    private void Start()
    {
        StartCoroutine(FadeFromBlack());
    }
    public IEnumerator FadeFromBlack()
    {
        FindObjectOfType<AudioManager>().Play("Game Music");
        float t = timeToFade;

        while (t > 0)
        {
            float alpha = t / timeToFade;
            blackScreen.color = new Color(0, 0, 0, alpha);
            t -= Time.deltaTime;
            yield return null;
        }

        blackScreen.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        FindObjectOfType<ShopManager>().OpenShop();
        gameObject.SetActive(false);
    }
}
