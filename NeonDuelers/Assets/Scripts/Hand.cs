using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
    public List<Card> cards;
    public Player owner;

    public void FitHand() {
        float spacing = 1.5f;
        for (int i = 0; i < cards.Count; i++) {
            cards[i].transform.position = new Vector3((transform.position.x - i + ((cards.Count-1) / 2f)) * spacing, transform.position.y, transform.position.z);
        }
    }

    public void ResetHand() {
        foreach (Card card in cards) {
            card.transform.SetParent(owner.deck.transform);
            card.model.SetActive(false);
            card.col.enabled = false;
        }
        cards.Clear();
    }
}

