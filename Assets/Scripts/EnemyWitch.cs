using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWitch : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public PlayerController player;
    public Vector2 radiusTriggerMove;
    public Vector2 radiusTriggerAttack;
    public float addRangeAttack;
    public float countHealth;
    public int countDamage;
    
    private float oldSpeed;
    private bool isTrigger;
    private bool isAttack;
    private bool isGetDamage;
    private SpriteRenderer sprite;
    private Animator animator;
    
    public void GetDamage()
    {
        if (countHealth == 0) return;
        isGetDamage = true;
        countHealth--;
        animator.Play("WitchDamage");
        StartCoroutine(WaitDamage());
        isAttack = false;
    }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        animator = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        if (countHealth == 0)
        {
            animator.Play("WitchDeath");
            return;
        }
        if (isAttack || isGetDamage) return;
        var playerPosition = player.transform.position;
        if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerMove))
        {
            if (playerPosition.x < transform.position.x)
            {
                speed = Math.Abs(speed) * -1;
                sprite.flipX = true;
            }
            else
            {
                speed = Math.Abs(speed);
                sprite.flipX = false;
            }
            StartMove();
        }
        else
        {
            animator.Play("WitchIdle");
            if (speed != 0)
                oldSpeed = speed;
            speed = 0;
        }
    }

    private void StartMove()
    {
        var playerPosition = player.transform.position;
        if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerAttack))
        {
            if (speed != 0)
                oldSpeed = speed;
            speed = 0;
            animator.Play("WitchCharge");
        }
        else
        {
            if (speed == 0)
                speed = oldSpeed;
            animator.Play("WitchRun");
            transform.Translate(direction.normalized * speed / 8);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("PlayerSpell"))
            GetDamage();
    }

    private void StartAttack()
    {
        animator.Play("WitchAttack");
        isAttack = true;
    }
    
    private void IsAttack()
    {
        isAttack = false;
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
        if (!sprite.flipX && playerPosition.x < positionCurrentObj.x ||
            sprite.flipX && playerPosition.x > positionCurrentObj.x ) return;
        if (!PlayerInsideRadius(player.transform.position, transform.position,
            new Vector2(radiusTriggerAttack.x + addRangeAttack, radiusTriggerAttack.y))) return;
        var moveX = -5;
        player.GetDamage = true;
        if (!sprite.flipX)
            moveX = Math.Abs(moveX);
        player.TakeDamage(countDamage, new Vector2(moveX, 5));
    }

    private void Death() {
        gameObject.SetActive(false);
    }
    
    private IEnumerator WaitDamage() {
        yield return new WaitForSeconds(0.5f);
        isGetDamage = false;
        isAttack = false;
    }
}
