using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class CardFace : MonoBehaviour
{
    public Card card;
    public TextMeshProUGUI nameText;
    public Image typeImage;
    public Texture2D typeSpriteSheet;
    public AudioClip cryAudio;
    public Image cardImage;
    public Image cardBackground;
    public TextMeshProUGUI cardAbility;
    public TextMeshProUGUI attackText;
    public int remainingAttack;
    public TextMeshProUGUI healthText;
    public int remainingHealth;
    public TextMeshProUGUI energyText;
    public string weaknessType;
    public string resistanceType;

    // Start is called before the first frame update
    void FixedUpdate()
    {
        // Separate Pokemon types sprites into array from spritesheet then assign the correct type sprite to the card
        Sprite[] sprites = Resources.LoadAll<Sprite>(typeSpriteSheet.name);
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name == card.type)
            {
                typeImage.sprite = sprites[i];
                break;
            }
        }

        // Values to appear on the card's face
        nameText.text = Path.GetFileName("Assets/Cards/" + card.ToString().Substring(0, card.name.Length));
        cryAudio = card.cry;
        cardImage.sprite = card.sprite;
        cardBackground.sprite = card.background;
        cardAbility.text = card.ability;
        attackText.text = card.attack.ToString();
        healthText.text = card.health.ToString();
        energyText.text = card.energy.ToString();
        remainingAttack = card.attack;
        remainingHealth = card.health;

        // Determine card's weakness and resistance
        if (card.type == "Bug")
        {
            weaknessType = "Flying";
            resistanceType = "";
        }
        else if (card.type == "Dark")
        {
            weaknessType = "Bug";
            resistanceType = "Psychic";
        }
        else if (card.type == "Dragon")
        {
            weaknessType = "Fairy";
            resistanceType = "";
        }
        else if (card.type == "Electric")
        {
            weaknessType = "Ground";
            resistanceType = "";
        }
        else if (card.type == "Fairy")
        {
            weaknessType = "Poison";
            resistanceType = "Dragon";
        }
        else if (card.type == "Fighting")
        {
            weaknessType = "Psychic";
            resistanceType = "";
        }
        else if (card.type == "Fire")
        {
            weaknessType = "Water";
            resistanceType = "";
        }
        else if (card.type == "Flying")
        {
            weaknessType = "Ice";
            resistanceType = "Ground";
        }
        else if (card.type == "Ghost")
        {
            weaknessType = "Dark";
            resistanceType = "Normal";
        }
        else if (card.type == "Grass")
        {
            weaknessType = "Fire";
            resistanceType = "";
        }
        else if (card.type == "Ground")
        {
            weaknessType = "Grass";
            resistanceType = "Electric";
        }
        else if (card.type == "Ice")
        {
            weaknessType = "Fire";
            resistanceType = "";
        }
        else if (card.type == "Normal")
        {
            weaknessType = "Fighting";
            resistanceType = "";
        }
        else if (card.type == "Poison")
        {
            weaknessType = "Psychic";
            resistanceType = "";
        }
        else if (card.type == "Psychic")
        {
            weaknessType = "Bug";
            resistanceType = "Fighting";
        }
        else if (card.type == "Rock")
        {
            weaknessType = "Water";
            resistanceType = "";
        }
        else if (card.type == "Steel")
        {
            weaknessType = "Ground";
            resistanceType = "Poison";
        }
        else if (card.type == "Water")
        {
            weaknessType = "Electric";
            resistanceType = "";
        }
        /*else if (card.type == "Sound")
        {
            weaknessType = "Rock";
            resistanceType = "Sound";
        }*/
    }
}
