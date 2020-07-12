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
    public List<Card> deck1 = new List<Card>();
    public List<Card> deck2 = new List<Card>();
    public List<Card> deck3 = new List<Card>();
    public Card newCustomCard;
    public int currentDeck;

    // Menus
    public GameObject mainMenu;
    public GameObject deckMenu;
    public GameObject createADeckMenu;
    public GameObject makeACardMenu;
    public GameObject pokemonEntry;

    // Submenu Items
    public GameObject deckBackButton;
    public GameObject deckEditButton;
    public GameObject createADeckCancelButton;
    public GameObject createADeckSaveButton;
    public GameObject createADeckScrollArea;
    public GameObject createADeckAmountOfCardsDisplay;

    // Custom Card 1
    public string customCard1_Name;
    public int customCard1_Energy;
    public int customCard1_Attack;
    public int customCard1_Heath;
    public int customCard1_Type;
    public string customCard1_Ability;
    public int customCard1_Background;
    public int customCard1_PokemonImage;
    public int customCard1_dexNumber;

    // Custom Card 2
    public string customCard2_Name;
    public int customCard2_Energy;
    public int customCard2_Attack;
    public int customCard2_Heath;
    public int customCard2_Type;
    public string customCard2_Ability;
    public int customCard2_Background;
    public int customCard2_PokemonImage;
    public int customCard2_dexNumber;

    // Custom Card 3
    public string customCard3_Name;
    public int customCard3_Energy;
    public int customCard3_Attack;
    public int customCard3_Heath;
    public int customCard3_Type;
    public string customCard3_Ability;
    public int customCard3_Background;
    public int customCard3_PokemonImage;
    public int customCard3_dexNumber;

    // Start is called before the first frame update
    void Start()
    {
        // If no decks have been created, then it's the player's first time playing. Open the deck creation menu.
        if (deck1.Count == 0 && deck2.Count == 0 && deck3.Count == 0)
        {
            deckMenu.SetActive(true);
            deckBackButton.SetActive(false);
            deckEditButton.SetActive(false);
        }
        else // Otherwise, open the main menu
        {
            mainMenu.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Disable the save button in Create-a-Deck mode until the deck has 30 cards
        if (createADeckMenu.activeInHierarchy == true)
        {
            if (currentDeck == 1)
            {
                if (deck1.Count != 30)
                {
                    createADeckSaveButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    createADeckSaveButton.GetComponent<Button>().interactable = true;
                }
            }
            else if (currentDeck == 2)
            {
                if (deck2.Count != 30)
                {
                    createADeckSaveButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    createADeckSaveButton.GetComponent<Button>().interactable = true;
                }
            }
            if (currentDeck == 3)
            {
                if (deck3.Count != 30)
                {
                    createADeckSaveButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    createADeckSaveButton.GetComponent<Button>().interactable = true;
                }
            }
        }

        // Update the amount of cards in the deck
        if (currentDeck == 1)
        {
            createADeckAmountOfCardsDisplay.GetComponent<TextMeshProUGUI>().text = deck1.Count + " / 30";
        }
        else if (currentDeck == 2)
        {
            createADeckAmountOfCardsDisplay.GetComponent<TextMeshProUGUI>().text = deck2.Count + " / 30";
        }
        else if (currentDeck == 3)
        {
            createADeckAmountOfCardsDisplay.GetComponent<TextMeshProUGUI>().text = deck3.Count + " / 30";
        }
    }

    public void CreateDeck1()
    {
        // Set the current deck
        currentDeck = 1;

        // Load current deck if it has already been made
        if (deck1.Count == 30)
        {
            for (int i = 0; i < 30; i++)
            {
                createADeckMenu.transform.Find("Scroll View/Viewport/ScrollArea/PokemonEntry (" + (deck1[i].dexNumber - 1) + ")").gameObject.GetComponent<PokemonEntry>().currentQuantity += 1; ;
            }
        }

        // Open the Create-a-Deck window
        deckMenu.SetActive(false);
        createADeckMenu.SetActive(true);

        // Disable the cancel button if this is the first deck being made
        if (deck1.Count == 0 && deck2.Count == 0 && deck3.Count == 0)
        {
            createADeckCancelButton.GetComponent<Button>().interactable = false;
        }

        // Populate the database with all of the cards
        for (int i = 0; i < allCards.Count; i++)
        {
            // Address the specific PokemonEntry
            GameObject newEntry = GameObject.Find("PokemonEntry (" + i + ")");

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

    public void OpenDeckManager()
    {
        mainMenu.SetActive(false);
        deckMenu.SetActive(true);
    }
}
