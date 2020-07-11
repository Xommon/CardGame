using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.IO;

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
    public int index;
    public bool legendary;

    private void Start()
    {
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
    }

    private void Update()
    {
        // Assign values to the text fields
        thisEntryName.GetComponent<TextMeshProUGUI>().text = name;
        thisEntryCount.GetComponent<TextMeshProUGUI>().text = currentQuantity + "/" + maxQuantity;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardDisplay.GetComponent<CardFace>().card = Object.FindObjectOfType<GameManager>().allCards[index];
            //Path.GetFileName("Assets/Cards/" + card.ToString().Substring(0, card.name.Length))
        //cardDisplay.GetComponent<CardFace>().card = associatedPokemon.GetComponent<CardFace>().card;
    }
}
