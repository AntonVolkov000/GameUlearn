using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    public static bool inDialogue;
    public Vector2 maxDistanceToNeutral;
    public TextMeshProUGUI shardText;
    public PlayerMagicSpell magicSpell;
    public Transform spellDirection;
    public static bool isAttackMagic;
    public static bool inLadder;
    public float startTimeAttack;

    private float moveInput;
    private float tempJumpForce;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJump;
    private Quaternion rotationSpell;
    private SpriteRenderer sprite;
    private Animator animator;

    public int CountShards { get; set; }
    public bool GetDamage { get; set; }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        animator.Play("PlayerHit");
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inDialogue = false;
        tempJumpForce = jumpForce;
        rb.gravityScale = gravityScale;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
    }

    private void FixedUpdate()
    {
        if (isAttackMagic) return;
        if (inDialogue)
        {
            animator.Play("PlayerIdle");
            return;
        }
        if (isAttackMagic) return;
        var horizontal = Input.GetAxis("Horizontal");
        if (isGrounded && Input.GetKey(KeyCode.Space) && !inLadder)
        {
            rb.velocity = Vector2.up * jumpForce;
            animator.Play("PlayerJump");
            isJump = true;
        }
        else if (isGrounded)
        {
            isJump = false;
        }
        jumpForce = inDialogue ? 0 : tempJumpForce;
        if (!GetDamage && !inDialogue)
        {
            if (horizontal < 0)
            {
                sprite.flipX = true;
                SetSpellDirection(-1.5f);
                moveInput = horizontal;
                if (!isJump && !inLadder)
                    animator.Play("PlayerRun");
            }
            else if (horizontal > 0)
            {
                sprite.flipX = false;
                SetSpellDirection(1.5f);
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
        shardText.text = CountShards.ToString();
        var difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - spellDirection.position;
        var rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        rotationSpell = Quaternion.Euler(0f, 0f, rotateZ + offset);
        if (!isAttackMagic && Input.GetMouseButton(0))
        {
            isAttackMagic = true;
            animator.Play("PlayerAttack2");
        }
    }

    private void PushSpell()
    {
        isAttackMagic = false;
        Instantiate(magicSpell, spellDirection.position, rotationSpell);
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

    /*public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            currentHealth = data.health;

            shardText = data.shardText;

            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            transform.position = position;
        }
    }
    */
}
