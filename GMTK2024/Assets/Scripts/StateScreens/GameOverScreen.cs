using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameOverText, gameOverButton;
    [SerializeField] RawImage blackScreen;
    [SerializeField] float timeToFade;

    public IEnumerator FadeInText()
    {
        FindObjectOfType<AudioManager>().Play("Game Over");
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        float t = 0;

        while(t < timeToFade)
        {
            float alpha = t / timeToFade;
            gameOverText.alpha = alpha;
            gameOverButton.alpha = alpha;
            t += Time.deltaTime;
            yield return null;
        }
    }

    public void StartBlackFade()
    {
        StartCoroutine(FadeToBlack());
    }

    public IEnumerator FadeToBlack()
    {
        FindObjectOfType<AudioManager>().Play("Game Restart");
        blackScreen.gameObject.SetActive(true);

        float t = 0;

        while(t < timeToFade)
        {
            float alpha = t / timeToFade;
            blackScreen.color = new Color(0, 0, 0, alpha);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(timeToFade);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
