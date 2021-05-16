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
    public MagicSpell magicSpell;
    public Transform spellDirection;
    public float startTimeAttack;

    private float moveInput;
    private float tempJumpForce;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Quaternion rotationSpell;
    private float timeSpell;

    public int CountShards { get; set; }
    public bool GetDamage { get; set; }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody2D>();
        inDialogue = false;
        tempJumpForce = jumpForce;
        rb.gravityScale = gravityScale;
    }

    private void FixedUpdate()
    {
        if (!GetDamage)
        {
            moveInput = inDialogue ? 0 : Input.GetAxis("Horizontal");
            jumpForce = inDialogue ? 0 : tempJumpForce;
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
        shardText.text = CountShards.ToString();
    }
    
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            rb.velocity = Vector2.up * jumpForce;

        if (Input.GetKeyDown(KeyCode.A))
            SetSpellDirection(-1);
        if (Input.GetKeyDown(KeyCode.D))
            SetSpellDirection(1);
        
        var difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        var rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        rotationSpell = Quaternion.Euler(0f, 0f, rotateZ + offset);
        if (timeSpell <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                // Instantiate(magicSpell, spellDirection.position, rotationSpell);
                timeSpell = startTimeAttack;
            }
        }
        else
        {
            timeSpell -= Time.deltaTime;
        }
    }
    
    private void SetSpellDirection(float add)
    {
        spellDirection.position = new Vector2(transform.position.x + add, transform.position.y);
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
