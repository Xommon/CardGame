using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public List<Card> player1_BattleDeck;
    public List<Card> player2_BattleDeck;
    public List<Card> tempDeck;
    public List<Card> player1_Hand;
    public List<Card> player2_Hand;
    public List<Card> player1_DiscardPile;
    public List<Card> player2_DiscardPile;
    public int player1_Health;
    public int player2_Health;
    public int player1_MaxEnergy;
    public int player2_MaxEnergy;
    public int player1_CurrentEnergy;
    public int player2_CurrentEnergy;
    public TextMesh player1_EnergyDisplay;
    public TextMesh player2_EnergyDisplay;
    public TextMesh player1_HealthDisplay;
    public TextMesh player2_HealthDisplay;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Prepare decks
        player1_BattleDeck = new List<Card>();
        //player2_BattleDeck = new List<Card>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update player stats
        player1_EnergyDisplay.text = player1_CurrentEnergy + "/" + player1_MaxEnergy;
        player2_EnergyDisplay.text = player2_CurrentEnergy + "/" + player2_MaxEnergy;
        player1_HealthDisplay.text = player1_Health.ToString();
        player2_HealthDisplay.text = player2_Health.ToString();
    }

    public void BattleStart()
    {
        // Shuffle the decks
        ShuffleDeck(player1_BattleDeck);
        ShuffleDeck(player2_BattleDeck);

        // Variables set
        player1_Health = 30;
        player2_Health = 30;
        player1_MaxEnergy = 1;
        player2_MaxEnergy = 1;
        player1_CurrentEnergy = 1;
        player2_CurrentEnergy = 1;
    }

    public void BattleEnd()
    {

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
}
