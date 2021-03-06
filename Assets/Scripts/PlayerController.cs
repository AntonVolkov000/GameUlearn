using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : HealthBar
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public float speed;
    public float gravityScale;
    public float jumpForce;
    public Transform feetPos;
    public float offset;
    public float checkRadius;
    public LayerMask whatIsGround;
    public LayerMask isEnemy;
    public static bool inDialogue;
    public Vector2 maxDistanceToNeutral;
    public TextMeshProUGUI shardText;
    public PlayerMagicSpell magicSpell;
    public Transform spellDirection;
    public Transform areaAttackHand;
    public PlayerAttackHand areaAttackHandStart;
    public static bool isAttackMagic;
    public bool isNeutralObject;
    public bool inLadder;
    public bool isGrounded;
    public bool isHit;
    public bool isJump;
    public float speedWriteText;
    public bool isLearnAttack;
    public bool isLearnDialogue;
    public bool isLearn;
    public bool isAttackHand;
    public MainMenu load;
    public bool isFall;

    private float moveInput;
    private float tempJumpForce;
    private Rigidbody2D rb;
    private bool isDead;
    private Quaternion rotationSpell;
    private SpriteRenderer sprite;
    private Animator animator;
    private Coroutine fallCoroutine;

    private Vector3 mousePos;
    //private VectorValue position;

    public int CountShards { get; set; }
    public bool GetDamage { get; set; }

    public bool MoveRight { get; set; }
    public bool MoveLeft { get; set; }
    public bool Jump { get; set; }
    public bool HandAttack { get; set; }
    public bool MagicAttack { get; set; }
    public bool DeadEnemy { get; set; }
    public bool Dialogue { get; set; }
    public bool OpenChest { get; set; }


    public void TakeDamage(int damage, Vector2 whereForce)
    {
        if (currentHealth != 0)
            rb.velocity = whereForce;
        if (damage > currentHealth)
            currentHealth = 0;
        else if (currentHealth > 0)
        {
            currentHealth -= damage;
            animator.Play("PlayerHit");
            isHit = true;
            isAttackMagic = false;
        }

        healthBar.SetHealth(currentHealth);
        isAttackMagic = false;
    }

    public void GetHealth(int addHealth)
    {
        if (currentHealth + addHealth <= maxHealth)
        {
            currentHealth += addHealth;
            healthBar.SetHealth(currentHealth);
        }
    }

    public void OffIsAttack()
    {
        isAttackMagic = false;
    }

    private void Start()
    {
        var generalVariable = GameObject.FindGameObjectWithTag("GeneralVar").GetComponent<GeneralVar>();
        currentHealth = generalVariable.countHeath;
        CountShards = generalVariable.countShards;
        // transform.position = position.initialValue;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inDialogue = false;
        tempJumpForce = jumpForce;
        rb.gravityScale = gravityScale;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround) ||
                     Physics2D.OverlapCircle(feetPos.position, 0, isEnemy);
    }

    private void FixedUpdate()
    {
        if (currentHealth == 0)
        {
            speed = 0;
            if (!isDead)
                animator.Play("PlayerDeath");
            return;
        }

        if (isAttackMagic || isHit) return;
        if (!isLearn && inDialogue)
        {
            animator.Play("PlayerIdle");
            return;
        }

        shardText.text = CountShards.ToString();

        if (isGrounded && fallCoroutine != null)
        {
            isFall = true;
            StopCoroutine(fallCoroutine);
        }

        if (isGrounded && Input.GetKey(KeyCode.Space) && !inLadder && !isAttackHand)
        {
            rb.velocity = Vector2.up * jumpForce;
            animator.Play("PlayerJump");
            isJump = true;
            fallCoroutine = StartCoroutine(WaitFall());
        }
        else if (isGrounded)
        {
            isJump = false;
        }

        jumpForce = inDialogue ? 0 : tempJumpForce;
        if (isLearn)
            jumpForce = tempJumpForce;
        PlayerMove();
        if (!isAttackMagic && !inLadder && !isLearnAttack || isLearnDialogue)
            PlayerAttack();
    }

    private void PlayerMove()
    {
        var horizontal = Input.GetAxis("Horizontal");
        if (GetDamage) return;
        if (!isLearn && inDialogue)
            return;
        if (horizontal < 0)
        {
            sprite.flipX = true;
            SetSpellDirection(-1.5f);
            SetAreaAttackPos(-1, 1);
            moveInput = horizontal;
            if (!isJump && !inLadder)
                animator.Play("PlayerRun");
        }
        else if (horizontal > 0)
        {
            sprite.flipX = false;
            SetSpellDirection(1.5f);
            SetAreaAttackPos(1, -1);
            moveInput = horizontal;
            if (!isJump && !inLadder)
                animator.Play("PlayerRun");
        }
        else
        {
            moveInput = 0;
            if (!isJump && !inLadder)
                animator.Play("PlayerIdle");
        }

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    private void PlayerAttack()
    {
        if (Input.GetMouseButton(0) && !isNeutralObject)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RotatePlayer();
            isAttackMagic = true;
            animator.Play("PlayerAttack2");
            StartCoroutine(WaitAttack(1f));
        }
        else if (Input.GetMouseButton(1) && !isNeutralObject && !isAttackHand)
        {
            RotatePlayer();
            isAttackMagic = true;
            isAttackHand = true;
            animator.Play("PlayerAttack1");
            StartCoroutine(WaitAttack1(0.9f));
        }
    }

    private void RotatePlayer()
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
        {
            sprite.flipX = true;
            SetSpellDirection(-1.5f);
            SetAreaAttackPos(-1, 1);
        }
        else
        {
            sprite.flipX = false;
            SetSpellDirection(1.5f);
            SetAreaAttackPos(1, -1);
        }
    }

    private void PushSpell()
    {
        if (!isNeutralObject)
        {
            var difference = mousePos - spellDirection.position;
            var rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rotationSpell = Quaternion.Euler(0f, 0f, rotateZ + offset);
            Instantiate(magicSpell, spellDirection.position, rotationSpell);
        }

        isAttackMagic = false;
        isNeutralObject = false;
    }

    private void SetAreaAttackPos(float dierection, float rotateArea)
    {
        areaAttackHand.position = new Vector2(transform.position.x + 0.7f * dierection, transform.position.y + 1.2f);
        areaAttackHand.rotation = Quaternion.Euler(0f, 0f, 40 * rotateArea);
    }

    private void SetSpellDirection(float add)
    {
        spellDirection.position = new Vector2(transform.position.x + add, transform.position.y + 1.7f);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Spell")) return;
        if (GetDamage)
            GetDamage = false;
    }

    private void AttackHand()
    {
        areaAttackHandStart.StartDamage();
    }

    private void EndAttack()
    {
        isAttackMagic = false;
    }

    private void EndHit()
    {
        isHit = false;
    }

    private void EndDeath()
    {
        isDead = true;
        animator.Play("PlayerDead");
        load.PlayGame(0);
    }

    private IEnumerator WaitAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isAttackMagic = false;
    }

    private IEnumerator WaitAttack1(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isAttackHand = false;
    }

    private IEnumerator WaitFall()
    {
        yield return new WaitForSeconds(1f);
        if (!isGrounded && !isHit && currentHealth != 0)
            animator.Play("PlayerFall");
        StartCoroutine(WaitFalling());
    }

    private IEnumerator WaitFalling()
    {
        yield return new WaitForSeconds(0.5f);
        if (isGrounded && isFall)
        {
            isJump = false;
            isFall = false;
        }
    }
}
