using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragilePlatform : MonoBehaviour
{
    [SerializeField] float timeUntilBreak, acceleration;
    Animator anim;
    float timer;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        anim.SetTrigger("Stepped On");
        StartCoroutine(Crumble());
    }

    IEnumerator Crumble()
    {
        //Animate Fall
        while(timer < timeUntilBreak)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        transform.GetChild(0).gameObject.SetActive(false);
        //Animate Fade
        anim.SetTrigger("Fall");

        float v = 0;
        Vector3 target = new Vector3(transform.position.x, -5f);

        while(transform.position.y > target.y)
        {
            v += acceleration * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, v * Time.deltaTime);
            yield return null;
        }
    }
}
