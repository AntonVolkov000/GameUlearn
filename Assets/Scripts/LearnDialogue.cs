using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnDialogue : DialogueTrigger
{
    public DialogueManager dialogueManager;
    public GameObject enemySlime;
    public GameObject frienStone;

    private bool MoveRightAndLeft;
    private bool Jump;
    private bool Attack;
    private bool DeadEnemy;

    private void Start()
    {
        player.isLearn = true;
        player.isLearnAttack = true;
    }
    
    private void FixedUpdate()
    {
        if ((!player.MoveLeft || !player.MoveRight) && !MoveRightAndLeft)
        {
            CompleteMove();
            return;
        }
        if (!MoveRightAndLeft)
        {
            dialogueManager.DisplayNextSentence();
            MoveRightAndLeft = true;
        }
        if (!player.Jump && !Jump)
        {
            CompleteJump();
            return;
        }
        if (!Jump)
        {
            dialogueManager.DisplayNextSentence();
            Jump = true;
            player.isLearnAttack = false;
        }

        if ((!player.HandAttack || !player.MagicAttack) && !Attack)
        {
            CompleteAttack();
            return;
        }
        if (!Attack)
        {
            dialogueManager.DisplayNextSentence();
            Attack = true;
            enemySlime.SetActive(true);
        }
        if (!player.DeadEnemy && !DeadEnemy)
        {
            CompleteEnemy();
            return;
        }
        if (!DeadEnemy)
        {
            dialogueManager.DisplayNextSentence();
            DeadEnemy = true;
            frienStone.SetActive(true);
        }
    }

    private void CompleteMove()
    {
        if (!player.MoveLeft && Input.GetKey(KeyCode.A))
            player.MoveLeft = true;
        if (!player.MoveRight && Input.GetKey(KeyCode.D))
            player.MoveRight = true;
    }
    
    private void CompleteJump()
    {
        if (!player.Jump && Input.GetKey(KeyCode.Space))
            player.Jump = true;
    }
    
    private void CompleteAttack()
    {
        if (!player.HandAttack && Input.GetMouseButton(1))
            player.HandAttack = true;
        if (!player.MagicAttack && Input.GetMouseButton(0))
            player.MagicAttack = true;
    }
    
    private void CompleteEnemy()
    {
        if (!player.DeadEnemy && !enemySlime.activeSelf)
            player.DeadEnemy = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue,
                other.GetComponent<PlayerController>().speedWriteText);
        }
    }
}