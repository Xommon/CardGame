using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public int damageDealt;
    public BattleManager battleManager;
    public bool attackInProgress;

    void Start()
    {
        battleManager = GetComponent<BattleManager>();
    }

    public bool NotAttackingATrainer(GamePiece defender)
    {
        if (defender.name != "Player1Trainer" && defender.name != "Player2Trainer")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Attack(GamePiece attacker, GamePiece defender)
    {
        int multiplierAttacker = attacker.currentAttack;
        int multiplierDefender = defender.currentAttack;
        
        // Apply weakness and resistance to damage if not attacking a trainer
        if (attacker.weakness == defender.type && NotAttackingATrainer(defender))
        {
            multiplierDefender = defender.currentAttack * 2;
        }
        else if (attacker.resistance == defender.type && NotAttackingATrainer(defender))
        {
            multiplierDefender = Mathf.RoundToInt(defender.currentAttack / 2);
        }
        if (defender.weakness == attacker.type)
        {
            multiplierAttacker = attacker.currentAttack * 2;
        }
        else if (defender.resistance == attacker.type && NotAttackingATrainer(defender))
        {
            multiplierAttacker = Mathf.RoundToInt(attacker.currentAttack / 2);
        }

        // The attacker attacks the defender
        if ((attacker.player != defender.player && attacker.canAttack) && (battleManager.playerTurn == 1 && !attackInProgress) || battleManager.playerTurn == 2)
        {
            if (battleManager.playerTurn == 1 && !attackInProgress)
            {
                attacker.HighlightEnemies();
            }
            attacker.counter = 0;
            defender.counter = 0;
            attackInProgress = true;
            attacker.canAttack = false;
            attacker.isSelected = false;
            defender.isSelected = false;
            battleManager.selectedGamePiece = null;
            attacker.damageEffect.SetActive(true);
            attacker.damageEffect.GetComponent<Animator>().enabled = true;
            attacker.damaged = true;
            defender.damageEffect.SetActive(true);
            defender.damageEffect.GetComponent<Animator>().enabled = true;
            defender.damaged = true;
            if (NotAttackingATrainer(defender))
            {
                // Attacker
                if (attacker.wonderGuarded && (attacker.weakness != defender.type))
                {
                    attacker.damageDisplay.text = "Wonder\nGuard";
                }
                else if (attacker.shielded)
                {
                    attacker.damageDisplay.text = "Protect";
                    if (defender.attack > 0)
                    {
                        attacker.shielded = false;
                    }
                }
                else if (defender.toxic)
                {
                    attacker.damageDisplay.text = "Toxic";
                    attacker.currentHealth = 0;
                }
                else
                {
                    attacker.damageDisplay.text = "-" + multiplierDefender;
                    attacker.currentHealth -= multiplierDefender;
                }

                // Defender
                if (defender.wonderGuarded && (defender.weakness != attacker.type))
                {
                    defender.damageDisplay.text = "Wonder\nGuard";
                }
                else if (defender.shielded)
                {
                    defender.damageDisplay.text = "Protect";
                    if (attacker.attack > 0)
                    {
                        defender.shielded = false;
                    }
                }
                else if (attacker.toxic)
                {
                    defender.damageDisplay.text = "Toxic";
                    defender.currentHealth = 0;
                }
                else
                {
                    defender.damageDisplay.text = "-" + multiplierAttacker;
                    defender.currentHealth -= multiplierAttacker;
                }
            }
            else
            {
                attacker.currentHealth -= multiplierDefender;
                attacker.damageDisplay.text = "-" + multiplierDefender;
                defender.currentHealth -= multiplierAttacker;
                defender.damageDisplay.text = "-" + multiplierAttacker;
            }
        }
    }

    public void Damage(GamePiece defender, int damageAmount)
    {
        if (defender.shielded)
        {
            defender.damageDisplay.text = "Shield";
            defender.shielded = false;
        }
        else
        {
            defender.currentHealth -= damageAmount;
            defender.damageDisplay.text = "-" + damageAmount;
        }
        defender.counter = 0;
        attackInProgress = true;
        defender.isSelected = false;
        battleManager.selectedGamePiece = null;
        defender.damageEffect.SetActive(true);
        defender.damageEffect.GetComponent<Animator>().enabled = true;
        defender.damaged = true;
    }
}
