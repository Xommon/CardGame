using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonText : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI text;
    public Vector3 startPosition;
    public float height;
    public bool pressed;

    void Start()
    {
        startPosition = text.transform.position;
        height = gameObject.GetComponent<RectTransform>().sizeDelta.y;
        pressed = false;
    }

    void Update()
    {
        if (!pressed)
        {
            if (gameObject.GetComponent<Button>().interactable == false)
            {
                text.color = new Color32(162, 144, 114, 255);
                //text.GetComponent<RectTransform>().position = new Vector3(text.GetComponent<RectTransform>().position.x, height/20, text.GetComponent<RectTransform>().position.z);
            }
            else
            {
                text.color = new Color32(255, 255, 255, 255);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        text.color = new Color32(162, 144, 114, 255);
        //text.GetComponent<RectTransform>().position = new Vector3(text.transform.position.x, text.transform.position.y - 10, text.transform.position.z);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        text.color = new Color32(255, 255, 255, 255);
        //text.GetComponent<RectTransform>().position = new Vector3(text.transform.position.x, text.transform.position.y - 10, text.transform.position.z); ;
    }
}
