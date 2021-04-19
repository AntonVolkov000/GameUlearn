using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public Animator animator;
    
    private Queue<string> sentences;
    private bool startDialogue;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOne", true);

        nameText.text = dialogue.name;
        PlayerController.inDialogue = true;
        
        sentences.Clear();
        startDialogue = true;

        foreach (var sentence in dialogue.sentences)
            sentences.Enqueue(sentence);
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        var sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
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
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void EndDialogue()
    {
        PlayerController.inDialogue = false;
        animator.SetBool("isOne", false);
    }
}
