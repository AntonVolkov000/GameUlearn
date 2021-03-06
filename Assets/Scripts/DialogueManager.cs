using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager: MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;
    public bool isEnd;

    private Queue<Sentence> sentences;
    private bool startDialogue;
    private Dialogue dialogue;
    private float speedWriteText;

    private void Start()
    {
        sentences = new Queue<Sentence>();
    }

    public void StartDialogue(Dialogue dialogue, float speedWriteText)
    {
        this.speedWriteText = speedWriteText;
        animator.SetBool("isOne", true);

        this.dialogue = dialogue;
        PlayerController.inDialogue = true;
        
        sentences.Clear();
        startDialogue = true;

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        var sentence = sentences.Dequeue();
        nameText.text = sentence.name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence.sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        if (startDialogue)
        {
            dialogueText.text = sentence;
            startDialogue = false;
        }
        else
        {
            dialogueText.text = "";
            foreach (var letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(speedWriteText);
            }
        }
    }

    private void EndDialogue()
    {
        PlayerController.inDialogue = false;
        PlayerController.isAttackMagic = false;
        animator.SetBool("isOne", false);
        isEnd = true;
    }
}
