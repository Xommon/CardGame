using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform parentToReturnTo = null;
    public GameObject placeHolder = null;
    public GameObject placeHolder2 = null;
    public GameObject card;
    public int counter;
    public GameObject cardDetails_Left;
    public GameObject cardDisplay_Left;
    public GameObject abilityDetails1_Left;
    public GameObject ability1_name_Left;
    public GameObject ability1_description_Left;
    public BattleManager battleManager;

    public void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        cardDetails_Left = GameObject.Find("CardInformationLeft");
        cardDisplay_Left = GameObject.Find("CardExampleDisplayLeft");
        abilityDetails1_Left = GameObject.Find("AbilityInfo1Left");
        ability1_name_Left = GameObject.Find("/AbilityInfo1Left/AbilityName");
        ability1_description_Left = GameObject.Find("/AbilityInfo1Left/AbilityDescription");
        parentToReturnTo = this.transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Enable dragging
        battleManager.isDragging = true;

        // Unselect any game pieces that may have been selected before
        for (int i = 0; i < battleManager.player1_BattleField.Count; i++)
        {
            if (battleManager.player1_BattleField[i].isSelected)
            {
                battleManager.player1_BattleField[i].isSelected = false;
                battleManager.player1_BattleField[i].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            }
        }

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Drag the card with the pointer
        this.transform.position = eventData.position;

        // If there are no cards found to the right, then the card must be going to the beginning of the hand
        int newSiblingIndex = parentToReturnTo.childCount;

        // Constantly check for cards to the left
        for (int i = 0; i < parentToReturnTo.childCount; i++)
        {
            if (this.transform.position.x < parentToReturnTo.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (placeHolder2.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }

                break;
            }
        }

        placeHolder2.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        battleManager.sentIsNotDragging = true;
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeHolder2.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // Return the card back to its original size
        gameObject.transform.localScale = new Vector3(1.0f, 0.98f, gameObject.transform.localScale.z);
        Destroy(GameObject.Find("New Game Object"));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Dispay details of the card when you hover over it
        if (!battleManager.isDragging)
        {
            // Create a dummy object to take the place of the moused over object
            placeHolder2 = new GameObject();
            placeHolder2.transform.SetParent(this.transform.parent);
            LayoutElement le = placeHolder2.AddComponent<LayoutElement>();
            le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
            le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
            le.flexibleWidth = 0;
            le.flexibleHeight = 0;
            //le.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
            placeHolder2.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            // Remove card from hand temporarily
            parentToReturnTo = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);

            // Edit the size and position of the card moused over
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(gameObject.transform.position.x, -500, gameObject.transform.position.z);
            gameObject.transform.localScale = new Vector3(1.5f, 1.47f, gameObject.transform.localScale.z);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!battleManager.isDragging)
        {
            // Put the card back into the slot where the dummy was
            Destroy(GameObject.Find("New Game Object"));
            this.transform.SetParent(parentToReturnTo);
            this.transform.SetSiblingIndex(placeHolder2.transform.GetSiblingIndex());

            // Return the card back to its original size
            gameObject.transform.localScale = new Vector3(1.0f, 0.98f, gameObject.transform.localScale.z);
        }  
    }
}
