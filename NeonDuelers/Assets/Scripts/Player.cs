using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum PlayerState { Drafting, Waiting, Buying, Farming, Dueling }

public class Player : MonoBehaviour {

    public PlayerState playerState;
    public Deck deck;
    public Hand hand;
    public UsedCards usedCards;
    public WeaponCard weaponCard;
    public ArmorCard armorCard;
    public PlayerCharacter playerChar;
    public float drawInterval = 6f;
    public float drawSpeedMultiplier = 1f;

    public int money;
    public TextMeshProUGUI moneyCounter;
    public TextMeshPro deckCounter;
    public Slider cardSlider;

    public int rerolls;
    public int startDrawSize = 3;
    private Coroutine drawPassively;

    public void ShowHand() {
        //Draw 3 cards from deck to hand
        DrawCards(startDrawSize);
    }

    public void DrawCards(int amount) {
        for (int i = 0; i < amount; i++) {
            if (deck.cards.Count == 0) {
                //Deck empty!
                Debug.LogWarning("Deck empty!");
                return;
            }
            //pick random card
            Card newCard = deck.cards[Random.Range(0, deck.cards.Count)];
            hand.cards.Add(newCard);
            deck.cards.Remove(newCard);
            deck.UpdateCount();

            //enable it
            newCard.model.SetActive(true);
            newCard.col.enabled = true;
            newCard.ShowCost(false);

            //move it to hand parent
            newCard.transform.SetParent(hand.transform);

            //fit to hand size
            hand.FitHand();
        }

    }
    public void UpdateMoney(int amount) {
        money += amount;
        moneyCounter.text = money.ToString();
    }

    public IEnumerator DrawPassively() {
        cardSlider.transform.parent.gameObject.SetActive(true);
        float timeLeft = drawInterval;
        cardSlider.maxValue = drawInterval;
        while (playerState == PlayerState.Farming || playerState == PlayerState.Dueling) {
            while (timeLeft > 0) {
                timeLeft -= (Time.deltaTime * drawSpeedMultiplier);
                cardSlider.normalizedValue = 1 - (timeLeft / drawInterval);
                yield return new WaitForEndOfFrame();
            }
            DrawCards(1);
            timeLeft = drawInterval;
            yield return null;
        }
    }

    public void StartPlaying() {
        deck.SetDeck();
        ShowHand();
        ResetEnergyHealth();
        playerState = PlayerState.Farming;
        drawPassively = StartCoroutine(DrawPassively());
        playerChar.energyText.gameObject.SetActive(true);
        playerChar.StartCoroutine(playerChar.EnergyRegen());
    }

    public void SwitchToDraftState() {
        playerChar.ResetPosition();
        hand.ResetHand();
        usedCards.ReturnToDeck();
        deck.ResetDeck();
        StopCoroutine(drawPassively);
        playerState = PlayerState.Drafting;
    }

    public void ResetEnergyHealth() {
        playerChar.health = playerChar.maxHealth;
        playerChar.healthBar.SetHealth(playerChar.health);
        playerChar.energy = playerChar.maxEnergy;
        playerChar.UpdateEnergyText();
    }
}
