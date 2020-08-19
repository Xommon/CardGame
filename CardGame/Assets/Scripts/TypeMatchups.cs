using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class TypeMatchups : MonoBehaviour
{
    public List<string> types = new List<string>();
    public List<Sprite> typesImages = new List<Sprite>();
    public Dictionary<string, string> weaknesses = new Dictionary<string, string>() 
    {
        { "Normal", "" },
        { "Sound", "Cosmic" },
        { "Cosmic", "Dragon" },
        { "Dragon", "Fairy" },
        { "Fairy", "Poison" },
        { "Poison", "Psychic" },
        { "Psychic", "Bug" },
        { "Ghost", "Dark" },
        { "Dark", "Bug" },
        { "Bug", "Flying" },
        { "Flying", "Rock" },
        { "Rock", "Grass" },
        { "Grass", "Fire" },
        { "Fire", "Water" },
        { "Water", "Electric" },
        { "Electric", "Ground" },
        { "Ground", "Ice" },
        { "Ice", "Steel" },
        { "Steel", "Fighting" },
        { "Fighting", "Sound" },
    };
    public Dictionary<string, string> resistances = new Dictionary<string, string>()
    {
        { "Normal", "" },
        { "Sound", "" },
        { "Cosmic", "Sound" },
        { "Dragon", "" },
        { "Fairy", "Dark" },
        { "Poison", "" },
        { "Psychic", "Fighting" },
        { "Ghost", "Normal" },
        { "Dark", "Psychic" },
        { "Bug", "" },
        { "Flying", "Ground" },
        { "Rock", "" },
        { "Grass", "" },
        { "Fire", "" },
        { "Water", "" },
        { "Electric", "" },
        { "Ground", "Electric" },
        { "Ice", "" },
        { "Steel", "Poison" },
        { "Fighting", "" },
    };

    void Start()
    {
        types.Add("Bug");
        types.Add("Cosmic");
        types.Add("Dark");
        types.Add("Dragon");
        types.Add("Electric");
        types.Add("Fairy");
        types.Add("Fighting");
        types.Add("Fire");
        types.Add("Flying");
        types.Add("Ghost");
        types.Add("Grass");
        types.Add("Ground");
        types.Add("Ice");
        types.Add("Normal");
        types.Add("Poison");
        types.Add("Psychic");
        types.Add("Rock");
        types.Add("Sound");
        types.Add("Steel");
        types.Add("Water");
    }

    public string GetWeakness(string type)
    {
        return weaknesses[type];
    }

    public string GetResistance(string type)
    {
        return resistances[type];
    }
}
