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
    public List<GamePiece> possibleTargets;
    public List<GameObject> physicalCardList;
    public GamePiece gamePiece;
    public enum Phase { Waiting, PlayingCards, Attacking, ReadyToAttack };
    public Phase phase;
    public bool endingTurn;
    public bool pokemonDamaged1;
    public bool pokemonDamaged2;

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
                if (thinkCounter >= 225)
                {
                    if (phase == Phase.Waiting)
                    {
                        Phase_PlayingCards();
                    }
                    else if (phase == Phase.ReadyToAttack)
                    {
                        Phase_Attacking();
                    }
                }
            }
        }
        else
        {
            thinkCounter = 0;
            endingTurn = false;
            phase = Phase.Waiting;
        }

        if (endingTurn)
        {
            pokemonDamaged1 = false;
            pokemonDamaged2 = false;
            for (int i = 0; i < battleManager.player2_BattleField.Count; i++)
            {
                if (battleManager.player2_BattleField[i].damaged)
                {
                    pokemonDamaged2 = true;
                    break;
                }
            }
            for (int i = 0; i < battleManager.player1_BattleField.Count; i++)
            {
                if (battleManager.player1_BattleField[i].damaged)
                {
                    pokemonDamaged1 = true;
                    break;
                }
            }

            if (pokemonDamaged1 == false && pokemonDamaged2 == false)
            {
                battleManager.AITurnEnd();
            }
        }
    }

    /// Card-Playing Phase
    public void Phase_PlayingCards()
    {
        Debug.Log("Playing cards.");
        phase = Phase.PlayingCards;
        possiblePlays.Clear();

        // Continue playing cards if there are less than 6 Pokemon already on the field
        if (battleManager.player2_BattleField.Count < 6)
        {
            Debug.Log("AI: " + battleManager.player2_BattleField.Count + " Pokemon on the field, so I can play more.''");

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
                thinkCounter = 100;
                phase = Phase.ReadyToAttack;
            }
            else
            {
                Card mostExpensiveCard = null;
                for (int i = 0; i < possiblePlays.Count; i++)
                {
                    if (mostExpensiveCard == null)
                    {
                        mostExpensiveCard = possiblePlays[i];
                    }
                    else
                    {
                        if (mostExpensiveCard.energy > possiblePlays[i].energy)
                        {
                            mostExpensiveCard = possiblePlays[i];
                        }
                        else if (mostExpensiveCard.energy == possiblePlays[i].energy)
                        {
                            if (Random.Range(0, 2) == 1)
                            {
                                mostExpensiveCard = possiblePlays[i];
                            }
                        }
                    }
                }

                PlayCard(mostExpensiveCard);
                Phase_PlayingCards();
            }
        }
        else
        {
            Debug.Log("My battle field is too full to play more Pokemon.");
            thinkCounter = 100;
            phase = Phase.ReadyToAttack;
        }
    }

    /// Attacking Phase
    public void Phase_Attacking()
    {
        Debug.Log("AI: Deciding on an attack ...");
        phase = Phase.Attacking;
        possibleAttacks.Clear();
        possibleTargets.Clear();
        thinkCounter = 0;

        // Ignore the game pieces that can't attack
        for (int i = 0; i < battleManager.player2_BattleField.Count; i++)
        {
            if (battleManager.player2_BattleField[i].canAttack)
            {
                possibleAttacks.Add(battleManager.player2_BattleField[i]);
            }
        }

        if (possibleAttacks.Count > 0)
        {
            // Compile a list of targets
            bool guardingTargetExists = false;
            if (battleManager.player1_BattleField.Count > 0)
            {
                for (int i = 0; i < battleManager.player1_BattleField.Count; i++)
                {
                    if (battleManager.player1_BattleField[i].guarding)
                    {
                        guardingTargetExists = true;
                        break;
                    }
                }

                // Must attack guarding Pokemon first
                if (guardingTargetExists)
                {
                    for (int i = 0; i < battleManager.player1_BattleField.Count; i++)
                    {
                        if (battleManager.player1_BattleField[i].guarding)
                        {
                            possibleTargets.Add(battleManager.player1_BattleField[i]);
                        }
                    }
                }
                else
                {
                    // There are no guarding Pokemon
                    // If the computer can take out the player now, it will
                    int combinedPower = 0;
                    for (int i = 0; i < possibleAttacks.Count; i++)
                    {
                        combinedPower += possibleAttacks[i].currentAttack;
                    }
                    if (combinedPower >= battleManager.player1_Trainer.currentHealth)
                    {
                        possibleTargets.Add(battleManager.player1_Trainer);
                    }
                    else
                    {
                        for (int i = 0; i < battleManager.player1_BattleField.Count; i++)
                        {
                            possibleTargets.Add(battleManager.player1_BattleField[i]);
                        }
                    }
                }
            }
            else
            {
                possibleTargets.Add(battleManager.player1_Trainer);
            }

            // Begin attacking
            // Have all Pokemon attack the player if their field is empty
            Debug.Log("AI: ''Attacking the trainer!''");
            if (battleManager.player1_BattleField.Count == 0)
            {
                battleManager.Attack(possibleAttacks[0], battleManager.player1_Trainer);
                thinkCounter = 100;
                phase = Phase.ReadyToAttack;
            }
            else if (battleManager.player1_BattleField.Count > 0 && possibleAttacks.Count > 0)
            {
                // There are Pokemon in the way
                GamePiece defender = null;
                GamePiece attacker = battleManager.player2_BattleField[Random.Range(0, battleManager.player2_BattleField.Count)];
                for (int i = 0; i < possibleTargets.Count; i++)
                {
                    if (defender == null)
                    {
                        if (attacker.currentAttack >= possibleTargets[i].currentHealth)
                        if (attacker.currentAttack >= possibleTargets[i].currentHealth)
                        {
                            defender = possibleTargets[i];
                        }
                    }
                    else
                    {
                        if (attacker.currentAttack >= possibleTargets[i].currentHealth && defender.currentAttack > possibleTargets[i].currentAttack)
                        {
                            defender = possibleTargets[i];
                        }
                    }
                }

                if (defender == null)
                {
                    for (int i = 0; i < possibleTargets.Count; i++)
                    {
                        if (defender == null)
                        {
                            defender = possibleTargets[i];
                        }
                        else
                        {
                            if (defender.currentHealth > possibleTargets[i].currentHealth)
                            {
                                defender = possibleTargets[i];
                            }
                        }
                    }
                }

                battleManager.Attack(attacker, defender);
                thinkCounter = 100;
                phase = Phase.ReadyToAttack;
            }
        }
        else
        {
            Debug.Log("AI: ''I don't have any Pokemon that can attack.''");
            if (possiblePlays.Count == 0)
            {
                endingTurn = true;
            }
            else
            {
                Phase_PlayingCards();
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
        for (int i = 0; i < physicalCardList.Count - 1; i++)
        {
            if (physicalCardList[i].GetComponent<CardFace>().card == card)
            {
                Destroy(physicalCardList[i].gameObject);
                physicalCardList.RemoveAt(i);
            }
        }

        battleManager.player2_Hand.Remove(card);

        // Ability targets
        if (card.ability == "Heal")
        {
            GamePiece mostDamagedGamePiece = null;
            for (int i = 0; i < battleManager.player2_BattleField.Count; i++)
            {
                if (mostDamagedGamePiece == null)
                {
                    mostDamagedGamePiece = battleManager.player2_BattleField[i];
                }
                else
                {
                    if ((mostDamagedGamePiece.health - mostDamagedGamePiece.currentHealth) < (battleManager.player2_BattleField[i].health - battleManager.player2_BattleField[i].currentHealth))
                    {
                        mostDamagedGamePiece = battleManager.player2_BattleField[i];
                    }
                    else if ((mostDamagedGamePiece.health - mostDamagedGamePiece.currentHealth) == (battleManager.player2_BattleField[i].health - battleManager.player2_BattleField[i].currentHealth)) 
                    {
                        if (Random.Range(0, 2) == 1)
                        {
                            mostDamagedGamePiece = battleManager.player2_BattleField[i];
                        }
                    }
                    if ((mostDamagedGamePiece.health - mostDamagedGamePiece.currentHealth) < (battleManager.player2_Trainer.health - battleManager.player2_Trainer.currentHealth))
                    {
                        mostDamagedGamePiece = battleManager.player2_Trainer;
                    }
                }
            }

            if (mostDamagedGamePiece != null)
            {
                if ((mostDamagedGamePiece.health - mostDamagedGamePiece.currentHealth) < card.health)
                {
                    mostDamagedGamePiece.currentHealth = mostDamagedGamePiece.health;
                }
                else
                {
                    mostDamagedGamePiece.currentHealth += card.health;
                }
            }
        }
        else if (card.ability == "Disable")
        {
            List<GamePiece> canDisable = new List<GamePiece>();
            for (int i = 0; i < battleManager.player1_BattleField.Count; i++)
            {
                if (battleManager.player1_BattleField[i].ability == "Toxic" || battleManager.player1_BattleField[i].ability == "Guard" || battleManager.player1_BattleField[i].ability == "Transform" || battleManager.player1_BattleField[i].ability == "Protect" || battleManager.player1_BattleField[i].ability == "Convert" || battleManager.player1_BattleField[i].ability == "Explosive")
                {
                    canDisable.Add(battleManager.player1_BattleField[i]);
                }
            }

            if (canDisable.Count > 0)
            {
                canDisable[Random.Range(0, canDisable.Count)].disabled = true;
            }
        }
        else if (card.ability == "Paralyse")
        {
            GamePiece targetToParalyse = null;
            for (int i = 0; i < battleManager.player1_BattleField.Count; i++)
            {
                if (targetToParalyse == null)
                {
                    targetToParalyse = battleManager.player1_BattleField[i];
                }
                else
                {
                    if (battleManager.player1_BattleField[i].currentAttack > targetToParalyse.currentAttack)
                    {
                        targetToParalyse = battleManager.player1_BattleField[i];
                    }
                    else if (battleManager.player1_BattleField[i].currentAttack == targetToParalyse.currentAttack)
                    {
                        if (Random.Range(0, 2) == 1)
                        {
                            targetToParalyse = battleManager.player1_BattleField[i];
                        }
                    }
                }
            }

            if (targetToParalyse != null)
            {
                targetToParalyse.paralysed = true;
            }
        }
        else if (card.ability == "Transform")
        {
            List<GamePiece> transformTargets = new List<GamePiece>();
            GamePiece transformTarget = null;
            for (int i = 0; i < battleManager.player1_BattleField.Count; i++)
            {
                transformTargets.Add(battleManager.player1_BattleField[i]);
            }
            for (int i = 0; i < battleManager.player2_BattleField.Count; i++)
            {
                transformTargets.Add(battleManager.player2_BattleField[i]);
            }
            for (int i = 0; i < transformTargets.Count; i++)
            {
                if (transformTarget == null)
                {
                    transformTarget = transformTargets[i];
                }
                else
                {
                    if (transformTargets[i].attack > transformTarget.attack)
                    {
                        transformTarget = transformTargets[i];
                    }
                    else if (transformTargets[i].attack == transformTarget.attack)
                    {
                        if (transformTargets[i].health > transformTarget.health)
                        {
                            transformTarget = transformTargets[i];
                        }
                        else if (transformTargets[i].health == transformTarget.health)
                        {
                            if (Random.Range(0, 2) == 1)
                            {
                                transformTarget = transformTargets[i];
                            }
                        }
                    }
                }
            }

            if (transformTarget != null)
            {
                newGamePiece.ability = "Transform";
                newGamePiece.transformOriginalCard = card;
                newGamePiece.transformed = true;
                newGamePiece.card = transformTarget.card;
            }
            else
            {
                newGamePiece.transformed = false;
            }
        }
    }
}