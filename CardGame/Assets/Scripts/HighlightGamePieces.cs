using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HighlightGamePieces : MonoBehaviour
{
    public event EventHandler OnHighlightGamePiece;
    public BattleManager battleManager;

    void Awake()
    {
        battleManager = GetComponent<BattleManager>();
    }

    void Update()
    {
        // Unhighlight game pieces if no piece is selected
        if (battleManager.selectedGamePiece == null && battleManager.enemiesHighlighted)
        {
            FindObjectOfType<GamePiece>().HighlightEnemies();
        }
    }
}
