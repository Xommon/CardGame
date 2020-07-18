using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GamePiece : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Card card;
    public Image PokemonImage;
    public Image PokemonImageShadow;
    public int energy;
    public int currentEnergy;
    public int attack;
    public int currentAttack;
    public TextMeshProUGUI attackDisplay;
    public int health;
    public int currentHealth;
    public TextMeshProUGUI healthDisplay;
    public Image backgroundImage;
    public int counter;
    public bool isHovering;
    public int player;
    public bool isSelected;
    public BattleManager battleManager;
    public bool canAttack;
    public bool mouseOver;
    public GameObject damageEffect;
    public TextMeshProUGUI damageDisplay;
    public bool damaged;

    // Start is called before the first frame update
    void Start()
    {
        // Attach the Battle Manager
        battleManager = FindObjectOfType<BattleManager>();

        // Starting stats
        energy = card.energy;
        attack = card.attack;
        health = card.health;
        currentEnergy = energy;
        currentAttack = attack;
        currentHealth = health;
        canAttack = true;

        battleManager.isDragging = false;
    }
    
    void FixedUpdate()
    {
        // Pokemon with 0 attack can never attack
        if (attack == 0)
        {
            canAttack = false;
        }

        // Set variables
        PokemonImage.sprite = card.sprite;
        PokemonImageShadow.sprite = card.sprite;
        if (card.type == "Bug")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(170, 240, 90, 255);
        }
        else if (card.type == "Dark")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(25, 25, 25, 255);
        }
        else if (card.type == "Dragon")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(84, 38, 183, 255);
        }
        else if (card.type == "Electric")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(222, 207, 37, 255);
        }
        else if (card.type == "Fairy")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(255, 65, 220, 255);
        }
        else if (card.type == "Fighting")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(94, 31, 12, 255);
        }
        else if (card.type == "Fire")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(255, 33, 28, 255);
        }
        else if (card.type == "Flying")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(153, 138, 255, 255);
        }
        else if (card.type == "Ghost")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(51, 31, 75, 255);
        }
        else if (card.type == "Grass")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(75, 255, 55, 255);
        }
        else if (card.type == "Ground")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(226, 181, 97, 255);
        }
        else if (card.type == "Ice")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(67, 255, 255, 255);
        }
        else if (card.type == "Normal")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(205, 184, 144, 255);
        }
        else if (card.type == "Poison")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(97, 21, 62, 255);
        }
        else if (card.type == "Psychic")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(255, 50, 123, 255);
        }
        else if (card.type == "Rock")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(162, 94, 45, 255);
        }
        else if (card.type == "Steel")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(142, 142, 142, 255);
        }
        else if (card.type == "Water")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(62, 158, 255, 255);
        }
        else if (card.type == "Sound")
        {
            backgroundImage.GetComponent<Image>().color = new Color32(28, 31, 120, 255);
        }

        // Update stats on card
        attackDisplay.text = currentAttack.ToString();
        healthDisplay.text = currentHealth.ToString();

        // Change colour of text to indicate a change from the default stats
        if (currentHealth < health)
        {
            healthDisplay.color = Color.red;
        }
        else if (currentHealth > health)
        {
            healthDisplay.color = Color.green;
        }
        else
        {
            healthDisplay.color = Color.white;
        }

        if (currentAttack < attack)
        {
            attackDisplay.color = Color.red;
        }
        else if (currentAttack > attack)
        {
            attackDisplay.color = Color.green;
        }
        else
        {
            attackDisplay.color = Color.white;
        }

        // Give the game piece a green glow if they're able to attack
        if (battleManager.playerTurn == 1 && canAttack == true && isSelected == false && player == 1 && mouseOver == false && battleManager.playerTurn == 1)
        {
            gameObject.GetComponent<Image>().color = new Color32(0, 255, 0, 150);
        }

        // Count for a delay after being damaged
        if (damaged)
        {
            // Deselect the attacking object
            isSelected = false;
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 0);

            // Start counter
            counter++;
            if (counter >= 100)
            {
                // End the damage effect
                damageEffect.GetComponent<Animator>().enabled = false;
                damageEffect.SetActive(false);
                battleManager.attackInProgress = false;
                damaged = false;

                // The pokemon has been knocked out if it has 0 or less health
                if (currentHealth <= 0)
                {
                    // Enabled death animation
                    gameObject.GetComponent<Animator>().enabled = true;
                    damaged = true;
                    if (counter >= 180)
                    {
                        if (player == 1)
                        {
                            battleManager.player1_BattleField.Remove(this);
                        }
                        else
                        {
                            battleManager.player2_BattleField.Remove(this);
                        }

                        // Remove self from field list
                        battleManager.player1_BattleField.Remove(this);

                        // Add self to discard pile
                        if (player == 1)
                        {
                            battleManager.player1_DiscardPile.Add(card);
                        }
                        else if (player == 1)
                        {
                            battleManager.player2_DiscardPile.Add(card);
                        }

                        // Destroy object
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Select the game piece if it's the player's turn
        if (battleManager.playerTurn == 1)
        {
            if (player == 1 && canAttack)
            {
                if (isSelected == false)
                {
                    // Unselect all other game pieces before selecting this piece
                    for (int i = 0; i < battleManager.player1_BattleField.Count; i++)
                    {
                        battleManager.player1_BattleField[i].isSelected = false;
                        battleManager.player1_BattleField[i].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                        battleManager.selectedGamePiece = null;
                    }

                    // Mark the object as selected
                    isSelected = true;
                    gameObject.GetComponent<Image>().color = new Color32(255, 242, 0, 255);
                    battleManager.selectedGamePiece = gameObject.GetComponent<GamePiece>();

                    // Highlight all other Pokemon and trainers that can be targetted

                }
                else
                {
                    // If the selected piece is clicked again, unselect it
                    isSelected = false;
                    battleManager.selectedGamePiece = null;
                    if (battleManager.playerTurn == 1 && canAttack == true && isSelected == false && player == 1)
                    {
                        gameObject.GetComponent<Image>().color = new Color32(150, 255, 150, 255);
                    }
                }
            }
            else
            {
                // Attack the target piece with the selected game piece
                if (battleManager.selectedGamePiece != null)
                {
                    battleManager.Attack(battleManager.selectedGamePiece, eventData.pointerPress.transform.gameObject.GetComponent<GamePiece>());
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;

        if (isSelected == false)
        {
            if (canAttack == true && player == 1 && battleManager.playerTurn == 1)
            {
                gameObject.GetComponent<Image>().color = new Color32(150, 255, 150, 255);
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;

        if (isSelected == false)
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }
    }
}
