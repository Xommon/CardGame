using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public Image player1;
    public Image player2;
    public int coinCounter;

    // Update is called once per frame
    void Update()
    {
        coinCounter++;

        if (coinCounter >= 35)
        {
            if (player1.IsActive())
            {
                FindObjectOfType<BattleManager>().playerTurn = 1;
            }
            else if (player2.IsActive())
            {
                FindObjectOfType<BattleManager>().playerTurn = 2;
            }
        }
        else if (coinCounter >= 140)
        {
            gameObject.SetActive(false);
        }
    }
}
