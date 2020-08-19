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
    public int weatherAttackBoost;
    public TextMeshProUGUI attackDisplay;
    public int health;
    public int currentHealth;
    public TextMeshProUGUI healthDisplay;
    public Image backgroundImage;
    public string type;
    public string ability;
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
    public bool targetable;
    public string weakness;
    public string resistance;
    public List<Sprite> forms;
    public WeatherManager weatherManager;
    public TypeMatchups typeMatchups;
    public DamageSystem damageSystem;
    public DeckManager deckManager;
    public AnnouncementEvents announcementEvents;

    // Abilities
    public GameObject guardEffect;
    public GameObject protectEffect;
    public GameObject disableEffect;
    public GameObject electricityEffect;
    public GameObject toxicEffect;
    public GameObject transformEffect;
    public GameObject wonderGuardEffect;
    public bool toxic;
    public bool paralysed;
    public bool shielded;
    public bool guarding;
    public bool disabled;
    public bool transformed;
    public Card transformOriginalCard;
    public bool wonderGuarded;
    public Dictionary<string, Color32> typeColours = new Dictionary<string, Color32>()
    {
        { "Bug", new Color32(170, 240, 90, 255) },
        { "Cosmic", new Color32(50, 15, 150, 255) },
        { "Dark", new Color32(25, 25, 25, 255) },
        { "Dragon", new Color32(84, 38, 183, 255) },
        { "Electric", new Color32(222, 207, 37, 255) },
        { "Fairy", new Color32(255, 65, 220, 255) },
        { "Fighting", new Color32(94, 31, 12, 255) },
        { "Fire", new Color32(255, 33, 28, 255) },
        { "Flying", new Color32(153, 138, 255, 255) },
        { "Ghost", new Color32(51, 31, 75, 255) },
        { "Grass", new Color32(75, 255, 55, 255) },
        { "Ground", new Color32(226, 181, 97, 255) },
        { "Ice", new Color32(67, 255, 255, 255) },
        { "Normal", new Color32(205, 184, 144, 255) },
        { "Poison", new Color32(97, 21, 62, 255) },
        { "Psychic", new Color32(255, 50, 123, 255) },
        { "Rock", new Color32(162, 94, 45, 255) },
        { "Sound", new Color32(255, 137, 0, 255) },
        { "Steel", new Color32(142, 142, 142, 255) },
        { "Water", new Color32(62, 158, 255, 255) },
    };
    public Dictionary<WeatherManager.Weather, string[]> typesAffectedByWeather = new Dictionary<WeatherManager.Weather, string[]>()
    {
        { WeatherManager.Weather.Clear, new string[]{"Normal", ""} },
        { WeatherManager.Weather.Sunny, new string[]{"Fire", "Grass"} },
        { WeatherManager.Weather.Rainy, new string[]{"Water", ""} },
        { WeatherManager.Weather.Snowy, new string[]{"Ice", ""} },
        { WeatherManager.Weather.Sandstorm, new string[]{"Ground", "Rock"} },
        { WeatherManager.Weather.Stormy, new string[]{"Electric", ""} },
        { WeatherManager.Weather.Cloudy, new string[]{"Dark", ""} },
        { WeatherManager.Weather.Windy, new string[]{"Flying", ""} },
        { WeatherManager.Weather.MeteorShower, new string[]{"Cosmic", "Steel"} },
    };

    // Start is called before the first frame update
    void Start()
    {
        // Attach the Battle Manager
        battleManager = FindObjectOfType<BattleManager>();
        weatherManager = FindObjectOfType<WeatherManager>();
        typeMatchups = FindObjectOfType<TypeMatchups>();
        damageSystem = FindObjectOfType<DamageSystem>();
        deckManager = FindObjectOfType<DeckManager>();

        // Starting stats
        energy = card.energy;
        attack = card.attack;
        health = card.health;
        currentEnergy = energy;
        currentAttack = attack;
        currentHealth = health;
        ability = card.ability;
        type = card.type;
        PokemonImage.sprite = card.sprite;
        PokemonImageShadow.sprite = card.sprite;
        
        // Abilities
        if (ability == "Quick")
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
        battleManager.isDragging = false;
        if (ability == "Guard")
        {
            guarding = true;
        }
        if (ability == "Convert" && player == 2)
        {
            type = FindObjectOfType<MakeACard>().types[Random.Range(0, FindObjectOfType<MakeACard>().types.Count)];
        }
        if (ability == "Toxic")
        {
            toxic = true;
        }
        if (ability == "Transform")
        {
            transformEffect.SetActive(true);
        }
        if (ability == "Delay")
        {
            paralysed = true;
        }
        if (ability == "Protect")
        {
            shielded = true;
        }
        if (ability == "Draw Card")
        {
            deckManager.DrawCard(player);
        }
        if (ability == "AirLock" || ability == "Drought" || ability == "Flood" || ability == "Blizzard" || ability == "Sand Stream" || ability == "Stormy" || ability == "Whirlwind" || ability == "Overcast" || ability == "Meteor Shower")
        {
            weatherManager.weatherPokemon.Add(this);
        }
        if (ability == "Wonder Guard")
        {
            wonderGuarded = true;
        }

        // Set weakness, resistance, and game piece colour
        weakness = typeMatchups.GetWeakness(type);
        resistance = typeMatchups.GetResistance(type);
        backgroundImage.color = typeColours[type];
    }
    
    void FixedUpdate()
    {
        // Destroy self once game has been quit
        if (battleManager.quitGame)
        {
            Destroy(gameObject);
        }

        // Ability effects
        if (paralysed)
        {
            electricityEffect.SetActive(true);
        }
        else
        {
            electricityEffect.SetActive(false);
        }
        if (transformed)
        {
            transformEffect.SetActive(true);
        }
        else
        {
            transformEffect.SetActive(false);
        }
        if (shielded)
        {
            protectEffect.SetActive(true);
        }
        else
        {
            protectEffect.SetActive(false);
        }
        if (guarding)
        {
            guardEffect.SetActive(true);
        }
        else
        {
            guardEffect.SetActive(false);
        }
        if (toxic)
        {
            toxicEffect.SetActive(true);
        }
        else
        {
            toxicEffect.SetActive(false);
        }
        if (disabled)
        {
            disableEffect.SetActive(true);
            ability = "";
            toxic = false;
            shielded = false;
            guarding = false;
            wonderGuarded = false;
            if (transformed)
            {
                int damageTransfer = (health - currentHealth);
                card = transformOriginalCard;
                health = transformOriginalCard.health;
                currentHealth = health;
                attack = transformOriginalCard.attack;
                currentAttack = attack;
                currentHealth -= damageTransfer;
                PokemonImage.sprite = transformOriginalCard.sprite;
            }
            transformed = false;
        }
        else
        {
            disableEffect.SetActive(false);
        }
        if (wonderGuarded)
        {
            wonderGuardEffect.SetActive(true);
        }
        else
        {
            wonderGuardEffect.SetActive(false);
        }

        // Pokemon with 0 attack can never attack
        if (attack == 0)
        {
            canAttack = false;
        }

        // Set variables
        PokemonImageShadow.sprite = PokemonImage.sprite;
        if (ability != "Convert" && ability != "Forecast")
        {
            type = card.type;
        }

        // Weather effects on attack
        if (type != "Normal" && type == typesAffectedByWeather[weatherManager.weather][0] || type == typesAffectedByWeather[weatherManager.weather][1])
        {
            weatherAttackBoost = 2;
        }
        else
        {
            weatherAttackBoost = 0;
        }

        if (ability == "Forecast")
        {
            type = typesAffectedByWeather[weatherManager.weather][0];
            PokemonImage.sprite = forms[weatherManager.spriteIndexByWeather[weatherManager.weather]];
        }

        // Update stats on card
        attackDisplay.text = currentAttack.ToString();
        healthDisplay.text = currentHealth.ToString();

        // Mark highlighted enemies
        if (targetable)
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 150);
        }

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

        // Take in account for weather changes
        currentAttack = attack + weatherAttackBoost;

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

                // Run away ability
                if (ability == "Run Away")
                {
                    if (player == 1)
                    {
                        Instantiate(card, battleManager.player1_HandObject.transform);
                        battleManager.player1_BattleField.Remove(this);
                        battleManager.player1_Hand.Add(card);
                    }
                    else if (player == 2)
                    {
                        Instantiate(card, battleManager.player2_HandObject.transform);
                        battleManager.player2_BattleField.Remove(this);
                        battleManager.player2_Hand.Add(card);
                    }

                    Destroy(gameObject);
                }

                // The pokemon has been knocked out if it has 0 or less health
                if (currentHealth <= 0)
                {
                    // Enabled death animation
                    gameObject.GetComponent<Animator>().enabled = true;
                    damaged = true;

                    if (counter >= 130)
                    {
                        // The Pokemon has been knocked out
                        if (card.ability == "Explosive" && !disabled)
                        {
                            // Damage all of the player's Pokemon
                            for (int i = 0; i < battleManager.player1_BattleField.Count; i++)
                            {
                                if (battleManager.player1_BattleField[i].health > 0)
                                {
                                    damageSystem.Damage(battleManager.player1_BattleField[i], card.attack);
                                }
                            }

                            // Damage all of the AI's Pokemon
                            for (int i = 0; i < battleManager.player2_BattleField.Count; i++)
                            {
                                if (battleManager.player2_BattleField[i].health > 0)
                                {
                                    damageSystem.Damage(battleManager.player2_BattleField[i], card.attack);
                                }
                            }

                            // Damage both of the players
                            damageSystem.Damage(battleManager.player1_Trainer, card.attack);
                            damageSystem.Damage(battleManager.player2_Trainer, card.attack);
                        }

                        if (player == 1)
                        {
                            battleManager.player1_BattleField.Remove(this);
                        }
                        else
                        {
                            battleManager.player2_BattleField.Remove(this);
                        }

                        // Remove self from field list
                        if (player == 1)
                        {
                            battleManager.player1_BattleField.Remove(this);
                        }
                        else if (player == 2)
                        {
                            battleManager.player2_BattleField.Remove(this);
                        }

                        // Add self to discard pile
                        if (player == 1)
                        {
                            battleManager.player1_DiscardPile.Add(card);
                        }
                        else if (player == 2)
                        {
                            battleManager.player2_DiscardPile.Add(card);
                        }

                        // Destroy object
                        damaged = false;
                        damageSystem.attackInProgress = false;
                        if (name == "Player1Trainer")
                        {
                            battleManager.winner = 2;
                            battleManager.BattleOver();
                        }
                        else if (name == "Player2Trainer")
                        {
                            battleManager.winner = 1;
                            battleManager.BattleOver();
                        }
                        Destroy(gameObject);
                    }
                }
                else
                {
                    damaged = false;
                    damageSystem.attackInProgress = false;
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Ability mode
        if (battleManager.abilityMode)
        {
            if (battleManager.abilityModeAbility == "Transform")
            {
                // Transform into the target
                battleManager.selectedGamePiece.GetComponent<GamePiece>().transformOriginalCard = card;
                battleManager.selectedGamePiece.GetComponent<GamePiece>().card = gameObject.GetComponent<GamePiece>().card;
                battleManager.selectedGamePiece.GetComponent<GamePiece>().transformed = true;
                battleManager.selectedGamePiece.GetComponent<GamePiece>().attack = gameObject.GetComponent<GamePiece>().attack;
                battleManager.selectedGamePiece.GetComponent<GamePiece>().health = gameObject.GetComponent<GamePiece>().health;
                battleManager.selectedGamePiece.GetComponent<GamePiece>().currentAttack = gameObject.GetComponent<GamePiece>().attack;
                battleManager.selectedGamePiece.GetComponent<GamePiece>().currentHealth = gameObject.GetComponent<GamePiece>().health;
                battleManager.selectedGamePiece.GetComponent<GamePiece>().ability = card.ability;
                battleManager.selectedGamePiece.GetComponent<GamePiece>().PokemonImage.sprite = gameObject.GetComponent<GamePiece>().PokemonImage.sprite;
            }
            else if (battleManager.abilityModeAbility == "Disable")
            {
                // Disable the target
                gameObject.GetComponent<GamePiece>().disabled = true;
            }
            else if (battleManager.abilityModeAbility == "Paralyse")
            {
                gameObject.GetComponent<GamePiece>().paralysed = true;
            }
            else if (battleManager.abilityModeAbility == "Heal")
            {
                // If the target is damaged, heal it
                if (gameObject.GetComponent<GamePiece>().currentHealth < gameObject.GetComponent<GamePiece>().card.health)
                {
                    int healthToHeal = (gameObject.GetComponent<GamePiece>().card.health - gameObject.GetComponent<GamePiece>().currentHealth);
                    if (healthToHeal <= battleManager.selectedGamePiece.health)
                    {
                        gameObject.GetComponent<GamePiece>().currentHealth = gameObject.GetComponent<GamePiece>().card.health;
                    }
                    else
                    {
                        gameObject.GetComponent<GamePiece>().currentHealth += battleManager.selectedGamePiece.health;
                    }
                }
            }

            battleManager.abilityMode = false;
            battleManager.abilityModeAbility = "";
            battleManager.abilityOverlay.SetActive(false);
            battleManager.abilityOverlay2.SetActive(false);
        }
        else
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
                        HighlightEnemies();
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
                        HighlightEnemies();
                    }
                }
                else
                {
                    // Attack the target piece with the selected game piece
                    if (battleManager.selectedGamePiece != null)
                    {
                        if (eventData.pointerPress.transform.gameObject.GetComponent<GamePiece>().targetable)
                        {
                            damageSystem.Attack(battleManager.selectedGamePiece, eventData.pointerPress.transform.gameObject.GetComponent<GamePiece>());
                        }
                        else
                        {
                            if (eventData.pointerPress.transform.gameObject.GetComponent<GamePiece>().player == 2)
                            {
                                announcementEvents.announcementCounter = 0;
                                announcementEvents.smallAnnouncement.gameObject.SetActive(true);
                                announcementEvents.smallAnnouncement.text = "You must attack guarding Pokémon first.";
                            }
                        }
                    }
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

        if (targetable)
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
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

    public void HighlightEnemies()
    {
        if (!battleManager.enemiesHighlighted)
        {
            battleManager.enemiesHighlighted = true;
            bool guardingPokemonExists = false;
            for (int i = 0; i < battleManager.player2_BattleField.Count; i++)
            {
                if (battleManager.player2_BattleField[i].guarding)
                {
                    guardingPokemonExists = true;
                    break;
                }
            }

            if (guardingPokemonExists)
            {
                for (int i = 0; i < battleManager.player2_BattleField.Count; i++)
                {
                    if (battleManager.player2_BattleField[i].guarding)
                    {
                        battleManager.player2_BattleField[i].targetable = true;
                    }
                    else
                    {
                        battleManager.player2_BattleField[i].targetable = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < battleManager.player2_BattleField.Count; i++)
                {
                    battleManager.player2_BattleField[i].targetable = true;
                }
                battleManager.player2_Trainer.targetable = true;
            }
        }
        else
        {
            battleManager.enemiesHighlighted = false;
            for (int i = 0; i < battleManager.player2_BattleField.Count; i++)
            {
                battleManager.player2_BattleField[i].targetable = false;
                battleManager.player2_BattleField[i].GetComponent<Image>().color = new Color32(255, 0, 0, 0);
            }
            battleManager.player2_Trainer.targetable = false;
            battleManager.player2_Trainer.GetComponent<Image>().color = new Color32(255, 0, 0, 0);
        }
    }
}
