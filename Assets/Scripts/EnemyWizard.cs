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
    public MageMagicSpell magicSpell;
    public Transform spellDirection;
    public float offset;
    public float countHealth;
    public int countDamageHand;
    public int countDamageMagic;
    
    private bool isGetDamage;
    private bool checkOneCell;
    private bool isBorder;
    private bool waitAttack;
    private Vector3 position;
    private SpriteRenderer sprite;
    private Animator animator;
    private Quaternion rotationSpell;
    
    public void GetDamage()
    {
        if (countHealth == 0) return;
        isGetDamage = true;
        countHealth--;
        animator.Play("MageDamage");
        StartCoroutine(WaitDamage());
        waitAttack = false;
    }

    private void Start()
    {
        magicSpell.countDamage = countDamageMagic;
        magicSpell.player = player;
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        animator = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        if (countHealth == 0)
            animator.Play("MageDeath");
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

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("PlayerSpell"))
            GetDamage();
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
        var difference = new Vector3(playerPosition.x, playerPosition.y + 1.2f, playerPosition.z) - spellDirection.position;
        var rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        rotationSpell = Quaternion.Euler(0f, 0f, rotateZ + offset);
        Instantiate(magicSpell, spellDirection.position, rotationSpell);
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
        player.TakeDamage(countDamageHand, new Vector2(moveX, 5));
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
