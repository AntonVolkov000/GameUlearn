using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWizard : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public PlayerController player;
    public Vector2 radiusTriggerAttackHand;
    public Vector2 radiusTriggerAttackMagic;
    public float addRangeAttackHand;
    public MagicSpell magicSpell;
    public Transform spellDirection;
    public float offset;
    public float countHealth;
    public int countDamage;
    
    private bool isGetDamage;
    private bool checkOneCell;
    private bool isBorder;
    private bool waitAttack;
    private Vector3 position;
    private SpriteRenderer sprite;
    private Animator animator;
    private Quaternion rotationSpell;

    private void Start()
    {
        magicSpell.player = player;
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        animator = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        if (countHealth == 0 || isGetDamage || waitAttack) return;
        var playerPosition = player.transform.position;
        if (checkOneCell)
        {
            if (Math.Abs(position.x - transform.position.x) >= 1 || isBorder)
            {
                speed *= -1;
                sprite.flipX = !sprite.flipX;
                checkOneCell = false;
                isBorder = false;
            }
            else
            {
                animator.Play("MageRunning");
                transform.Translate(direction.normalized * speed / 8);
            }
        }
        else
        {
            if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerAttackMagic))
            {
                sprite.flipX = playerPosition.x < transform.position.x;
                if (playerPosition.x < transform.position.x)
                    SetSpellDirection(-1.3f);
                else
                    SetSpellDirection(1.3f);
                StartAttack();
            }
            else
                animator.Play("MageIdle");
        }
    }
    
    private void SetSpellDirection(float add)
    {
        spellDirection.position = new Vector2(transform.position.x + add, transform.position.y + 0.7f);
    }

    private void StartAttack()
    {
        var playerPosition = player.transform.position;
        if (PlayerInsideRadius(playerPosition, transform.position, radiusTriggerAttackHand))
        {
            waitAttack = true;
            animator.Play("MageAttackHand");
        }
        else
            AttackMagic();
    }
    
    private void AttackHand()
    {
        position = transform.position;
        if (player.transform.position.x < transform.position.x)
        {
            speed = Math.Abs(speed);
            sprite.flipX = false;
        }
        else
        {
            speed = Math.Abs(speed) * -1;
            sprite.flipX = true;
        }
        checkOneCell = true;
        StartCoroutine(WaitBorder());
        waitAttack = false;
    }

    private void AttackMagic()
    {
        if (player.transform.position.y <= transform.position.y + 1)
            animator.Play("MageAttackMagicHorizontal");
        else
            animator.Play("MageAttackMagicHigh");
    }
    
    private void PushMagicSpell()
    {
        var playerPosition = player.transform.position;
        var difference = playerPosition - spellDirection.position;
        var rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        rotationSpell = Quaternion.Euler(0f, 0f, rotateZ + offset);
        Instantiate(magicSpell, spellDirection.position, rotationSpell);
    }
    
    private void GetDamage()
    {
        isGetDamage = true;
        animator.Play("MageDamage");
        StartCoroutine(WaitDamage());
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
            new Vector2(radiusTriggerAttackHand.x + addRangeAttackHand, radiusTriggerAttackHand.y))) return;
        var moveX = -5;
        player.GetDamage = true;
        if (!sprite.flipX)
            moveX = Math.Abs(moveX);
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX, 5);
        player.TakeDamage(countDamage);
    }

    private IEnumerator WaitDamage() {
        yield return new WaitForSeconds(0.6f);
        isGetDamage = false;
    }
    
    private IEnumerator WaitBorder() {
        yield return new WaitForSeconds(1);
        if (Math.Abs(position.x - transform.position.x) < 1)
            isBorder = true;
    }

    private void Death() {
        gameObject.SetActive(false);
    }
}
