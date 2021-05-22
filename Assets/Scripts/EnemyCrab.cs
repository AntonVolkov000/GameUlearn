using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyCrab : MonoBehaviour
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
    private bool allowAttack;
    private SpriteRenderer sprite;
    private Animator animator;
    
    public void GetDamage()
    {
        if (countHealth == 0) return;
        isGetDamage = true;
        countHealth--;
        animator.Play("CrabDamage");
        StartCoroutine(WaitDamage());
        isAttack = false;
    }

    private void Start()
    {
        oldSpeed = speed;
        speed = 0;
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        allowAttack = true;
        animator = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        if (countHealth == 0)
        {
            speed = 0;
            animator.Play("CrabDeath");
            return;
        }
        if (isGetDamage || isAttack) return;
        var playerPosition = player.transform.position;
        if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerMove))
        {
            if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerAttack) && allowAttack)
                StartAttack();
            else
            {
                if (speed == 0)
                    speed = oldSpeed;
                if (playerPosition.x < transform.position.x)
                    speed = Math.Abs(speed) * -1;
                else
                    speed = Math.Abs(speed);
                animator.Play("CrabRun");
            }
        }
        else
        {
            speed = 0;
            animator.Play("CrabIdle");
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
        allowAttack = false;
        speed = 0;
        animator.Play("CrabAttack");
        StartCoroutine(WaitAttack());
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
        if (!PlayerInsideRadius(player.transform.position, transform.position,
            new Vector2(radiusTriggerAttack.x + addRangeAttack, radiusTriggerAttack.y))) return;
        player.GetDamage = true;
        player.TakeDamage(countDamage, new Vector2(0, 6));
    }

    private void Death() {
        gameObject.SetActive(false);
    }
    
    private IEnumerator WaitAttack() {
        yield return new WaitForSeconds(1f);
        allowAttack = true;
    }
    
    private IEnumerator WaitDamage() {
        yield return new WaitForSeconds(0.5f);
        isGetDamage = false;
    }
}
