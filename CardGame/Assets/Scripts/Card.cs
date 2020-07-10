using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    //public new string name;
    public string type;
    public AudioClip cry;
    public Sprite sprite;
    public int attack;
    public int health;
    public int energy;
}
