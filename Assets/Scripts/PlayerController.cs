using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public static bool inDialogue;
    public Vector2 maxDistanceToNeutral;
    public TextMeshProUGUI shardText;

    private float moveInput;
    private float tempJumpForce;
    private Rigidbody2D rb;
    private bool isGrounded;
    
    public int CountShards { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inDialogue = false;
        tempJumpForce = jumpForce;
    }

    private void FixedUpdate()
    {
        moveInput = inDialogue ? 0 : Input.GetAxis("Horizontal");
        jumpForce = inDialogue ? 0 : tempJumpForce;
        
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        shardText.text = CountShards.ToString();
    }
    
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            rb.velocity = Vector2.up * jumpForce;
    }
}
