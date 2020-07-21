using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.IMGUI;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Decks and Cards
    public List<Card> allCards = new List<Card>();
    public string deck1Name;
    public string deck2Name;
    public string deck3Name;
    public List<Card> deck1 = new List<Card>();
    public List<Card> deck2 = new List<Card>();
    public List<Card> deck3 = new List<Card>();
    public List<Card> aiDeck = new List<Card>();
    public List<Card> tempDeck = new List<Card>();
    public Card newCustomCard;
    public int currentDeck;
    public int editingDeck;

    // Menus
    public GameObject menuPanel;
    public GameObject mainMenu;
    public GameObject deckMenu;
    public GameObject createADeckMenu;
    public GameObject makeACardMenu;
    public GameObject pokemonEntry;

    // Submenu Items
    public GameObject deckBackButton;
    //public GameObject deckEditButton;
    public GameObject createADeckCancelButton;
    public GameObject createADeckSaveButton;
    public GameObject createADeckScrollArea;
    public GameObject createADeckAmountOfCardsDisplay;
    public TextMeshProUGUI createADeck_DeckNameEntry;
    public TextMeshProUGUI deckManager_Deck1Name;
    public TextMeshProUGUI deckManager_Deck2Name;
    public TextMeshProUGUI deckManager_Deck3Name;
    public GameObject deckManager_Deck1Cardback;
    public GameObject deckManager_Deck2Cardback;
    public GameObject deckManager_Deck3Cardback;
    public Button deckManager_Deck1EditButton;
    public Button deckManager_Deck2EditButton;
    public Button deckManager_Deck3EditButton;
    public GameObject deckManager_Deck1CurrentDeck;
    public GameObject deckManager_Deck2CurrentDeck;
    public GameObject deckManager_Deck3CurrentDeck;
    public GameObject deckManager_BackButton;
    public Button makeACardButton;
    public Button createACardButton;

    // Scripts
    public BattleManager battleManager;

    // Start is called before the first frame update
    void Start()
    {
        // Enable panel
        menuPanel.GetComponent<CanvasGroup>().alpha = 1;

        // Select current deck
        currentDeck = 1;

        // If no decks have been created, then it's the player's first time playing. Open the deck creation menu.
        if (deck1.Count == 0 && deck2.Count == 0 && deck3.Count == 0)
        {
            deckMenu.SetActive(true);
            deckBackButton.SetActive(false);
            //deckEditButton.SetActive(false);
        }
        else // Otherwise, open the main menu
        {
            mainMenu.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Fadeout panel
        if (!mainMenu.activeInHierarchy && !deckMenu.activeInHierarchy && !createADeckMenu.activeInHierarchy && !makeACardMenu.activeInHierarchy)
        {
            menuPanel.GetComponent<CanvasGroup>().alpha -= 0.05f;
        }
        if (menuPanel.GetComponent<CanvasGroup>().alpha <= 0)
        {
            menuPanel.GetComponent<CanvasGroup>().alpha = 0;
            menuPanel.SetActive(false);
        }
        else
        {
            menuPanel.SetActive(true);
        }

        // Disable the save button in Create-a-Deck mode until the deck has 30 cards and has a name
        if (createADeckMenu.activeInHierarchy == true)
        {
            if (tempDeck.Count != 30 || createADeck_DeckNameEntry.text.Length < 2)
            {
                createADeckSaveButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                createADeckSaveButton.GetComponent<Button>().interactable = true;
            }
        }

        // Update the amount of cards in the deck
        createADeckAmountOfCardsDisplay.GetComponent<TextMeshProUGUI>().text = tempDeck.Count + "/30";

        // Show current deck if deck manager window is open
        if (deckMenu.activeInHierarchy)
        {
            if (currentDeck == 2)
            {
                deckManager_Deck1CurrentDeck.SetActive(false);
                deckManager_Deck2CurrentDeck.SetActive(true);
                deckManager_Deck3CurrentDeck.SetActive(false);
            }
            else if (currentDeck == 3)
            {
                deckManager_Deck1CurrentDeck.SetActive(false);
                deckManager_Deck2CurrentDeck.SetActive(false);
                deckManager_Deck3CurrentDeck.SetActive(true);
            }
            else
            {
                deckManager_Deck1CurrentDeck.SetActive(true);
                deckManager_Deck2CurrentDeck.SetActive(false);
                deckManager_Deck3CurrentDeck.SetActive(false);
            }
        }

        // If a deck exists, set up all of the display options
        if (deckMenu.activeInHierarchy)
        {
            if (deck1.Count != 30)
            {
                deckManager_Deck1Cardback.SetActive(false);
                deckManager_Deck1EditButton.interactable = false;
                deckManager_Deck1Name.text = "Create New Deck";
            }
            else
            {
                deckManager_Deck1Cardback.SetActive(true);
                deckManager_Deck1EditButton.interactable = true;
                deckManager_Deck1Name.text = deck1Name;
            }

            if (deck2.Count != 30)
            {
                deckManager_Deck2Cardback.SetActive(false);
                deckManager_Deck2EditButton.interactable = false;
                deckManager_Deck2Name.text = "Create New Deck";
            }
            else
            {
                deckManager_Deck2Cardback.SetActive(true);
                deckManager_Deck2EditButton.interactable = true;
                deckManager_Deck2Name.text = deck2Name;
            }

            if (deck3.Count != 30)
            {
                deckManager_Deck3Cardback.SetActive(false);
                deckManager_Deck3EditButton.interactable = false;
                deckManager_Deck3Name.text = "Create New Deck";
            }
            else
            {
                deckManager_Deck3Cardback.SetActive(true);
                deckManager_Deck3EditButton.interactable = true;
                deckManager_Deck3Name.text = deck3Name;
            }
        }

        // Disable CreateACard button if there have already been 3 cards made
        if (allCards.Count >= 254)
        {
            createACardButton.interactable = false;
        }

        // Update the length of the scroll area
        createADeckScrollArea.GetComponent<RectTransform>().sizeDelta = new Vector2(-200, allCards.Count * 60f);

        // DEBUG
        if (Input.GetKeyDown("z"))
        {
            EnergyCostTally(tempDeck);
        }
        if (Input.GetKeyDown("x"))
        {
            AllCardTypeTally();
        }
    }

    public void ClearDeckDisplay()
    {
        // Clear past card displays
        for (int i = 0; i < allCards.Count - 1; i++)
        {
            GameObject.Find("PokemonEntry (" + i + ")").gameObject.GetComponent<PokemonEntry>().currentQuantity = 0;
        }
        tempDeck.Clear();
    }

    public void SelectDeck1()
    {
        // If deck is brand new, create it. Otherwise, select it.
        if (deck1.Count == 30)
        {
            currentDeck = 1;
        }
        else
        {
            CreateDeck1();
        }
    }

    public void SelectDeck2()
    {
        // If deck is brand new, create it. Otherwise, select it.
        if (deck2.Count == 30)
        {
            currentDeck = 2;
        }
        else
        {
            CreateDeck2();
        }
    }

    public void SelectDeck3()
    {
        // If deck is brand new, create it. Otherwise, select it.
        if (deck3.Count == 30)
        {
            currentDeck = 3;
        }
        else
        {
            CreateDeck3();
        }
    }

    public void CreateDeck1()
    {
        // Open the Create-a-Deck window
        deckMenu.SetActive(false);
        createADeckMenu.SetActive(true);

        // Find Object index


        // Set Editing Deck
        editingDeck = 1;

        // Clear past card displays
        ClearDeckDisplay();

        // Load current deck if it has already been made
        if (deck1.Count == 30)
        {
            // Load individual cards
            for (int i = 0; i < 30; i++)
            {
                GameObject.Find("PokemonEntry (" + (deck1[i].dexNumber - 1) + ")").gameObject.GetComponent<PokemonEntry>().currentQuantity += 1;
            }

            // Convert real deck to temp deck until the player is ready to save
            tempDeck.Clear();
            for (int i = 0; i < 30; i++)
            {
                tempDeck.Add(deck1[i]);
            }
        }

        // Set up deck name
        if (deck1.Count == 30)
        {
            createADeck_DeckNameEntry.text = deck1Name;
        }
        else
        {
            createADeck_DeckNameEntry.text = "";
        }

        // Disable the cancel button if this is the first deck being made
        if (deck1.Count == 0 && deck2.Count == 0 && deck3.Count == 0)
        {
            createADeckCancelButton.GetComponent<Button>().interactable = false;
        }

        PopulateCardDatabase();
    }

    public void CreateDeck2()
    {
        // Set Editing Deck
        editingDeck = 2;

        // Open the Create-a-Deck window
        deckMenu.SetActive(false);
        createADeckMenu.SetActive(true);

        // Clear past card displays
        ClearDeckDisplay();

        // Load current deck if it has already been made
        if (deck2.Count == 30)
        {
            // Load individual cards
            for (int i = 0; i < 30; i++)
            {
                createADeckMenu.transform.Find("/AllCards/Scroll View/Viewport/ScrollArea/PokemonEntry (" + (deck2[i].dexNumber - 1) + ")").gameObject.GetComponent<PokemonEntry>().currentQuantity += 1;
            }

            // Convert real deck to temp deck until the player is ready to save
            tempDeck.Clear();
            for (int i = 0; i < 30; i++)
            {
                tempDeck.Add(deck2[i]);
            }
        }

        // Set up deck name
        if (deck2.Count == 30)
        {
            createADeck_DeckNameEntry.text = deck2Name;
        }
        else
        {
            createADeck_DeckNameEntry.text = "";
        }

        // Disable the cancel button if this is the first deck being made
        if (deck1.Count == 0 && deck2.Count == 0 && deck3.Count == 0)
        {
            createADeckCancelButton.GetComponent<Button>().interactable = false;
        }

        PopulateCardDatabase();
    }

    public void CreateDeck3()
    {
        // Set Editing Deck
        editingDeck = 3;

        // Open the Create-a-Deck window
        deckMenu.SetActive(false);
        createADeckMenu.SetActive(true);

        // Clear past card displays
        ClearDeckDisplay();

        // Load current deck if it has already been made
        if (deck3.Count == 30)
        {
            // Load individual cards
            for (int i = 0; i < 30; i++)
            {
                createADeckMenu.transform.Find("Scroll View/Viewport/ScrollArea/PokemonEntry (" + (deck3[i].dexNumber - 1) + ")").gameObject.GetComponent<PokemonEntry>().currentQuantity += 1;
            }

            // Convert real deck to temp deck until the player is ready to save
            tempDeck.Clear();
            for (int i = 0; i < 30; i++)
            {
                tempDeck.Add(deck3[i]);
            }
        }

        // Set up deck name
        if (deck3.Count == 30)
        {
            createADeck_DeckNameEntry.text = deck3Name;
        }
        else
        {
            createADeck_DeckNameEntry.text = "";
        }

        // Disable the cancel button if this is the first deck being made
        if (deck1.Count == 0 && deck2.Count == 0 && deck3.Count == 0)
        {
            createADeckCancelButton.GetComponent<Button>().interactable = false;
        }

        PopulateCardDatabase();
    }

    public Card FindCardByName(string name)
    {
        return Resources.Load("Assets/Cards/" + name) as Card;
    }

    public void OpenDeckManager()
    {
        // Enable window
        mainMenu.SetActive(false);
        deckMenu.SetActive(true);
    }

    public void CreateADeck_SaveButton()
    {
        // Convert temp deck back into a real deck
        if (editingDeck == 1)
        {
            deck1Name = createADeck_DeckNameEntry.text;
            deck1.Clear();
            for (int i = 0; i < 30; i++)
            {
                deck1.Add(tempDeck[i]);
            }
        }
        else if (editingDeck == 2)
        {
            deck2Name = createADeck_DeckNameEntry.text;
            deck2.Clear();
            for (int i = 0; i < 30; i++)
            {
                deck2.Add(tempDeck[i]);
            }
        }
        else if (editingDeck == 3)
        {
            deck3Name = createADeck_DeckNameEntry.text;
            deck3.Clear();
            for (int i = 0; i < 30; i++)
            {
                deck3.Add(tempDeck[i]);
            }
        }
        tempDeck.Clear();
        createADeckMenu.SetActive(false);
        deckMenu.SetActive(true);
    }

    public void CreateADeck_CancelButton()
    {
        tempDeck.Clear();
        createADeckMenu.SetActive(false);
        deckMenu.SetActive(true);
    }

    public void DeckManager_BackButton()
    {
        deckMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void MakeACardButton()
    {
        createADeckMenu.SetActive(false);
        makeACardMenu.SetActive(true);
    }

    public void PopulateCardDatabase()
    {
        // Add custom cards to card database
        /*string[] customCards = AssetDatabase.FindAssets("", new[] { "Cards/Custom" });
        for (int i = 0; i < 3; i++)
        {
            allCards.Add(customCards[0]);
        }*/

        // Populate the database with all of the custom cards
        for (int i = 0; i < 250; i++)
        {
            // Get the names of all of the default cards so they're not accidentally readded
            //List<string> namesToAvoid = new List<string>();
            //namesToAvoid.Add(allCards[i].name);
        }
        for (int i = 0; i < 250; i++)
        {
            // Add the cards that are not part of the default cards list
            //allCards.Add(AssetDatabase.FindAssets());
            //namesToAvoid.Add(allCards[i].name);
        }
        for (int i = 0; i < allCards.Count; i++)
        {
            // Address the specific PokemonEntry
            GameObject newEntry = GameObject.Find("PokemonEntry (" + i + ")");
            newEntry.GetComponent<PokemonEntry>().associatedPokemon = allCards[i];

            // Assign the next Pokemon on the list to the entry field
            newEntry.GetComponent<PokemonEntry>().name = allCards[i].name;

            // Define wether the card field is legendary or not
            newEntry.GetComponent<PokemonEntry>().legendary = allCards[i].legendary;

            // Limit legendary cards to one of each kind per deck
            if (newEntry.GetComponent<PokemonEntry>().legendary == false)
            {
                newEntry.GetComponent<PokemonEntry>().maxQuantity = 3;
            }
            else
            {
                newEntry.GetComponent<PokemonEntry>().maxQuantity = 1;
            }

            // Pass on the index number
            newEntry.GetComponent<PokemonEntry>().index = i;
        }
    }

    public void PlayButton()
    {
        // Transfer the current deck to the battle manager
        battleManager.player1_BattleDeck.Clear();
        battleManager.player2_BattleDeck.Clear();
        for (int i = 0; i < 30; i++)
        {
            if (currentDeck == 1)
            {
                battleManager.player1_BattleDeck.Add(deck1[i]);
            }
            else if (currentDeck == 2)
            {
                battleManager.player1_BattleDeck.Add(deck2[i]);
            }
            else if (currentDeck == 3)
            {
                battleManager.player1_BattleDeck.Add(deck3[i]);
            }

            if (deck3.Count == 30)
            {
                battleManager.player2_BattleDeck.Add(deck3[i]);
            }
            else
            {
                battleManager.player2_BattleDeck.Add(aiDeck[i]);
            }
        }

        // Start the battle
        battleManager.BattleStart();
        mainMenu.SetActive(false);
    }

    public void AllCardTypeTally()
    {
        // Debug function that displays how many cards of each type exist
        for (int i = 0; i < allCards.Count; i++)
        {
            Debug.Log(allCards[i].type);
        }
    }

    public void EnergyCostTally(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Debug.Log(deck[i].energy);
        }
    }
}
