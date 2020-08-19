using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class MakeACard : MonoBehaviour
{
    public GameObject cancelButton;
    public Button createButton;
    public TextMeshProUGUI NameSlot;
    public TextMeshProUGUI EnergyDisplay;
    public Button Attack_Plus;
    public Button Attack_Minus;
    public TextMeshProUGUI AttackDisplay;
    public Button Health_Plus;
    public Button Health_Minus;
    public TextMeshProUGUI HealthDisplay;
    public Button Image_Plus;
    public Button Image_Minus;
    public Image ImageDisplay;
    public Button Background_Plus;
    public Button Background_Minus;
    public Image BackgroundDisplay;
    public Button Type_Plus;
    public Button Type_Minus;
    public Image TypeDisplay;
    public Button Ability_Plus;
    public Button Ability_Minus;
    public TextMeshProUGUI AbilityDisplay;
    public TextMeshProUGUI DescriptionDisplay;
    public GameManager gameManager;

    public int energy;
    public int attack;
    public int health;
    public int pokemonImage;
    public List<Sprite> pokemonSprites = new List<Sprite>();
    public List<Sprite> backgroundSprites = new List<Sprite>();
    public int backgroundImage;
    public int type;
    public List<string> types = new List<string>();
    public List<string> abilities = new List<string>();
    public int ability;
    public bool legendary;
    public Image weaknessDisplay;
    public Image resistanceDisplay;
    public string weaknessType;
    public string resistanceType;
    public TypeMatchups typeMatchups;
    public AbilityManager abilityManager;

    // Start is called before the first frame update
    void Start()
    {
        // Assign GameManager
        gameManager = FindObjectOfType<GameManager>();

        // Set default variables
        SetDefaultVariables();

        // Load lists
        // Types
        types = typeMatchups.types;
    }

    void Update()
    {
        // Disable save button if there's too many cards made or the user didn't give the card a name
        if (gameManager.allCards.Count >= 254)
        {
            createButton.interactable = false;
        }
        else if (NameSlot.text == "")
        {
            createButton.interactable = false;
        }
        else
        {
            createButton.interactable = true;
        }

        // Set up menu displays
        if (gameManager.makeACardMenu.activeInHierarchy)
        {
            EnergyDisplay.text = energy.ToString();
            AttackDisplay.text = attack.ToString();
            HealthDisplay.text = health.ToString();
            ImageDisplay.sprite = pokemonSprites[pokemonImage];
            BackgroundDisplay.sprite = backgroundSprites[backgroundImage];
            TypeDisplay.sprite = typeMatchups.typesImages[type];
            AbilityDisplay.text = abilities[ability];
            //DescriptionDisplay.text = abilityManager.abilities[abilityManager.abilitiesList[0]][1];
            DescriptionDisplay.text = abilityManager.abilitiesList[ability];
            weaknessType = typeMatchups.GetWeakness(typeMatchups.types[type]);
            resistanceType = typeMatchups.GetResistance(typeMatchups.types[type]);
        }

        // Enable and disable Attack buttons
        if (attack == 0)
        {
            Attack_Minus.interactable = false;
        }
        else if (attack == 9)
        {
            Attack_Plus.interactable = false;
        }
        else
        {
            Attack_Minus.interactable = true;
            Attack_Plus.interactable = true;
        }

        // Enable and disable Health buttons
        if (health == 1)
        {
            Health_Minus.interactable = false;
        }
        else if (health == 9)
        {
            Health_Plus.interactable = false;
        }
        else
        {
            Health_Minus.interactable = true;
            Health_Plus.interactable = true;
        }

        // Enable and disable Types buttons
        if (type == 0)
        {
            Type_Minus.interactable = false;
        }
        else if (type == (types.Count - 1))
        {
            Type_Plus.interactable = false;
        }
        else
        {
            Type_Minus.interactable = true;
            Type_Plus.interactable = true;
        }

        // Enable and disable Ability buttons
        if (ability == 0)
        {
            Ability_Minus.interactable = false;
        }
        else if (ability == (abilities.Count - 1))
        {
            Ability_Plus.interactable = false;
        }
        else
        {
            Ability_Minus.interactable = true;
            Ability_Plus.interactable = true;
        }

        // Enable and disable Background buttons
        if (backgroundImage == 0)
        {
            Background_Minus.interactable = false;
        }
        else if (backgroundImage == 47)
        {
            Background_Plus.interactable = false;
        }
        else
        {
            Background_Minus.interactable = true;
            Background_Plus.interactable = true;
        }

        // Enable and disable PokemonImage buttons
        if (pokemonImage == 0)
        {
            Image_Minus.interactable = false;
        }
        else if (pokemonImage == 17)
        {
            Image_Plus.interactable = false;
        }
        else
        {
            Image_Minus.interactable = true;
            Image_Plus.interactable = true;
        }

        // Display type weakness and resistance
        for (int i = 0; i < typeMatchups.typesImages.Count - 1; i++)
        {
            if (typeMatchups.typesImages[i].name == weaknessType)
            {
                weaknessDisplay.sprite = typeMatchups.typesImages[i];
                weaknessDisplay.color = new Color(255, 255, 255, 1);
                break;
            }
            else
            {
                weaknessDisplay.color = new Color(255, 255, 255, 0);
            }
        }
        for (int i = 0; i < typeMatchups.typesImages.Count - 1; i++)
        {
            if (typeMatchups.typesImages[i].name == resistanceType)
            {
                resistanceDisplay.sprite = typeMatchups.typesImages[i];
                resistanceDisplay.color = new Color(255, 255, 255, 1);
                break;
            }
            else
            {
                resistanceDisplay.color = new Color(255, 255, 255, 0);
            }
        }
    }

    public void AttackPlus()
    {
        attack += 1;
        DetermineEnergyCost();
    }

    public void AttackMinus()
    {
        attack -= 1;
        DetermineEnergyCost();
    }

    public void HealthPlus()
    {
        health += 1;
        DetermineEnergyCost();
    }

    public void HealthMinus()
    {
        health -= 1;
        DetermineEnergyCost();
    }

    public void TypePlus()
    {
        type += 1;
        weaknessType = typeMatchups.GetWeakness(typeMatchups.types[type]);
        resistanceType = typeMatchups.GetResistance(typeMatchups.types[type]);
    }

    public void TypeMinus()
    {
        type -= 1;
        weaknessType = typeMatchups.GetWeakness(typeMatchups.types[type]);
        resistanceType = typeMatchups.GetResistance(typeMatchups.types[type]);
    }

    public void AbilityPlus()
    {
        ability += 1;
        DetermineEnergyCost();
    }

    public void AbilityMinus()
    {
        ability -= 1;
        DetermineEnergyCost();
    }

    public void ImagePlus()
    {
        pokemonImage += 1;
    }

    public void ImageMinus()
    {
        pokemonImage -= 1;
    }

    public void BackgroundPlus()
    {
        backgroundImage += 1;
    }

    public void BackgroundMinus()
    {
        backgroundImage -= 1;
    }

    public void DetermineEnergyCost()
    {
        // Clear the energy amount so it doesn't accumulate
        energy = 0;

        // Analyse the attack strength
        energy += (attack / 2);

        // Analyse the health
        energy += (health / 2);

        // Analyse ability
        if (ability > 0)
        {
            if (ability == 2)
            {
                if (health < 4)
                {
                    energy += 2;
                }
                else if (health >= 4 && health < 7)
                {
                    energy += 3;
                }
                else
                {
                    energy += 4;
                }
            }
            else if (ability == 4)
            {
                if (attack < 4)
                {
                    energy += 2;
                }
                else if (attack >= 4 && attack < 7)
                {
                    energy += 3;
                }
                else
                {
                    energy += 4;
                }
            }
            else
            {
                energy += 1;
            }
        }

        // Cap energy cost at 9
        if (energy > 9)
        {
            legendary = true;
            energy = 9;
        }
        else
        {
            legendary = false;
        }
    }

    public void SetDefaultVariables()
    {
        // Set default variables
        energy = 0;
        attack = 0;
        health = 1;
        pokemonImage = 0;
        backgroundImage = 0;
        type = 0;
        ability = 0;
    }

    public void CancelButton()
    {
        gameManager.makeACardMenu.SetActive(false);
        gameManager.createADeckMenu.SetActive(true);
        gameManager.PopulateCardDatabase();
    }

    [MenuItem("GameObject/Create New Card")]
    public void CreateCustomCard(string name, string typeType, AudioClip cry, int dexNumber, Sprite sprite, Sprite background, int attack, int health, int energy, string ability, bool legendary)
    {
        Card customCard = new Card();
        customCard.cry = null;
        customCard.sprite = sprite;
        customCard.background = background;
        customCard.attack = attack;
        customCard.health = health;
        customCard.energy = energy;
        customCard.ability = ability;
        customCard.type = types[type];
        dexNumber = gameManager.allCards.Count + 1;
        gameManager.allCards.Add(customCard);
        customCard.legendary = legendary;
        AssetDatabase.CreateAsset(customCard, "Assets/Cards/Custom/" + name + ".asset");
        gameManager.newCustomCard = customCard;
    }

    public void CreateButton()
    {
        CreateCustomCard(NameSlot.text, types[type], null, gameManager.allCards.Count + 1, ImageDisplay.sprite, BackgroundDisplay.sprite, attack, health, energy, abilities[ability], legendary);
        gameManager.makeACardMenu.SetActive(false);
        gameManager.createADeckMenu.SetActive(true);
        gameManager.PopulateCardDatabase();
    }
}
