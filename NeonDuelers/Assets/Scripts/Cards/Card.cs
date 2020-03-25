using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public Player owner;
    public SpriteRenderer cardSurface;
    public TextMeshPro cardName;
    public SpriteRenderer cardArt;
    public TextMeshPro description;
    public GameObject costSprite;
    public GameObject model;
    public TextMeshPro costAmount;
    public Collider2D col;
    public int price = 20;

    public void ShowCost(bool show) {
        costSprite.SetActive(show);
        costAmount.text = price.ToString();
    }



    public void PurchaseCard() {
        Debug.Log("Purchased!");
    }
}
