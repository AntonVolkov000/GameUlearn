using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NeutralHeroes : DialogueTrigger, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        TriggerDialogue();
    }
    
    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
    }
}
