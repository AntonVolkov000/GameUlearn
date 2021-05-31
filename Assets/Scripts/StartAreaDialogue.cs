using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAreaDialogue : DialogueTrigger
{
    private bool endDialogue;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !endDialogue)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue,
                other.GetComponent<PlayerController>().speedWriteText);
            endDialogue = true;
        }
    }
}
