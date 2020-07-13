using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject gamePiece;
    public List<GamePiece> field1 = new List<GamePiece>();
    public List<GamePiece> field2 = new List<GamePiece>();

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null)
        {
            // Convert the player's card into a game piece if there's enough room on the field
            if (field1.Count < 6)
            {
                GameObject newGamePiece = Instantiate(gamePiece, Vector3.zero, Quaternion.identity, FindObjectOfType<DropZone>().transform);
                newGamePiece.GetComponent<GamePiece>().card = eventData.pointerDrag.GetComponent<CardFace>().card;
                field1.Add(newGamePiece.GetComponent<GamePiece>());
                Destroy(eventData.pointerDrag.gameObject);
            }
        }
    }
}
