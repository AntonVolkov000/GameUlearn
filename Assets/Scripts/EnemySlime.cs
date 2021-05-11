using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemySlime : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public bool barrierMove;
    public float radiusMove;
    public float inaccuracy;
    public PlayerController player;
    public Vector2 radiusTriggerMove;
    public Vector2 radiusTriggerAttack;
    public float addRangeAttack;
    public float countHealth;
    public int countDamage;
    
    private float oldSpeed;
    private bool isTrigger;
    private bool isGetDamage;
    private Vector3 position;
    private SpriteRenderer sprite;
    private Animator animator;

    private void Start()
    {
        oldSpeed = speed;
        position = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        animator = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        if (countHealth == 0) return;
        var playerPosition = player.transform.position;
        if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerMove))
        {
            isTrigger = true;
            if (!isGetDamage)
                StartTrigger();
        }
        else
        {
            isTrigger = false;
            if (speed == 0)
                speed = oldSpeed;
            animator.Play("SlimeWalking");
            if (!barrierMove && Math.Abs(position.x - transform.position.x) >= radiusMove - inaccuracy)
            {
                if (transform.position.x < position.x)
                {
                    speed = Math.Abs(speed);
                    sprite.flipX = true;
                }
                else
                {
                    speed = Math.Abs(speed) * -1;
                    sprite.flipX = false;
                }
            }
            transform.Translate(direction.normalized * speed / 8);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (countHealth == 0) return;
        if (!isTrigger && barrierMove && coll.gameObject.CompareTag("Barrier"))
        {
            speed *= -1;
            sprite.flipX = !sprite.flipX;
        }
        if (!coll.gameObject.CompareTag("Player") || !(player.feetPos.position.y > transform.position.y)) return;
        isGetDamage = true;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 7);
        countHealth--;
        if (countHealth == 0)
        {
            speed = 0;
            animator.Play("SlimeDeath");
        }
        else
        {
            if (speed != 0)
                oldSpeed = speed;
            speed = 0;
            animator.Play("SlimeDamageByJumpedOn");
            StartCoroutine(WaitDamage());
        }
    }
    
    private void StartTrigger()
    {
        if (!isTrigger) return;
        var playerPosition = player.transform.position;
        if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerAttack))
        {
            if (speed != 0)
                oldSpeed = speed;
            speed = 0;
            animator.Play("SlimeAttack");
        }
        else
        {
            if (speed == 0)
                speed = oldSpeed;
            animator.Play("SlimeWalking");
            if (playerPosition.x < transform.position.x)
            {
                speed = Math.Abs(speed) * -1;
                sprite.flipX = false;
            }
            else
            {
                speed = Math.Abs(speed);
                sprite.flipX = true;
            }
            transform.Translate(direction.normalized * speed / 8);
        }
    }
    
    private bool PlayerInsideRadius(Vector3 playerPosition,Vector2 positionCurrentObj,Vector2 radiusTrigger)
    {
        return Math.Abs(playerPosition.x - positionCurrentObj.x) < radiusTrigger.x &&
               Math.Abs(playerPosition.y - positionCurrentObj.y) < radiusTrigger.y;
    }
    
    private void Attack()
    {
        
        var playerPosition = player.transform.position;
        var positionCurrentObj = transform.position;
        if (!sprite.flipX && playerPosition.x > positionCurrentObj.x ||
            sprite.flipX && playerPosition.x < positionCurrentObj.x ) return;
        if (!PlayerInsideRadius(player.transform.position, transform.position,
            new Vector2(radiusTriggerAttack.x + addRangeAttack, radiusTriggerAttack.y))) return;
        var moveX = -5;
        player.GetDamage = true;
        if (sprite.flipX)
            moveX = Math.Abs(moveX);
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX, 5);
        player.TakeDamage(countDamage);
    }
    
    private void Death() {
        gameObject.SetActive(false);
    }
    
    private IEnumerator WaitDamage() {
        yield return new WaitForSeconds(0.5f);
        isGetDamage = false;
    }
}
