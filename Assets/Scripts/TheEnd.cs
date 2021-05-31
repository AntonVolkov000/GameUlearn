using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TheEnd : MonoBehaviour
{
    public Dialogue dialogue;
    public PlayerController player;
    public Animator animator;
    
    private void Start()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, player.speedWriteText);
    }

    private void FixedUpdate()
    {
        if (FindObjectOfType<DialogueManager>().isEnd)
        {
            animator.Play("TheEnd");
        }
    }
}
