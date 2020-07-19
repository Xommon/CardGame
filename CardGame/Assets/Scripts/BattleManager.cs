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
    public TextMeshProUGUI player1_EnergyDisplay;
    public TextMeshProUGUI player2_EnergyDisplay;
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
    public GameObject abilityOverlay;
    public GameObject abilityOverlay2;
    public GameObject conversionPrompt;
    public bool abilityMode;
    public string abilityModeAbility;
    public int conversionIndex;
    public Image conversionDisplay;
    public Button conversionPlus;
    public Button conversionMinus;
    public bool enemiesHighlighted;

    // Start is called before the first frame update
    void Start()
    {
        enemiesHighlighted = false;

        // Prepare decks
        player1_BattleDeck = new List<Card>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update player stats
        player1_EnergyDisplay.text = player1_CurrentEnergy + "/" + player1_MaxEnergy;
        player2_EnergyDisplay.text = player2_CurrentEnergy + "/" + player2_MaxEnergy;

        if (Input.GetKeyDown("m"))
        {
            Destroy(artificialIntelligence.physicalCardList[artificialIntelligence.physicalCardList.Count].gameObject);
            artificialIntelligence.physicalCardList.RemoveAt(artificialIntelligence.physicalCardList.Count);
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

        // Conversion display
        conversionDisplay.sprite = FindObjectOfType<MakeACard>().typeSprites[conversionIndex];

        // Start the initial card draw counter
        if (cardDrawBool)
        {
            cardDrawCounter++;
        }

        if (cardDrawCounter == 60)
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
        else if (cardDrawCounter >= 150)
        {
            cardDrawCounter = 0;
            cardDrawBool = false;

            // Determine who plays first
            playerTurn = Random.Range(1, 3);
            PlayerTurnStart();
        }

        // Enable/Disable conversion buttons
        if (conversionIndex == 17)
        {
            conversionPlus.interactable = false;
        }
        else
        {
            conversionPlus.interactable = true;
        }
        if (conversionIndex == 0)
        {
            conversionMinus.interactable = false;
        }
        else
        {
            conversionMinus.interactable = true;
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
        int multiplierAttacker = attacker.currentAttack;
        int multiplierDefender = defender.currentAttack;
        if (attacker.name != "Player1Trainer" && defender.name != "Player1Trainer" && attacker.name != "Player2Trainer" && defender.name != "Player2Trainer")
        if (attacker.weakness == defender.type)
        {
            multiplierDefender = defender.currentAttack * 2;
        }
        else if (attacker.resistance == defender.type)
        {
            multiplierDefender = Mathf.RoundToInt(defender.currentAttack / 2);
        }
        if (defender.weakness == attacker.type)
        {
            multiplierAttacker = attacker.currentAttack * 2;
        }
        else if (defender.resistance == attacker.type)
        {
            multiplierAttacker = Mathf.RoundToInt(attacker.currentAttack / 2);
        }

        if (attacker.player != defender.player && attacker.canAttack)
        {
            if ((playerTurn == 1 && !attackInProgress) || playerTurn == 2)
            {
                attacker.HighlightEnemies();
                attacker.counter = 0;
                defender.counter = 0;
                attackInProgress = true;
                attacker.canAttack = false;
                attacker.isSelected = false;
                defender.isSelected = false;
                selectedGamePiece = null;
                attacker.damageEffect.SetActive(true);
                attacker.damageEffect.GetComponent<Animator>().enabled = true;
                attacker.damaged = true;
                defender.damageEffect.SetActive(true);
                defender.damageEffect.GetComponent<Animator>().enabled = true;
                defender.damaged = true;
                if (defender.name != "Player1Trainer" && defender.name != "Player2Trainer")
                {
                    if (attacker.toxic)
                    {
                        if (defender.shielded)
                        {
                            attacker.currentHealth -= multiplierDefender;
                            attacker.damageDisplay.text = "-" + multiplierDefender;
                            defender.shielded = false;
                        }
                        else
                        {
                            defender.currentHealth -= defender.currentHealth;
                            defender.damageDisplay.text = "Toxic";
                            if (defender.toxic)
                            {
                                attacker.currentHealth -= attacker.currentHealth;
                                attacker.damageDisplay.text = "Toxic";
                            }
                            else
                            {
                                attacker.currentHealth -= multiplierDefender;
                                attacker.damageDisplay.text = "-" + multiplierDefender;
                            }
                        }
                    }
                    else if (defender.toxic)
                    {
                        attacker.currentHealth -= attacker.currentHealth;
                        attacker.damageDisplay.text = "Toxic";
                        if (attacker.toxic)
                        {
                            defender.currentHealth -= defender.currentHealth;
                            defender.damageDisplay.text = "Toxic";
                        }
                        else
                        {
                            defender.currentHealth -= multiplierAttacker;
                            defender.damageDisplay.text = "-" + multiplierAttacker;
                        }
                    }
                    else
                    {
                        if (attacker.shielded)
                        {
                            attacker.damageDisplay.text = "Shield";
                            defender.currentHealth -= multiplierAttacker;
                            defender.damageDisplay.text = "-" + multiplierAttacker;
                            if (defender.attack > 0)
                            {
                                attacker.shielded = false;
                            }
                        }
                        else if (defender.shielded)
                        {
                            defender.damageDisplay.text = "Shield";
                            attacker.currentHealth -= multiplierDefender;
                            attacker.damageDisplay.text = "-" + multiplierDefender;
                            if (attacker.attack > 0)
                            {
                                defender.shielded = false;
                            }
                        }
                        else
                        {
                            attacker.currentHealth -= multiplierDefender;
                            attacker.damageDisplay.text = "-" + multiplierDefender;
                            defender.currentHealth -= multiplierAttacker;
                            defender.damageDisplay.text = "-" + multiplierAttacker;
                        }
                    }
                }
                else
                {
                    attacker.currentHealth -= multiplierDefender;
                    attacker.damageDisplay.text = "-" + multiplierDefender;
                    defender.currentHealth -= multiplierAttacker;
                    defender.damageDisplay.text = "-" + multiplierAttacker;
                }
            }
        }
    }

    public void Damage(GamePiece defender, int damageAmount)
    {
        if (defender.shielded)
        {
            defender.damageDisplay.text = "Shield";
            defender.shielded = false;
        }
        else
        {
            defender.currentHealth -= damageAmount;
            defender.damageDisplay.text = "-" + damageAmount;
        }
        defender.counter = 0;
        attackInProgress = true;
        defender.isSelected = false;
        selectedGamePiece = null;
        defender.damageEffect.SetActive(true);
        defender.damageEffect.GetComponent<Animator>().enabled = true;
        defender.damaged = true;
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
            if (player1_MaxEnergy < 9)
            {
                player1_MaxEnergy++;
            }
            player1_CurrentEnergy = player1_MaxEnergy;
            DrawCard(1);
            if (player1_BattleField.Count > 0)
            {
                for (int i = 0; i < player1_BattleField.Count; i++)
                {
                    if (player1_BattleField[i].paralysed)
                    {
                        player1_BattleField[i].canAttack = false;
                        player1_BattleField[i].paralysed = false;
                    }
                    else
                    {
                        player1_BattleField[i].canAttack = true;
                    }
                }
            }
        }
        else if (playerTurn == 1)
        {
            // Opponent's turn
            artificialIntelligence.on = true;
            artificialIntelligence.phase = ArtificialIntelligence.Phase.Waiting;
            announcementCounter = 0;
            bigAnnouncement.gameObject.SetActive(true);
            bigAnnouncement.text = "Opponent's turn";
            endTurnButton.interactable = false;
            playerTurn = 2;
            if (player2_MaxEnergy < 9)
            {
                player2_MaxEnergy++;
            }
            player2_CurrentEnergy = player2_MaxEnergy;
            DrawCard(2);
            if (player2_BattleField.Count > 0)
            {
                for (int i = 0; i < player2_BattleField.Count; i++)
                {
                    if (player2_BattleField[i].paralysed)
                    {
                        player2_BattleField[i].canAttack = false;
                        player2_BattleField[i].paralysed = false;
                    }
                    else
                    {
                        player2_BattleField[i].canAttack = true;
                    }
                }
            }

            // Unhighlight enemies
            if (enemiesHighlighted)
            {
                FindObjectOfType<GamePiece>().HighlightEnemies();
            }
        }
    }

    public void AITurnEnd()
    {
        Debug.Log("The AI has ended their turn.");
        artificialIntelligence.thinkCounter = 0;
        PlayerTurnStart();
        artificialIntelligence.on = false;
        artificialIntelligence.possiblePlays.Clear();
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

    public void ConvertPlusButton()
    {
        conversionIndex += 1;
    }

    public void ConvertMinusButton()
    {
        conversionIndex -= 1;
    }

    public void ConvertButton()
    {
        selectedGamePiece.type = FindObjectOfType<MakeACard>().types[conversionIndex];
        conversionPrompt.SetActive(false);
        abilityOverlay.SetActive(false);
        abilityMode = false;
        abilityModeAbility = "";
    }
}