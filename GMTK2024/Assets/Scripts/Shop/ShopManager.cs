using System;
using Random=System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ObstacleItem[] possibleObstacles;
    [SerializeField] float flightSpeed, snapDistance;
    [SerializeField] bool open, gameOver;
    [SerializeField] SpriteRenderer holo;
    [SerializeField] Image[] ShopButtons;
    [SerializeField] TextMeshProUGUI moneyCounter;

    ObstacleItem[] currentObstacles = new ObstacleItem[3];
    ObstacleItem purchased;
    
    Vector3 target, velocity = Vector3.zero;
    RectTransform canvasTransform;

    int balance = 20;

    public void Start()
    {
        canvasTransform = GetComponent<RectTransform>();
        RefreshBalanceVisual();
        target = canvasTransform.anchoredPosition;
    }

    public void Buy(int slot)
    {
        if (!open || purchased != null) return;

        ObstacleItem a = currentObstacles[slot];

        if (balance < a.cost) return;

        LoseMoney(a.cost);
        purchased = a;
        open = false;
        holo.sprite = purchased.holo;
        holo.gameObject.SetActive(true);

        //currentObstacles[slot].GetComponent<>
    }

    private void Update()
    {
        if ((canvasTransform.anchoredPosition3D - target).magnitude > snapDistance) canvasTransform.anchoredPosition = Vector3.SmoothDamp(canvasTransform.anchoredPosition, target, ref velocity, flightSpeed);
        else canvasTransform.anchoredPosition = Vector3.MoveTowards(canvasTransform.anchoredPosition, target, snapDistance / flightSpeed);

        Vector2 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        holo.transform.position = mouseInput;
        if (Input.GetMouseButton(0)) TryPlace();
    }

    IEnumerator Animate()
    {
        while(!gameOver)
        {
            RandomizeShop();
            target = new Vector3(canvasTransform.anchoredPosition.x, 0);

            while (canvasTransform.anchoredPosition.y < target.y - 1)
            {
                yield return null;
            }
            canvasTransform.anchoredPosition = target;

            open = true;

            while (open) yield return null;

            target = new Vector3(canvasTransform.anchoredPosition.x, 2 * Screen.height);

            while (canvasTransform.anchoredPosition.y < target.y - 1) yield return null;
            canvasTransform.anchoredPosition = target;

            canvasTransform.anchoredPosition = new Vector3(canvasTransform.anchoredPosition.x, -Screen.height);
        }
    }

    void RandomizeShop()
    {
        List<ObstacleItem> tempObstacles = new List<ObstacleItem>();
        tempObstacles.AddRange(possibleObstacles);
        for(int i = 0; i < tempObstacles.Count; i++)
        {
            Random random = new Random();
            int k;
            k = random.Next(tempObstacles.Count);
            // if (i == 0) {
            //     while (tempObstacles[k].cost > balance || tempObstacles[k].cost !=  1) {
            //         k = random.Next(tempObstacles.Count);
            //     }
            // }
            currentObstacles[i] = tempObstacles[k];
            tempObstacles.RemoveAt(k);
            InitializeSlot(i);
        }
    }

    void InitializeSlot(int i)
    {
        ShopButtons[i].sprite = currentObstacles[i].preview;
        ShopButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentObstacles[i].cost.ToString();
    }

    void TryPlace()
    {
        if (purchased == null) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, 1 << LayerMask.NameToLayer("Tree"));

        if (hit.collider != null)
        {
            FindObjectOfType<SFXManager>().Play("Build");
            Instantiate(purchased.obstacle, mousePos, Quaternion.identity);
            purchased = null;
            holo.gameObject.SetActive(false);
        }
    }

    public void AddCoin()
    {
        AddCoin(1);
    }

    public void AddCoin(int value)
    {
        balance += value;
        RefreshBalanceVisual();
    }

    public void LoseMoney()
    {
        LoseMoney(1);
    }

    public void LoseMoney(int value)
    {
        balance -= value;

        SFXManager man = FindObjectOfType<SFXManager>();
        if (value <= 2) man.Play("Small Purchase");
        else if (value <= 4) man.Play("Medium Purchase");
        else man.Play("Large Purchase");

        RefreshBalanceVisual();
    }

    void RefreshBalanceVisual()
    {
        moneyCounter.text = balance.ToString();
    }

    public void OpenShop()
    {
        StartCoroutine(Animate());
        moneyCounter.transform.parent.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        open = false;
        gameOver = true;
        purchased = null;
        holo.gameObject.SetActive(false);
    }
}
