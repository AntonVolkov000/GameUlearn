using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chest : MonoBehaviour, IPointerClickHandler
{
    public GameObject shard;
    public Vector2 maxDistancePlayer;
    public PlayerController player;
    public Animator animator;
    
    private IEnumerator blinkCoroutine;

    private void Start()
    {
        shard.SetActive(false);
    }
    
    private void FixedUpdate()
    {
        if (shard.gameObject.GetComponent<Shard>().taken)
        {
            blinkCoroutine = Blink(5f);
            StartCoroutine(blinkCoroutine);
            animator.SetBool("isClose", false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!shard.activeSelf &&
            Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) <
            maxDistancePlayer.x &&
            Math.Abs(player.transform.position.y - this.gameObject.transform.position.y) <
            maxDistancePlayer.y)
        {
            animator.SetBool("isClose", true);
            if (!shard.gameObject.GetComponent<Shard>().taken)
                shard.SetActive(true);
        }
    }
    
    private IEnumerator Blink(float waitTime) {
        yield return new WaitForSeconds(waitTime);
    }
}
