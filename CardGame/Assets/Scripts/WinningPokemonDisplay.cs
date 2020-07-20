using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinningPokemonDisplay : MonoBehaviour
{
    public Card card;
    public Image image;
    
    void Start()
    {
        image.sprite = card.sprite;
    }

    private void Update()
    {
        if (FindObjectOfType<BattleManager>().quitGame)
        {
            Destroy(gameObject);
        }
    }
}
