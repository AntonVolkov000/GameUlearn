using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;
    private float tempJumpForce;
    
    public static bool inDialogue;
    private Rigidbody2D rb;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    
    
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
    }
    
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            rb.velocity = Vector2.up * jumpForce;
    }
}
