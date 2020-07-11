using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Card> allCards;
    public List<Card> deck1;
    public List<Card> deck2;
    public List<Card> deck3;

    // Start is called before the first frame update
    void Start()
    {
        // Assemble all possible cards into a list
        allCards = new List<Card>();


        // If no decks have been created, then it's the player's first time playing. Open the deck creation menu.


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
