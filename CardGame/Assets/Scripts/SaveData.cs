using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData : MonoBehaviour
{
    /// Variables to be saved ///
    // Decks
    public string deck1Name;
    public string deck2Name;
    public string deck3Name;
    public string[] deck1 = new string[30];
    public string[] deck2 = new string[30];
    public string[] deck3 = new string[30];
    public int currentDeck;

    // Custom Card 1
    public string customCard1_Name;
    public int customCard1_Attack;
    public int customCard1_Heath;
    public int customCard1_Type;
    public string customCard1_Ability;
    public int customCard1_Background;
    public int customCard1_PokemonImage;

    // Custom Card 2
    public string customCard2_Name;
    public int customCard2_Attack;
    public int customCard2_Heath;
    public int customCard2_Type;
    public string customCard2_Ability;
    public int customCard2_Background;
    public int customCard2_PokemonImage;

    // Custom Card 3
    public string customCard3_Name;
    public int customCard3_Attack;
    public int customCard3_Heath;
    public int customCard3_Type;
    public string customCard3_Ability;
    public int customCard3_Background;
    public int customCard3_PokemonImage;
    
    public SaveData (GameManager gameManager)
    {
        // Save deck names
        deck1Name = gameManager.deck1Name;
        deck2Name = gameManager.deck2Name;
        deck3Name = gameManager.deck3Name;

        // Deconstruct the existing decks down to their Pokemon names to be saved
        if (gameManager.deck1.Count == 30)
        {
            for (int i = 0; i < 30; i++)
            {
                deck1[i] = gameManager.deck1[i].name;
            }
        }
        if (gameManager.deck2.Count == 30)
        {
            for (int i = 0; i < 30; i++)
            {
                deck2[i] = gameManager.deck2[i].name;
            }
        }
        if (gameManager.deck3.Count == 30)
        {
            for (int i = 0; i < 30; i++)
            {
                deck3[i] = gameManager.deck3[i].name;
            }
        }

        currentDeck = gameManager.currentDeck;
    }
}
