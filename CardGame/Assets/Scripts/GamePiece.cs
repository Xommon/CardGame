using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GamePiece : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    // Start is called before the first frame update
    void Start()
    {
        // Starting stats
        energy = card.energy;
        attack = card.attack;
        health = card.health;
        currentEnergy = energy;
        currentAttack = attack;
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
    }
}
