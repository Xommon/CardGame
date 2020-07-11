using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PokemonEntry : MonoBehaviour
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
    }

    private void Update()
    {
        // Assign values to the text fields
        thisEntryName.GetComponent<TextMeshProUGUI>().text = name;
        thisEntryCount.GetComponent<TextMeshProUGUI>().text = currentQuantity + "/" + maxQuantity;
    }
}
