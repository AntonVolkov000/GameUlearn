using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public bool taken;
    public int addHealth;
    public PlayerController player;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !taken)
        {
            player.GetHealth(addHealth);
            gameObject.SetActive(false);
            taken = true;
        }
    }
}