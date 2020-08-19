using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GamePiece player1_Trainer;
    public GamePiece player1_TrainerPrefab;
    public GamePiece player2_Trainer;
    public GamePiece player2_TrainerPrefab;
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
    public bool isDragging;
    public bool sentIsNotDragging;
    public Button endTurnButton;
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
    public GameObject quitMenu;
    public GameObject endGameMenu;
    public GameObject winningPokemonList;
    public GameObject winningPokemonEntry;
    public TextMeshProUGUI endGameText;
    public bool gameOver;
    public int winner;
    public bool quitGame;
    public WeatherManager weatherManager;
    public DeckManager deckManager;

    // Events
    public AnnouncementEvents announcementEvents;
    public HighlightGamePieces highlightGamePieces;

    // Start is called before the first frame update
    void Awake()
    {
        // Deselected any remaining pieces
        enemiesHighlighted = false;

        // Prepare decks
        deckManager.player1_BattleDeck = new List<Card>();
        if (gameManager.deck3.Count == 30)
        {
            deckManager.player2_BattleDeck = gameManager.deck3;
        }

        // Subscribe to new events
        announcementEvents = GetComponent<AnnouncementEvents>();
        highlightGamePieces = GetComponent<HighlightGamePieces>();
        announcementEvents.OnAnnouncementStart += AccouncementEvents_OnAnnouncementStart;
        highlightGamePieces.OnHighlightGamePiece += HighlightGamePieces_OnHighlightGamePiece;
    }

    private void HighlightGamePieces_OnHighlightGamePiece(object sender, System.EventArgs e)
    {
        FindObjectOfType<GamePiece>().HighlightEnemies();
    }

    private void AccouncementEvents_OnAnnouncementStart(object sender, AnnouncementEvents.OnAnnouncementStartEventArgs e)
    {
        announcementEvents.announcementCounter++;

        if (announcementEvents.announcementCounter >= 140)
        {
            announcementEvents.bigAnnouncement.gameObject.SetActive(false);
            announcementEvents.smallAnnouncement.gameObject.SetActive(false);
        }
        AnnouncementEvents accouncementEvents = GetComponent<AnnouncementEvents>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update player stats
        player1_EnergyDisplay.text = player1_CurrentEnergy + "/" + player1_MaxEnergy;
        player2_EnergyDisplay.text = player2_CurrentEnergy + "/" + player2_MaxEnergy;

        if (sentIsNotDragging)
        {
            isDragging = false;
            sentIsNotDragging = false;
        }

        // Conversion display
        conversionDisplay.sprite = FindObjectOfType<MakeACard>().typeMatchups.typesImages[conversionIndex];

        // Start the initial card draw counter
        deckManager.GameStartCardDraw(3);

        // Enable/Disable conversion buttons
        if (conversionIndex == (FindObjectOfType<MakeACard>().types.Count - 1))
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
        deckManager.ShuffleDeck(deckManager.player1_BattleDeck);
        deckManager.ShuffleDeck(deckManager.player2_BattleDeck);

        // Variables set
        player1_MaxEnergy = 0;
        player2_MaxEnergy = 0;
        player1_CurrentEnergy = 0;
        player2_CurrentEnergy = 0;
        gameOver = false;
        winner = 0;
        if (gameManager.deck3.Count == 30)
        {
            // The user can create a deck for the computer player to play with if they create Deck 3.
            deckManager.player2_BattleDeck = gameManager.deck3;
        }
        player1_CurrentEnergy = 0;
        player2_CurrentEnergy = 0;
        Destroy(player1_Trainer);
        Destroy(player2_Trainer);
        GamePiece newPlayer1 = Instantiate(player1_TrainerPrefab, GameObject.Find("BattleUI").transform);
        GamePiece newPlayer2 = Instantiate(player2_TrainerPrefab, GameObject.Find("BattleUI").transform);
        newPlayer1.name = "Player1Trainer";
        newPlayer2.name = "Player2Trainer";
        player1_Trainer = GameObject.Find("Player1Trainer").GetComponent<GamePiece>();
        player2_Trainer = GameObject.Find("Player2Trainer").GetComponent<GamePiece>();
        player2_Trainer.transform.SetAsFirstSibling();
        player1_Trainer.transform.SetAsFirstSibling();
        player1_Hand.Clear();
        player2_Hand.Clear();
        player1_BattleField.Clear();
        player2_BattleField.Clear();
        endGameMenu.SetActive(false);
        winner = 0;
        quitGame = false;
        playerTurn = 0;
        artificialIntelligence.physicalCardList.Clear();
        endTurnButton.interactable = false;
        announcementEvents.bigAnnouncement.gameObject.SetActive(false);
        announcementEvents.smallAnnouncement.gameObject.SetActive(false);
        weatherManager.weather = WeatherManager.Weather.Clear;
        deckManager.cardDrawCounter = 0;
        deckManager.cardDrawBool = true;
    }

    public void PlayerTurnStart()
    {
        if (playerTurn == 2)
        {
            // Player's turn
            announcementEvents.bigAnnouncement.gameObject.SetActive(true);
            announcementEvents.bigAnnouncement.text = "Your turn";
            endTurnButton.interactable = true;
            playerTurn = 1;
            if (player1_MaxEnergy < 9)
            {
                player1_MaxEnergy++;
            }
            player1_CurrentEnergy = player1_MaxEnergy;
            deckManager.DrawCard(1);
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
            announcementEvents.bigAnnouncement.gameObject.SetActive(true);
            announcementEvents.bigAnnouncement.text = "Opponent's turn";
            endTurnButton.interactable = false;
            playerTurn = 2;
            if (player2_MaxEnergy < 9)
            {
                player2_MaxEnergy++;
            }
            player2_CurrentEnergy = player2_MaxEnergy;
            deckManager.DrawCard(2);
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

    public void QuitButton()
    {
        Time.timeScale = 0;
        quitMenu.SetActive(true);
    }

    public void QuitNoButton()
    {
        Time.timeScale = 1;
        quitMenu.SetActive(false);
    }

    public void QuitYesButton()
    {
        quitGame = true;
        Time.timeScale = 1;
        quitMenu.SetActive(false);
        gameManager.mainMenu.SetActive(true);
        gameManager.menuPanel.SetActive(true);
        gameManager.menuPanel.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void BattleOver()
    {
        endGameMenu.SetActive(true);
        if (winner == 2)
        {
            // You lose!
            endGameText.text = "You lose!";
            for (int i = 0; i < player2_BattleField.Count; i++)
            {
                GameObject winningPokemon = Instantiate(winningPokemonEntry, winningPokemonList.transform);
                winningPokemon.GetComponent<WinningPokemonDisplay>().card = player2_BattleField[i].card;
            }
        }
        else if (winner == 1)
        {
            // You win!
            endGameText.text = "You win!";
            for (int i = 0; i < player1_BattleField.Count; i++)
            {
                GameObject winningPokemon = Instantiate(winningPokemonEntry, winningPokemonList.transform);
                winningPokemon.GetComponent<WinningPokemonDisplay>().card = player1_BattleField[i].card;
            }
        }
    }
}