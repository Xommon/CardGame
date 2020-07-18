using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour
{
    public bool on;
    public int thinkCounter;
    public BattleManager battleManager;
    public GameManager gameManager;
    public List<Card> possiblePlays;
    public List<GamePiece> possibleAttacks;
    public List<GameObject> physicalCardList;
    public GamePiece gamePiece;
    public enum Phase {Waiting, PlayingCards, Attacking};
    public Phase phase;

    void Start()
    {
        phase = Phase.Waiting;
        possiblePlays = new List<Card>();
    }

    void Update()
    {
        if (on)
        {
            if (battleManager.playerTurn == 2)
            {
                thinkCounter++;

                // Time for the AI to start playing
                if (thinkCounter >= 200)
                {
                    if (phase == Phase.Waiting)
                    {
                        Phase_PlayingCards();
                    }
                }
            }
        }
        else
        {
            thinkCounter = 0;
            phase = Phase.Waiting;
        }
    }

    public void Phase_PlayingCards()
    {
        /// Card-Playing Phase
        phase = Phase.PlayingCards;
        if (battleManager.player2_BattleField.Count < 6)
        {
            Debug.Log("AI: ''There are " + battleManager.player2_BattleField.Count + " Pokemon on the field, so I can play more.''");
            // Make a list of Pokemon that can be played
            for (int i = 0; i < battleManager.player2_Hand.Count; i++)
            {
                if (battleManager.player2_Hand[i].energy <= battleManager.player2_CurrentEnergy)
                {
                    possiblePlays.Add(battleManager.player2_Hand[i]);
                }
            }

            // Choose which cards out of the possiblities to play
            Debug.Log("AI: ''Calculating possibilities ...''");
            if (possiblePlays.Count == 0)
            {
                Debug.Log("AI: ''I can't play any Pokemon.''");
                Phase_Attacking();
            }
            else if (possiblePlays.Count == 1)
            {
                PlayCard(possiblePlays[0]);
                Phase_Attacking();
            }
            else if (possiblePlays.Count == 2)
            {
                if (possiblePlays[0].energy + possiblePlays[1].energy <= battleManager.player2_CurrentEnergy)
                {
                    // Play both cards if you can afford it
                    PlayCard(possiblePlays[0]);
                    PlayCard(possiblePlays[1]);
                }
                else
                {
                    // Choose one card randomly to play
                    int chosenNumber = Random.Range(0, 2);
                    PlayCard(possiblePlays[chosenNumber]);
                }

                Phase_Attacking();
            }
            else if (possiblePlays.Count == 3)
            {
                // Try playing two cards
                if (possiblePlays[0].energy + possiblePlays[1].energy <= battleManager.player2_CurrentEnergy)
                {
                    PlayCard(possiblePlays[0]);
                    PlayCard(possiblePlays[1]);
                    Phase_Attacking();
                }
                else if (possiblePlays[1].energy + possiblePlays[2].energy <= battleManager.player2_CurrentEnergy)
                {
                    PlayCard(possiblePlays[1]);
                    PlayCard(possiblePlays[2]);
                    Phase_Attacking();
                }
                else if (possiblePlays[0].energy + possiblePlays[2].energy <= battleManager.player2_CurrentEnergy)
                {
                    PlayCard(possiblePlays[0]);
                    PlayCard(possiblePlays[2]);
                    Phase_Attacking();
                }
                else
                {
                    // If you can't play two cards, then play one
                    Card mostExpensiveCard = null;
                    if (possiblePlays[0].energy == possiblePlays[1].energy && possiblePlays[0].energy == possiblePlays[2].energy)
                    {
                        // Choose one card randomly to play
                        int chosenNumber = Random.Range(0, 3);
                        PlayCard(possiblePlays[chosenNumber]);
                        Phase_Attacking();
                    }
                    else
                    {
                        // Play the most expensive card
                        for (int i = 0; i < possiblePlays.Count; i++)
                        {
                            if (mostExpensiveCard == null || possiblePlays[i].energy > mostExpensiveCard.energy)
                            {
                                mostExpensiveCard = possiblePlays[i];
                            }
                        }
                        Phase_Attacking();
                    }
                    
                }
            }
        }
    }

    public void Phase_Attacking()
    {
        /// Attacking Phase
        Debug.Log("The AI is in the Attack Phase.");
        phase = Phase.Attacking;
        possiblePlays.Clear();

        // Ignore the game pieces that can't attack
        for (int i = 0; i < battleManager.player2_BattleField.Count; i++)
        {
            if (battleManager.player2_BattleField[i].canAttack)
            {
                possibleAttacks.Add(battleManager.player2_BattleField[i]);
            }
        }

        // If there's nothing to attack with, end the turn
        if (possibleAttacks.Count == 0)
        {
            Debug.Log("AI: ''I don't have any Pokemon that can attack.''");
            battleManager.AITurnEnd();
        }
        else if (possibleAttacks.Count == 1)
        {
            Debug.Log("AI: ''I have one Pokemon that can attack ...''");
            // Use the one game piece to attack
            if (battleManager.player1_BattleField.Count == 0)
            {
                // Attack the trainer if there are no other game pieces
                Debug.Log("AI: ''Attacking the trainer!''");
                battleManager.Attack(possibleAttacks[0], battleManager.player1_Trainer);
                battleManager.AITurnEnd();
            }
            else
            {
                Debug.Log("AI: ''Attacking a Pokemon!''");
                // Attack the strongest pokemon
                GamePiece strongestEnemy = null;

                // Search through all of the player's active game pieces
                for (int i = 0; i < battleManager.player1_BattleField.Count; i++)
                {
                    // Assemble a list of targets that would be knocked out instantly
                    List<GamePiece> attackIsEqualToHealth = new List<GamePiece>();
                    if (possibleAttacks[0].attack == battleManager.player1_BattleField[i].health)
                    {
                        attackIsEqualToHealth.Add(battleManager.player1_BattleField[i]);
                    }

                    // Find the one with the highest attack stat
                    if (strongestEnemy == null || possibleAttacks[i].attack > strongestEnemy.attack)
                    {
                        strongestEnemy = possibleAttacks[i];
                    }
                }

                // Attack the strongest pokemon
                Debug.Log("AI: ''I'm going to attack you now!''");
                battleManager.Attack(possibleAttacks[0], strongestEnemy);
                battleManager.AITurnEnd();
            }
        }
        else if (possibleAttacks.Count == 2)
        {
            if (battleManager.player1_BattleField.Count == 0)
            {
                // Attack the trainer if there are no other game pieces
                Debug.Log("AI: ''Attacking the trainer!''");
                thinkCounter = 0;
                if (thinkCounter > 50)
                {
                    battleManager.Attack(possibleAttacks[0], battleManager.player1_Trainer);
                    possibleAttacks.RemoveAt(0);
                    thinkCounter = 0;
                }
            }
        }
    }

    public void PlayCard(Card card)
    {
        // Decrease energy
        battleManager.player2_CurrentEnergy -= card.energy;

        // Play the game piece
        GamePiece newGamePiece = Instantiate(gamePiece, battleManager.player2_BattleFieldObject.transform);
        newGamePiece.card = card;
        newGamePiece.player = 2;
        battleManager.player2_BattleField.Add(newGamePiece);
        
        // Remove the card from the hand
        battleManager.player2_Hand.Remove(card);
        for (int i = 0; i < physicalCardList.Count; i++)
        {
            if (physicalCardList[i].GetComponent<CardFace>().card == card)
            {
                physicalCardList.RemoveAt(i);
                Destroy(physicalCardList[i]);
                break;
            }
        }
    }
}
