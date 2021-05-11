using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpell : MonoBehaviour
{
    public float speed;
    public PlayerController player;

    private bool isFly;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("MageMagicSpellStart");
    }

    private void FixedUpdate()
    {
        if (!isFly) return;
        animator.Play("MageMagicSpellFly");
        transform.Translate(Vector2.right * speed / 8);
    }
    
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Ladder")) return;
        isFly = false;
        speed = 0;
        animator.Play("MageMagicSpellEnd");
        if (!coll.gameObject.CompareTag("Player")) return;
        var moveX = -5;
        player.GetDamage = true;
        if (player.transform.position.x > transform.position.x)
            moveX = Math.Abs(moveX);
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX, 5);
    }
    
    private void Fly()
    {
        isFly = true;
    }

    private void DestroySpell()
    {
        Destroy(gameObject);
    }
}
