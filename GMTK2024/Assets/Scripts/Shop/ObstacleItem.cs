using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Item", menuName = "ShopItem/ObstacleItem", order = 0)]
public class ObstacleItem : ScriptableObject
{
    public int cost;
    public Sprite preview, holo;
    public GameObject obstacle;
}
