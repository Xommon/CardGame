using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFace : MonoBehaviour
{
    public Card card;
    public TextMesh nameText;
    public TextMesh typeText;
    public AudioClip cryAudio;
    public Sprite cardImage;
    public TextMesh attackText;
    public TextMesh healthText;
    public TextMesh energyText;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = card.name;
        typeText.text = card.type;
        cryAudio = card.cry;
        cardImage = card.image;
        attackText.text = card.attack.ToString();
        healthText.text = card.health.ToString();
        energyText.text = card.energy.ToString();
    }
}
