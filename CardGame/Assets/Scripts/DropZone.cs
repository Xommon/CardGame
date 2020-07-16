using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject gamePiece;
    public BattleManager battleManager;

    void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        battleManager.isDragging = false;

        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null)
        {
            // Convert the player's card into a game piece if there's enough room on the field, if the player has the energy to play the card, and it's the player's turn
            if (battleManager.player1_BattleField.Count < 6 && battleManager.playerTurn == 1 && battleManager.player1_CurrentEnergy >= eventData.pointerDrag.GetComponent<CardFace>().card.energy)
            {
                battleManager.player1_CurrentEnergy -= eventData.pointerDrag.GetComponent<CardFace>().card.energy;
                draggable.parentToReturnTo = this.transform;
                GameObject newGamePiece = Instantiate(gamePiece, Vector3.zero, Quaternion.identity, FindObjectOfType<DropZone>().transform);
                newGamePiece.GetComponent<GamePiece>().card = eventData.pointerDrag.GetComponent<CardFace>().card;
                newGamePiece.GetComponent<GamePiece>().player = 1;
                battleManager.player1_BattleField.Add(newGamePiece.GetComponent<GamePiece>());
                battleManager.player2_BattleField.Add(newGamePiece.GetComponent<GamePiece>());
                /*if (eventData.pointerDrag.gameObject.GetComponent<Draggable>().placeHolder != null)
                {
                    Destroy(eventData.pointerDrag.gameObject.GetComponent<Draggable>().placeHolder);
                }*/
                if (eventData.pointerDrag.gameObject.GetComponent<Draggable>().placeHolder2 != null)
                {
                    Destroy(eventData.pointerDrag.gameObject.GetComponent<Draggable>().placeHolder2);
                }
                Destroy(eventData.pointerDrag.gameObject);
            }
        }
    }
}
