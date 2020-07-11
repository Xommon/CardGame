using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    //public new string name;
    public string type;
    public AudioClip cry;
    public int dexNumber;
    public Sprite sprite;
    public Sprite background;
    public int attack;
    public int health;
    public int energy;
    public string ability;
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
}
