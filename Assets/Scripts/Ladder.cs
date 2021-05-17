using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public PlayerController player;
    public float speed = 5;
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerController.inLadder = true;
            other.GetComponent<Animator>().Play("PlayerStairs");
            other.GetComponent<Rigidbody2D>().gravityScale = 0;
            player.GetDamage = false;
            if (Input.GetKey(KeyCode.W))
                other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
            else if (Input.GetKey(KeyCode.S))
                other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
            else
                other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.inLadder = false;
            other.GetComponent<Rigidbody2D>().gravityScale = player.gravityScale;
        }
    }
}
