using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking.PlayerConnection;

public class DialogueTrigger : MonoBehaviour, IPointerClickHandler
{
    public Dialogue dialogue;
    public PlayerController player;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var playerComponent = player.GetComponent<PlayerController>();
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) <
            playerComponent.maxDistanceToNeutral.x &&
            Math.Abs(player.transform.position.y - this.gameObject.transform.position.y) <
            playerComponent.maxDistanceToNeutral.y)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, player.speedWriteText);
        }
    }
}
