using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<string> abilitiesList = new List<string>();
    public Dictionary<string, string> abilities = new Dictionary<string, string>()
    {
        { "", "" },
        { "Paralyse", "When played, choose an opposing Pokémon to paralyse for one turn." },
        { "Whirlwind", "The weather is always windy while this Pokémon is in play." },
        { "Draw Card", "When played, draw a card." },
        { "Meteor Shower", "The weather is always meteor shower while this Pokémon is in play." },
        { "Run Away", "When this Pokémon has 1 health remaining, it is returned to the player's hand." },
        { "Toxic", "Any opposing Pokémon who attacks or is attacked by this Pokémon is knocked out." },
        { "Protect", "The first attack on this Pokémon does no damage." },
        { "Quick", "This Pokémon can attack immediately after being played." },
        { "Guard", "This Pokémon must be knocked out before oppossing Pokémon can attack other friendly Pokémon." },
        { "Disable", "When played, choose an opposing Pokémon and disable its ability permanently." },
        { "Explosive", "When knocked out, this Pokémon deals damage equivalent to its attack to all Pokémon and trainers on the field." },
        { "Heal", "When played, choose a friendly Pokémon to heal equal to this Pokémon's health." },
        { "Transform", "When played, choose any Pokémon to turn into." },
        { "Convert", "When played, choose any type to turn into." },
        { "Delay", "This Pokémon cannot attack for two turns once played." },
        { "Drought", "The weather is always sunny while this Pokémon is in play." },
        { "Flood", "The weather is always rainy while this Pokémon is in play." },
        { "Sand Stream", "The weather is always sandstorm while this Pokémon is in play." },
        { "Blizzard", "The weather is always snowy while this Pokémon is in play." },
        { "Air Lock", "The weather is always clear while this Pokémon is in play." },
        { "Overcast", "The weather is always cloudy while this Pokémon is in play." },
        { "Stormy", "The weather is always stormy while this Pokémon is in play." },
        //{ "Forecast", "Changes form with the weather." },
        //{ "Wonder Guard", "This Pokémon can only be damaged by its weakness type." },
        //{ "Morph", "Choose form when played." },
    };


    void Start()
    {
        // Populate abilities
        foreach (KeyValuePair<string, string> s in abilities)
        {
            abilitiesList.Add(s.Key);
        }
    }
}
