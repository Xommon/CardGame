using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string type;
    public string weaknessType;
    public string resistanceType;
    public AudioClip cry;
    public int dexNumber;
    public Sprite sprite;
    public Sprite background;
    public int attack;
    public int health;
    public int energy;
    public string ability;
    public bool legendary;
}
