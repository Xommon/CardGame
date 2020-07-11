using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string type;
    public AudioClip cry;
    public int dexNumber;
    public Sprite sprite;
    public Sprite background;
    public int attack;
    public int health;
    public int energy;
    public string ability;
    public bool legendary;
    /*paralyse
    toxic
    protect
    quick
    guard
    disable
    explode
    heal
    transform
    convert*/

    [MenuItem("GameObject/Create New Card")]
    public void CreateCustomCard(string name, string type, AudioClip cry, int dexNumber, Sprite sprite, Sprite background, int attack, int health, int energy, string ability)
    {
        Card customCard = new Card();
        customCard.cry = null;
        customCard.dexNumber = 152;
        customCard.sprite = sprite;
        customCard.background = background;
        customCard.attack = attack;
        customCard.health = health;
        customCard.energy = energy;
        customCard.ability = ability;
        AssetDatabase.CreateAsset(customCard, "Assets/Cards/" + name + ".asset");
    }
}
