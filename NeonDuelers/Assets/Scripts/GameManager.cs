using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState { Drafting, Buying, Fighting }

public class GameManager : MonoBehaviour {
    public GameState gameState;
    public static GameManager Instance;
    public List<Player> players;
    public Player singlePlayer;
    public List<Card> allCards;
    public Transform cardsOffered;
    public int startMoney = 1000;
    public int draftRerolls = 20;
    public int draftOffers = 3;
    public float draftTime = 120f;
    public float roundTime = 120f;
    public TextMeshProUGUI roundTimer;
    public DraftText draftText;

    float roundTimeLeft = 120f;
    float draftTimeLeft = 120f;


    private void Awake() {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);

        players.Add(singlePlayer);
    }

    private void Start() {
        GiveStartMoney();
        DraftCardsForPlayers();
    }

    void GiveStartMoney() {
        foreach (Player player in players) {
            player.money = startMoney;
            player.moneyCounter.text = startMoney.ToString();
        }
    }

    public void DraftCardsForPlayers() {
        gameState = GameState.Drafting;
        EnemySpawner.Instance.waveBar.gameObject.SetActive(false);
        foreach (Player player in players) {
            player.rerolls = draftRerolls;
            OfferCards(player, draftOffers);
            player.playerChar.energyText.gameObject.SetActive(false);
        }
        draftText.Activate(true);
        StartCoroutine(DraftTimer());
    }

   public void OfferCards(Player player, int amount) {
        //Remove all offered cards
        DestroyOffers();

        if (player.rerolls > 0) {
            //Present new
            float spacing = 1.5f;
            for (int i = 0; i < amount; i++) {
                Card newCard = Instantiate(PickRandomCardFromAllCards(), new Vector3((cardsOffered.position.x - i + (amount / 2)) * spacing, cardsOffered.position.y, cardsOffered.position.z), Quaternion.identity, cardsOffered);
                newCard.ShowCost(true);
            }
            player.rerolls--;
        }
        else {
            Debug.Log("Done drafting!");
            players[0].playerState = PlayerState.Waiting;
            if (AllPlayersDoneDrafting()) GoToPlayState();
        }
    }

    public void DestroyOffers() {
        foreach (Card existingCard in cardsOffered.GetComponentsInChildren<Card>()) {
            Destroy(existingCard.gameObject);
        }
    }

    public void Skip() {
        OfferCards(players[0], draftOffers);
        draftText.UpdateCounter();
    }

    public Card PickRandomCardFromAllCards() {
        if (allCards.Count <= 0) return null;
        Card foundCard = allCards[Random.Range(0, allCards.Count)];
        return foundCard;
    }

    public void DraftCard(Card card) {
        allCards.Remove(card);
        AddCardToPlayer(card, players[0]);
        OfferCards(players[0], draftOffers);
        draftText.UpdateCounter();
    }

    public void AddCardToPlayer(Card card, Player player) {
        player.deck.Add(card);
        card.owner = player;
        card.owner.deck.UpdateCount();
        card.owner.UpdateMoney(-card.price);
        card.transform.SetParent(player.deck.transform);
        card.model.SetActive(false);
        card.col.enabled = false;
    }


    bool AllPlayersDoneDrafting() {
        foreach (Player player in players) {
            if (player.playerState == PlayerState.Drafting) {
                return false;
            }
        }
        return true;
    }

    public void GoToPlayState() {
        if (gameState == GameState.Fighting) return;
        gameState = GameState.Fighting;
        StopCoroutine(DraftTimer());
        DestroyOffers();
        draftText.Activate(false);
        EnemySpawner.Instance.waveBar.gameObject.SetActive(true);
        foreach (Player player in players) {
            player.StartPlaying();
        }
        EnemySpawner.Instance.StartSpawning();
        StartCoroutine(FightTimer());
    }

    IEnumerator DraftTimer() {
        draftTimeLeft = draftTime;
        while (draftTimeLeft > 0) {
            draftText.timer.text = draftTimeLeft.ToString();
            draftTimeLeft--;
            yield return new WaitForSeconds(1);
        }
        GoToPlayState();
    }


    IEnumerator FightTimer() {
        roundTimeLeft = roundTime;
        while (roundTimeLeft > 0) {
            roundTimer.text = roundTimeLeft.ToString();
            roundTimeLeft--;
            yield return new WaitForSeconds(1);
        }
        if (gameState == GameState.Fighting) {
            foreach (Player player in players)
                if (player.playerState == PlayerState.Farming) {
                    FarmingRoundEnds(player);
                }
        }
    }


    public void FarmingRoundEnds(Player loser) {
        gameState = GameState.Buying;
        EnemySpawner.Instance.StopAllCoroutines();
        EnemySpawner.Instance.waveBar.gameObject.SetActive(false);
        foreach (Enemy enemy in EnemySpawner.Instance.GetComponentsInChildren<Enemy>()) {
            Destroy(enemy.gameObject);
        }
        PlayArea.Instance.enemiesInArea.Clear();
        foreach (Player player in players) {
            player.SwitchToDraftState();
            }
        DraftCardsForPlayers();
    }
    
}
