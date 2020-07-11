﻿using System.Collections;
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

    // Start is called before the first frame update
    void Update()
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
    }
}
