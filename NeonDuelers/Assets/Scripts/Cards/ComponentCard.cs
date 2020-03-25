using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentCard : Card
{
    [SerializeField]
    public PermanentStatBoost[] permanentStatBoosts;

    public void OnMouseDown() {
        switch (GameManager.Instance.players[0].playerState) {
            case PlayerState.Drafting:
                DraftCard();
                break;
        }
    }

    public void DraftCard() {
        if (GameManager.Instance.players[0].money >= price) {
            owner = GameManager.Instance.players[0];
            AddStatsToChar(owner.playerChar);
            owner.UpdateMoney(-price);
            GameManager.Instance.allCards.Remove(this);
            GameManager.Instance.OfferCards(GameManager.Instance.players[0], GameManager.Instance.draftOffers);
            GameManager.Instance.draftText.UpdateCounter();
            Destroy(gameObject);

        }
        else Debug.LogWarning("Not enough money to buy!");
    }

    public void AddStatsToChar(PlayerCharacter character) {
        foreach (PermanentStatBoost permanentStatBoost in permanentStatBoosts) {
            permanentStatBoost.ApplyBoostToPlayerChar(character);
        }
    }
}
