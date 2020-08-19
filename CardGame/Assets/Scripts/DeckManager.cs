using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> player1_BattleDeck;
    public List<Card> player2_BattleDeck;
    public List<Card> tempDeck;
    public int cardDrawCounter;
    public bool cardDrawBool;
    public BattleManager battleManager;
    public ArtificialIntelligence artificialIntelligence;
    public AnnouncementEvents announcementEvents;

    // Start is called before the first frame update
    void Start()
    {
        battleManager = GetComponent<BattleManager>();
        artificialIntelligence = GetComponent<ArtificialIntelligence>();
        announcementEvents = GetComponent<AnnouncementEvents>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStartCardDraw(int startingCardsAmount)
    {
        // Populate each players hand with 3 cards to start
        if (cardDrawBool)
        {
            cardDrawCounter++;
        }

        if (cardDrawBool && cardDrawCounter <= 210 && (cardDrawCounter - 60) % 30 == 0 && battleManager.player1_Hand.Count < startingCardsAmount && battleManager.player2_Hand.Count < startingCardsAmount)
        {
            DrawCard(1);
            DrawCard(2);
        }
        else if (cardDrawBool && battleManager.player1_Hand.Count >= 3 && battleManager.player2_Hand.Count >= 3)
        {
            cardDrawCounter = 0;
            cardDrawBool = false;

            // Determine who plays first
            battleManager.playerTurn = Random.Range(1, 3);
            battleManager.PlayerTurnStart();
        }
    }

    public void ShuffleDeck(List<Card> deck)
    {
        // Build a temp deck to house the battle deck
        tempDeck = new List<Card>();
        for (int i = 0; i < 30; i++)
        {
            tempDeck.Add(deck[i]);
        }

        // Clear out the battle deck
        deck.Clear();

        // Reimport the cards randomly
        for (int i = 0; i < 30; i++)
        {
            Card cardToImport = tempDeck[Random.Range(0, tempDeck.Count)];
            deck.Add(cardToImport);
            tempDeck.Remove(cardToImport);
        }

        // Clear out the temp deck
        tempDeck.Clear();
    }

    public void DrawCard(int player)
    {
        if (player == 1 && battleManager.player1_Hand.Count < 10 && player1_BattleDeck.Count > 0)
        {
            battleManager.player1_Hand.Add(player1_BattleDeck[0]);
            GameObject drawnCard = Instantiate(battleManager.card, battleManager.player1_HandObject.transform);
            drawnCard.GetComponent<CardFace>().card = player1_BattleDeck[0];
            player1_BattleDeck.RemoveAt(0);
        }
        else if (player == 2 && battleManager.player2_Hand.Count < 10 && player2_BattleDeck.Count > 0)
        {
            battleManager.player2_Hand.Add(player2_BattleDeck[0]);
            GameObject drawnCard = Instantiate(battleManager.opponentCard, battleManager.player2_HandObject.transform);
            drawnCard.GetComponent<CardFace>().card = player2_BattleDeck[0];
            player2_BattleDeck.RemoveAt(0);
            artificialIntelligence.physicalCardList.Add(drawnCard);
        }
        else
        {
            announcementEvents.smallAnnouncement.gameObject.SetActive(true);

            if (player == 1 && battleManager.player1_Hand.Count >= 10)
            {
                announcementEvents.smallAnnouncement.text = "Your hand is too full to draw a card!";
                player1_BattleDeck.RemoveAt(0);
            }
            else if (player == 2 && battleManager.player2_Hand.Count >= 10)
            {
                announcementEvents.smallAnnouncement.text = "Your opponent's hand is too full to draw a card!";
                player2_BattleDeck.RemoveAt(0);
            }
        }
    }
}
