using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedCards : MonoBehaviour
{
    public List<Card> used = new List<Card>();
    public Player owner;

    public void ReturnToDeck() {
        foreach (Card card in used) { 
            card.transform.SetParent(owner.deck.transform);
        }
        used.Clear();
    }
}
