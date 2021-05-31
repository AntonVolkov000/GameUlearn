using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemySkeleton : MonoBehaviour
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
    private bool isGetDamage;
    private bool isAttack;
    private SpriteRenderer sprite;
    private Animator animator;
    
    public void GetDamage()
    {
        if (countHealth == 0) return;
        isGetDamage = true;
        countHealth--;
        animator.Play("SkeletonHit");
        StartCoroutine(WaitDamage());
        isAttack = false;
    }

    private void Start()
    {
        oldSpeed = speed;
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        animator = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        if (countHealth == 0)
        {
            speed = 0;
            animator.Play("SkeletonDead");
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            return;
        }
        if (isGetDamage || isAttack) return;
        var playerPosition = player.transform.position;
        if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerMove))
        {
            var positionCurrentObj = transform.position;
            if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerAttack) && 
                (sprite.flipX && playerPosition.x < positionCurrentObj.x ||
                !sprite.flipX && playerPosition.x > positionCurrentObj.x ))
                StartAttack();
            else
            {
                if (speed == 0)
                    speed = oldSpeed;
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
                animator.Play("SkeletonWalk");
            }
        }
        else
        {
            speed = 0;
            animator.Play("SkeletonIdle");
        }
        transform.Translate(direction.normalized * speed / 8);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("PlayerSpell"))
            GetDamage();
    }
    
    private void StartAttack()
    {
        isAttack = true;
        speed = 0;
        animator.Play("SkeletonAttack");
    }
    
    private void EndAttack()
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
        player.GetDamage = true;
        player.TakeDamage(countDamage, new Vector2(0, 6));
    }

    private void Death() {
        gameObject.SetActive(false);
    }

    private IEnumerator WaitDamage() {
        yield return new WaitForSeconds(0.2f);
        isGetDamage = false;
    }
}
