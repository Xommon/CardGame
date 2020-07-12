using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;

public class PokemonEntry : MonoBehaviour, IPointerEnterHandler
{
    public GameObject associatedPokemon;
    public new string name;
    public int maxQuantity;
    public int currentQuantity;
    public GameObject thisPlusButton;
    public GameObject thisMinusButton;
    public GameObject thisEntryName;
    public GameObject thisEntryCount;
    public GameObject thisHighlight;
    public GameObject cardDisplay;
    public Image weaknessDisplay;
    public Image resistanceDisplay;
    public Texture2D typeSpriteSheet;
    public int currentDeck;
    public int index;
    public bool legendary;

    private void Start()
    {
        // Set current deck
        currentDeck = FindObjectOfType<GameManager>().currentDeck;

        // Attach Pokemon card name and quantity fields
        thisEntryName = gameObject.transform.Find("PokemonName").gameObject;
        thisEntryCount = gameObject.transform.Find("IndividualCardCount").gameObject;

        // Attach plus and minus buttons that will affect the quantity of a card per deck
        thisPlusButton = gameObject.transform.Find("PlusButton").gameObject;
        thisMinusButton = gameObject.transform.Find("MinusButton").gameObject;

        // Attach highlight section
        thisHighlight = gameObject.transform.Find("Highlight").gameObject;

        // Attach card object to the Pokemon entry
        associatedPokemon = GameObject.Find(name);

        // Attach the card for display
        cardDisplay = GameObject.Find("CardDisplay");

        // Attach spritesheet for typing
        typeSpriteSheet = cardDisplay.GetComponent<CardFace>().typeSpriteSheet;
    }

    private void Update()
    {
        // Assign values to the text fields
        thisEntryName.GetComponent<TextMeshProUGUI>().text = name;
        thisEntryCount.GetComponent<TextMeshProUGUI>().text = currentQuantity + "/" + maxQuantity;

        // Separate Pokemon types sprites into array from spritesheet, then display the correct weakness and resistance
        Sprite[] sprites = Resources.LoadAll<Sprite>(typeSpriteSheet.name);
        for (int i = 0; i < sprites.Length; i++)
        {
            if (cardDisplay.GetComponent<CardFace>().weaknessType != "")
            {
                weaknessDisplay.color = new Color(255, 255, 255, 1);
                if (sprites[i].name == cardDisplay.GetComponent<CardFace>().weaknessType)
                {
                    weaknessDisplay.sprite = sprites[i];
                    break;
                }
            }
            else
            {
                weaknessDisplay.color = new Color(255, 255, 255, 0);
            }
        }
        for (int i = 0; i < sprites.Length; i++)
        {
            if (cardDisplay.GetComponent<CardFace>().resistanceType != "")
            {
                resistanceDisplay.color = new Color(255, 255, 255, 1);
                if (sprites[i].name == cardDisplay.GetComponent<CardFace>().resistanceType)
                {
                    resistanceDisplay.sprite = sprites[i];
                    break;
                }
            }
            else
            {
                resistanceDisplay.color = new Color(255, 255, 255, 0);
            }
        }

        // Disable and enable plus/minus buttons accordingly
        if (currentQuantity == 0)
        {
            thisMinusButton.GetComponent<Button>().interactable = false;
            thisPlusButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            thisMinusButton.GetComponent<Button>().interactable = true;
        }

        if ((maxQuantity == 3 && currentQuantity == 3) || (maxQuantity == 1 && currentQuantity == 1))
        {
            thisPlusButton.GetComponent<Button>().interactable = false;
        }

        if (currentQuantity < maxQuantity)
        {
            thisPlusButton.GetComponent<Button>().interactable = true;
        }
    }

    // Add functionality to the buttons
    public void AddCard()
    {
        currentQuantity += 1;
        FindObjectOfType<GameManager>().tempDeck.Add(cardDisplay.GetComponent<CardFace>().card);
    }

    public void RemoveCard()
    {
        currentQuantity -= 1;
        FindObjectOfType<GameManager>().tempDeck.Remove(cardDisplay.GetComponent<CardFace>().card);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Display card
        cardDisplay.GetComponent<CardFace>().card = Object.FindObjectOfType<GameManager>().allCards[index];
    }
}
