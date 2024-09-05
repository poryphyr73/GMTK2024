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
    [SerializeField] GameObject controlsScreen;

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

    public void OpenControlsSheet(){
        controlsScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void CloseControlsSheet(){
        controlsScreen.SetActive(false);
        gameObject.SetActive(true);
    }

}
