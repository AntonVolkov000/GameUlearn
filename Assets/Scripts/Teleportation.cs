using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Teleportation : Loader, IPointerClickHandler
{
    // public Animator animator;
    public int sceneIndex;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        LoadLevel(sceneIndex);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRadus = true;
            animator.Play("ObeliskWork");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRadus = false;
            animator.Play("ObeliskStand");
        }
    }
}
