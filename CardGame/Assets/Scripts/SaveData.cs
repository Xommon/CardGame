using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData : MonoBehaviour
{
    /// Variables to be saved ///
    // Decks
    public int[] deck1 = new int[30];
    public int[] deck2 = new int[30];
    public int[] deck3 = new int[30];
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
        // Deconstruct the existing decks down to their dex numbers to be saved
        for (int i = 0; i < 30; i++)
        {
            if (deck1.Length != 0)
            {
                deck1[i] = gameManager.deck1[i].dexNumber;
            }
            if (deck2.Length != 0)
            {
                deck2[i] = gameManager.deck2[i].dexNumber;
            }
            if (deck3.Length != 0)
            {
                deck3[i] = gameManager.deck3[i].dexNumber;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
