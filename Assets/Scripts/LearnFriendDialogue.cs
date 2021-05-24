using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LearnFriendDialogue : MonoBehaviour, IPointerClickHandler
{
    public PlayerController player;
    public GameObject buttonContinue;
    public GameObject ladderAndChest;
    public DialogueManager dialogueManager;
    public TwoPartDialogue twoPartDialogue;

    private void FixedUpdate()
    {
        if (dialogueManager.isEnd)
        {
            buttonContinue.SetActive(false);
            player.Dialogue = true;
            ladderAndChest.SetActive(true);
            dialogueManager.isEnd = false;
            player.isLearnAttack = false;
            twoPartDialogue.StartDialogue();
            twoPartDialogue.Dialogue = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonContinue.SetActive(true);
        player.isLearnAttack = true;
    }
}
