using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GamePiece player1_Trainer;
    public GamePiece player2_Trainer;
    public List<Card> player1_BattleDeck;
    public List<Card> player2_BattleDeck;
    public List<Card> tempDeck;
    public List<Card> player1_Hand;
    public List<Card> player2_Hand;
    public List<Card> player1_DiscardPile;
    public List<Card> player2_DiscardPile;
    public List<GamePiece> player1_BattleField;
    public List<GamePiece> player2_BattleField;
    public GameObject player1_BattleFieldObject;
    public GameObject player2_BattleFieldObject;
    public int player1_MaxEnergy;
    public int player2_MaxEnergy;
    public int player1_CurrentEnergy;
    public int player2_CurrentEnergy;
    public TextMesh player1_EnergyDisplay;
    public TextMesh player2_EnergyDisplay;
    public TextMeshProUGUI player1_HealthDisplay;
    public TextMeshProUGUI player2_HealthDisplay;
    public GameManager gameManager;
    public int playerTurn;
    public GamePiece selectedGamePiece;
    public GameObject placeHolder;
    public GameObject card;
    public GameObject opponentCard;
    public GameObject player1_HandObject;
    public GameObject player2_HandObject;
    public bool attackInProgress;
    public bool isDragging;
    public bool sentIsNotDragging;
    public TextMeshProUGUI bigAnnouncement;
    public TextMeshProUGUI smallAnnouncement;
    public int announcementCounter;
    public Button endTurnButton;
    public int cardDrawCounter;
    public bool cardDrawBool;
    public ArtificialIntelligence artificialIntelligence;

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

        if (Input.GetKeyDown("m"))
        {
            DrawCard(1);
            DrawCard(2);
        }

        if (sentIsNotDragging)
        {
            isDragging = false;
            sentIsNotDragging = false;
        }

        if (bigAnnouncement.gameObject.activeInHierarchy || smallAnnouncement.gameObject.activeInHierarchy)
        {
            announcementCounter++;

            if (announcementCounter >= 120)
            {
                bigAnnouncement.gameObject.SetActive(false);
                smallAnnouncement.gameObject.SetActive(false);
            }
        }

        // Start the initial card draw counter
        if (cardDrawBool)
        {
            cardDrawCounter++;
        }

        if (cardDrawCounter == 30)
        {
            DrawCard(1);
            DrawCard(2);
        }
        else if (cardDrawCounter == 60)
        {
            DrawCard(1);
            DrawCard(2);
        }
        else if (cardDrawCounter == 90)
        {
            DrawCard(1);
            DrawCard(2);
        }
        else if (cardDrawCounter == 120)
        {
            DrawCard(1);
            DrawCard(2);
        }
        else if (cardDrawCounter == 150)
        {
            DrawCard(1);
            DrawCard(2);
        }
        else if (cardDrawCounter >= 180)
        {
            cardDrawCounter = 0;
            cardDrawBool = false;

            // Determine who plays first
            if (Random.Range(1, 3) == 1)
            {
                playerTurn = 2;
                PlayerTurnStart();
            }
            else
            {
                playerTurn = 1;
                artificialIntelligence.on = true;
                PlayerTurnStart();
            }
        }
    }

    public void BattleStart()
    {
        // Shuffle the decks
        ShuffleDeck(player1_BattleDeck);
        ShuffleDeck(player2_BattleDeck);

        // Variables set
        player1_MaxEnergy = 0;
        player2_MaxEnergy = 0;
        player1_CurrentEnergy = 0;
        player2_CurrentEnergy = 0;

        // Each player draws 5 cards
        cardDrawCounter = 0;
        cardDrawBool = true;
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

    public void Attack(GamePiece attacker, GamePiece defender)
    {
        if (attacker.player != defender.player && !attackInProgress)
        {
            attacker.counter = 0;
            defender.counter = 0;
            attackInProgress = true;
            defender.currentHealth -= attacker.currentAttack;
            attacker.currentHealth -= defender.currentAttack;
            attacker.canAttack = false;
            attacker.isSelected = false;
            defender.isSelected = false;
            selectedGamePiece = null;
            attacker.damageDisplay.text = "-" + defender.currentAttack;
            attacker.damageEffect.SetActive(true);
            attacker.damageEffect.GetComponent<Animator>().enabled = true;
            attacker.damaged = true;
            defender.damageDisplay.text = "-" + attacker.currentAttack;
            defender.damageEffect.SetActive(true);
            defender.damageEffect.GetComponent<Animator>().enabled = true;
            defender.damaged = true;
        }
    }

    public void DrawCard(int player)
    {
        if (player == 1)
        {
            if (player1_BattleDeck.Count > 0)
            {
                player1_Hand.Add(player1_BattleDeck[0]);
                GameObject drawnCard = Instantiate(card, player1_HandObject.transform);
                drawnCard.GetComponent<CardFace>().card = player1_BattleDeck[0];
                player1_BattleDeck.RemoveAt(0);
            }
            else
            {
                Debug.Log("Out of cards!");
            }
        }
        else
        {
            if (player2_BattleDeck.Count > 0)
            {
                player2_Hand.Add(player2_BattleDeck[0]);
                GameObject drawnCard = Instantiate(opponentCard, player2_HandObject.transform);
                drawnCard.GetComponent<CardFace>().card = player2_BattleDeck[0];
                player2_BattleDeck.RemoveAt(0);
                artificialIntelligence.physicalCardList.Add(drawnCard);
            }
        }
    }

    public void PlayerTurnStart()
    {
        if (playerTurn == 2)
        {
            // Player's turn
            announcementCounter = 0;
            bigAnnouncement.gameObject.SetActive(true);
            bigAnnouncement.text = "Your turn";
            endTurnButton.interactable = true;
            playerTurn = 1;
            player1_MaxEnergy++;
            player1_CurrentEnergy = player1_MaxEnergy;
            DrawCard(1);
            if (player1_BattleField.Count > 0)
            {
                for (int i = 0; i < player1_BattleField.Count; i++)
                {
                    player1_BattleField[i].canAttack = true;
                }
            }
        }
        else if (playerTurn == 1)
        {
            // Opponent's turn
            artificialIntelligence.phase = ArtificialIntelligence.Phase.Waiting;
            announcementCounter = 0;
            bigAnnouncement.gameObject.SetActive(true);
            bigAnnouncement.text = "Opponent's turn";
            endTurnButton.interactable = false;
            playerTurn = 2;
            player2_MaxEnergy++;
            player2_CurrentEnergy = player2_MaxEnergy;
            DrawCard(2);
            if (player2_BattleField.Count > 0)
            {
                for (int i = 0; i < player2_BattleField.Count; i++)
                {
                    player2_BattleField[i].canAttack = true;
                }
            }
        }
    }

    public void AITurnEnd()
    {
        Debug.Log("The AI has ended their turn.");
        artificialIntelligence.thinkCounter = 0;
        PlayerTurnStart();
        artificialIntelligence.on = false;
    }

    public void PlayerTurnEnd()
    {
        // Unselect all game pieces
        for (int i = 0; i < player1_BattleField.Count; i++)
        {
            player1_BattleField[i].isSelected = false;
            player1_BattleField[i].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            selectedGamePiece = null;
        }
        artificialIntelligence.on = true;
        PlayerTurnStart();
    }
}
