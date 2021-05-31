using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public Dialogue dialogue;
    public PlayerController player;
    
    private void FixedUpdate()
    {
        if (FindObjectOfType<DialogueManager>().isEnd)
        {
            player.currentHealth = 0;
        }
    }
}
