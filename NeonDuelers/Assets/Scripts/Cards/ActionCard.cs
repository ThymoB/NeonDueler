using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class ActionCard : Card
{
    public int energyCost;
    public TextMeshPro energyCostText;
    public CardAbility cardAbility;

    private void Awake() {
        UpdateEnergyCost();
    }

    public void OnMouseDown() {
        switch (GameManager.Instance.players[0].playerState) {
            case PlayerState.Farming:
                UseCard();
                break;
            case PlayerState.Dueling:
                UseCard();
                break;
            case PlayerState.Drafting:
                DraftCard();
                break;
        }
    }


    public void UseCard() {
        if (CanUse()) {
            owner.playerChar.ChangeEnergy(-energyCost);
            if(cardAbility) cardAbility.StartCoroutine(cardAbility.UseAbility());
            RemoveFromHand();
        }
    }

    public void RemoveFromHand() {
        owner.hand.cards.Remove(this);
        transform.SetParent(owner.usedCards.transform);
        owner.usedCards.used.Add(this);
        owner.hand.FitHand();
        model.SetActive(false);
        col.enabled = false;
    }

    public void UpdateEnergyCost() {
        energyCostText.text = energyCost.ToString();
    }


    public bool CanUse() {
        if (owner.playerChar.energy >= energyCost && gameObject.activeSelf) {
            return true;
        }
        return false;
    }
}
