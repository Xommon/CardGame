using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int offsetX = 2, offsetY = 2;
    public TextMeshProUGUI text;
    public Vector3 startPosition;
    public RectTransform textRect;
    public float height;
    public bool pressed;

    void Start()
    {
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        textRect = text.GetComponent<RectTransform>();
        height = text.transform.position.y;
        pressed = false;
        if (gameObject.GetComponent<Button>().interactable == false)
        {
            gameObject.GetComponent<Button>().interactable = true;
            startPosition = textRect.localPosition;
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            startPosition = textRect.localPosition;
        }
    }

    void Update()
    {
        if (!pressed)
        {
            if (gameObject.GetComponent<Button>().interactable == false)
            {
                text.color = new Color32(162, 144, 114, 255);
                if (name.Substring(0, 4) == "Plus" || name.Substring(0, 5) == "Minus")
                {
                    textRect.localPosition = new Vector3(startPosition.x, startPosition.y - ((float)offsetY * 2.5f), startPosition.z);
                }
                else if (name.Substring(0, 4) == "Edit")
                {
                    textRect.localPosition = new Vector3(startPosition.x, startPosition.y - ((float)offsetY * 9.0f), startPosition.z);
                }
                else
                {
                    textRect.localPosition = new Vector3(startPosition.x, startPosition.y - ((float)offsetY * 10.5f), startPosition.z);
                }
            }
            else
            {
                text.color = new Color32(255, 255, 255, 255);
                textRect.localPosition = startPosition;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Button>().interactable)
        {
            pressed = true;
            text.color = new Color32(162, 144, 114, 255);
            if (name.Substring(0, 4) == "Plus" || name.Substring(0, 5) == "Minus")
            {
                textRect.localPosition = new Vector3(startPosition.x, startPosition.y - ((float)offsetY * 2.5f), startPosition.z);
            }
            else if (name.Substring(0, 4) == "Edit")
            {
                textRect.localPosition = new Vector3(startPosition.x, startPosition.y - ((float)offsetY * 9.0f), startPosition.z);
            }
            else
            {
                textRect.localPosition = new Vector3(startPosition.x, startPosition.y - ((float)offsetY * 10.5f), startPosition.z);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        text.color = new Color32(255, 255, 255, 255);
        textRect.localPosition = startPosition;
    }
}