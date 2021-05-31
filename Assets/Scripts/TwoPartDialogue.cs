using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPartDialogue : DialogueTrigger
{
    public DialogueManager dialogueManager;
    public GameObject obelisk;
    public GameObject thunder;
    
    public bool Dialogue;
    private bool OpenChest;

    public void StartDialogue()
    {
        dialogueManager.StartDialogue(dialogue, player.speedWriteText);
    }
    
    private void FixedUpdate()
    {
        if (!Dialogue) return;
        if (!player.OpenChest)
        {
            CompleteOpenChest();
            return;
        }
        if (!OpenChest)
        {
            dialogueManager.DisplayNextSentence();
            OpenChest = true;
            thunder.SetActive(true);
            obelisk.SetActive(true);
        }
    }
    
    private void CompleteOpenChest()
    {
        if (!player.OpenChest && player.CountShards > 0)
            player.OpenChest = true;
    }
}
