using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class CardFace : MonoBehaviour
{
    public Card card;
    public TextMesh nameText;
    public TextMesh typeText;
    public AudioClip cryAudio;
    public Image cardImage;
    public TextMesh attackText;
    public TextMesh healthText;
    public TextMesh energyText;

    // Start is called before the first frame update
    void Start()
    {
        // Values to appear on the card's face
        nameText.text = Path.GetFileName("Assets/Cards/" + card.ToString().Substring(0, card.name.Length));
        typeText.text = card.type;
        cryAudio = card.cry;
        cardImage.sprite = card.sprite;
        attackText.text = card.attack.ToString();
        healthText.text = card.health.ToString();
        energyText.text = card.energy.ToString();
    }
}
