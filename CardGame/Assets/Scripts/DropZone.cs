using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public GameObject gamePiece;
    public BattleManager battleManager;
    public AnnouncementEvents announcementEvents;

    void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        announcementEvents = FindObjectOfType<AnnouncementEvents>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        battleManager.isDragging = false;

        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null)
        {
            // Convert the player's card into a game piece if there's enough room on the field, if the player has the energy to play the card, and it's the player's turn
            if (battleManager.playerTurn != 1)
            {
                announcementEvents.announcementCounter = 0;
                announcementEvents.smallAnnouncement.gameObject.SetActive(true);
                announcementEvents.smallAnnouncement.text = "It is not your turn.";
            }
            else if (battleManager.player1_CurrentEnergy < eventData.pointerDrag.GetComponent<CardFace>().card.energy)
            {
                announcementEvents.announcementCounter = 0;
                announcementEvents.smallAnnouncement.gameObject.SetActive(true);
                announcementEvents.smallAnnouncement.text = "You don't have enough energy to play this card.";
            }
            else if (battleManager.player1_BattleField.Count == 6)
            {
                announcementEvents.announcementCounter = 0;
                announcementEvents.smallAnnouncement.gameObject.SetActive(true);
                announcementEvents.smallAnnouncement.text = "You can only have 6 Pokémon on your side of the field at a time.";
            }
            else
            {
                battleManager.player1_CurrentEnergy -= eventData.pointerDrag.GetComponent<CardFace>().card.energy;
                draggable.parentToReturnTo = this.transform;
                GameObject newGamePiece = Instantiate(gamePiece, Vector3.zero, Quaternion.identity, FindObjectOfType<DropZone>().transform);
                newGamePiece.GetComponent<GamePiece>().card = eventData.pointerDrag.GetComponent<CardFace>().card;
                newGamePiece.GetComponent<GamePiece>().player = 1;
                battleManager.player1_BattleField.Add(newGamePiece.GetComponent<GamePiece>());
                if (eventData.pointerDrag.gameObject.GetComponent<Draggable>().placeHolder2 != null)
                {
                    Destroy(eventData.pointerDrag.gameObject.GetComponent<Draggable>().placeHolder2);
                }
                battleManager.player1_Hand.Remove(newGamePiece.GetComponent<GamePiece>().card);
                Destroy(eventData.pointerDrag.gameObject);

                // Activate abilities
                if (newGamePiece.GetComponent<GamePiece>().card.ability == "Protect")
                {
                    // Shield this Pokemon
                    newGamePiece.GetComponent<GamePiece>().shielded = true;
                }
                else if (newGamePiece.GetComponent<GamePiece>().card.ability == "Guard")
                {
                    // This Pokemon is guarding
                    newGamePiece.GetComponent<GamePiece>().guarding = true;
                }
                else if (newGamePiece.GetComponent<GamePiece>().card.ability == "Toxic")
                {
                    // This Pokemon is toxic
                    newGamePiece.GetComponent<GamePiece>().toxic = true;
                }
                else if (newGamePiece.GetComponent<GamePiece>().card.ability == "Convert")
                {
                    // Convert your Pokemon type
                    battleManager.selectedGamePiece = newGamePiece.GetComponent<GamePiece>();
                    battleManager.abilityOverlay.SetActive(true);
                    battleManager.conversionPrompt.SetActive(true);
                }
                else if(newGamePiece.GetComponent<GamePiece>().card.ability == "Heal")
                    {
                    // Heal a Pokemon
                    battleManager.selectedGamePiece = newGamePiece.GetComponent<GamePiece>();
                    battleManager.abilityOverlay2.SetActive(true);
                    announcementEvents.announcementCounter = 0;
                    announcementEvents.smallAnnouncement.gameObject.SetActive(true);
                    announcementEvents.smallAnnouncement.text = "Choose a Pokémon to heal.";
                    battleManager.abilityMode = true;
                    battleManager.abilityModeAbility = "Heal";
                }
                else if (newGamePiece.GetComponent<GamePiece>().card.ability == "Transform" && ((battleManager.player1_BattleField.Count + battleManager.player2_BattleField.Count) > 0))
                {
                    // Disable a Pokemon
                    battleManager.selectedGamePiece = newGamePiece.GetComponent<GamePiece>();
                    battleManager.abilityOverlay.SetActive(true);
                    announcementEvents.announcementCounter = 0;
                    announcementEvents.smallAnnouncement.gameObject.SetActive(true);
                    announcementEvents.smallAnnouncement.text = "Choose a Pokémon to transform into.";
                    battleManager.abilityMode = true;
                    battleManager.abilityModeAbility = "Transform";
                }
                else if (battleManager.player2_BattleField.Count > 0)
                {
                    if (newGamePiece.GetComponent<GamePiece>().card.ability == "Paralyse")
                    {
                        // Paralyse a Pokemon
                        battleManager.abilityOverlay.SetActive(true);
                        announcementEvents.announcementCounter = 0;
                        announcementEvents.smallAnnouncement.gameObject.SetActive(true);
                        announcementEvents.smallAnnouncement.text = "Choose a Pokémon to paralyse.";
                        battleManager.abilityMode = true;
                        battleManager.abilityModeAbility = "Paralyse";
                    }
                    else if (newGamePiece.GetComponent<GamePiece>().card.ability == "Disable")
                    {
                        // Disable a Pokemon
                        battleManager.abilityOverlay.SetActive(true);
                        announcementEvents.announcementCounter = 0;
                        announcementEvents.smallAnnouncement.gameObject.SetActive(true);
                        announcementEvents.smallAnnouncement.text = "Choose a Pokémon to disable.";
                        battleManager.abilityMode = true;
                        battleManager.abilityModeAbility = "Disable";
                    }
                }
            }
        }
    }
}
