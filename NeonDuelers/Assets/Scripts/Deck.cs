using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deck : MonoBehaviour {
    public Player owner;
    public List<Card> cards = new List<Card>();
    public TextMeshPro sizeText;
    public SpriteRenderer sizePrefab;
    public Transform sizeParent;
    public List<SpriteRenderer> sizeSprites = new List<SpriteRenderer>();

    List<Card> savedDeck = new List<Card>();

    public void Add(Card card) {
        cards.Add(card);
    }

    public void UpdateCount() {
        for (int i = 0; i < cards.Count; i++) {
            if (i >= sizeSprites.Count) {
                SpriteRenderer newSprite = Instantiate(sizePrefab, sizeParent.transform.position + (Vector3.down * 0.01f * i), Quaternion.identity, sizeParent);
                newSprite.sortingOrder = i;
                sizeSprites.Add(newSprite);
            }
        }
        for (int i = 0; i < sizeSprites.Count; i++) {
            if (i >= cards.Count) {
                Destroy(sizeSprites[i].gameObject);
                sizeSprites.RemoveAt(i);
            }
        }
        sizeText.text = cards.Count.ToString();
        sizeText.sortingOrder = sizeSprites.Count + 1;
    }

    public void SetDeck() {
        //Save the deck in the buffer
        savedDeck.Clear();
        savedDeck.AddRange(cards);

    }

    public void ResetDeck() {
        foreach (Card card in cards) {
            if (!savedDeck.Contains(card)) Destroy(card.gameObject);
            else {
                card.transform.SetParent(transform);
                card.model.SetActive(false);
                card.col.enabled = false;
            }
        }
        cards.Clear();
        cards.AddRange(savedDeck);
        UpdateCount();
    }
}
