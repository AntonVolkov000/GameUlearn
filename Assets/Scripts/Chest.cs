using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chest : MonoBehaviour, IPointerClickHandler
{
    public GameObject thing;
    public Vector2 maxDistancePlayer;
    public PlayerController player;
    public Animator animator;
    
    private IEnumerator blinkCoroutine;
    private bool isShard;
    private bool isPotion; 

    private void Start()
    {
        isShard = thing.name == "shard";
        isPotion = thing.name == "potion";
        thing.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isShard)
            if (thing.gameObject.GetComponent<Shard>().taken)
                Close();
        if (isPotion) 
            if (thing.gameObject.GetComponent<Potion>().taken)
                Close();
    }

    private void Close()
    {
        StartCoroutine(Blink(5f));
        animator.SetBool("isClose", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!thing.activeSelf &&
            Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) <
            maxDistancePlayer.x &&
            Math.Abs(player.transform.position.y - this.gameObject.transform.position.y) <
            maxDistancePlayer.y)
        {
            animator.SetBool("isClose", true);
            if (isShard)
                if (!thing.gameObject.GetComponent<Shard>().taken)
                    thing.SetActive(true);
            if (isPotion) 
                if (!thing.gameObject.GetComponent<Potion>().taken)
                    thing.SetActive(true);
        }
    }
    
    private IEnumerator Blink(float waitTime) {
        yield return new WaitForSeconds(waitTime);
    }
}
