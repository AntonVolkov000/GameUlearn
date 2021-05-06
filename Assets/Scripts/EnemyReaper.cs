using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReaper : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public PlayerController player;
    public Vector2 radiusTriggerMove;
    public Vector2 radiusTriggerAttack;
    public float addRangeAttack;
    public float countHealth;
    
    private float oldSpeed;
    private bool isTrigger;
    private bool isAttack;
    private bool isAttackComplete;
    private SpriteRenderer sprite;
    private Animator animator;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        animator = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        if (countHealth == 0 || isAttackComplete) return;
        var playerPosition = player.transform.position;
        if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerMove))
        {
            if (!isTrigger)
                animator.Play("ReaperWieldWeapon");
            if (isAttack)
                StartAttack();
            isTrigger = true;
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
        }
        else
        {
            animator.Play("ReaperPassiveIdle");
            if (speed != 0)
                oldSpeed = speed;
            speed = 0;
            isTrigger = false;
            isAttack = false;
        }
    }

    private void StartAttack()
    {
        var playerPosition = player.transform.position;
        if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerAttack))
        {
            if (speed != 0)
                oldSpeed = speed;
            speed = 0;
            animator.Play("ReaperAttack");
            StartCoroutine(WaitAttack());
        }
        else
        {
            if (speed == 0)
                speed = oldSpeed;
            animator.Play("ReaperRunning");
            transform.Translate(direction.normalized * speed / 8);
        }
    }
    
    private void IsAttack()
    {
        isAttack = true;
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
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX, 5);
    }

    private void Death() {
        gameObject.SetActive(false);
    }
    
    private IEnumerator WaitAttack()
    {
        isAttackComplete = true;
        yield return new WaitForSeconds(0.6f);
        isAttackComplete = false;
    }
}
