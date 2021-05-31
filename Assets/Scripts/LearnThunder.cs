using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnThunder : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public Animator animator;
    public PlayerController player;
    
    private AudioClip audioClipOld;
    private bool thunder;

    private void Start()
    {
        audioClipOld = audioSource.clip;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !thunder)
        {
            player.isHit = true;
            animator.Play("Fade");
            StartCoroutine(EndGrom());
            audioSource.clip = audioClip;
            audioSource.volume = 1;
            audioSource.Play();
            thunder = true;
            StartCoroutine(WaitGrom());
        }
    }
    
    private IEnumerator EndGrom() {
        yield return new WaitForSeconds(0.5f);
        player.isHit = false;
    }

    private IEnumerator WaitGrom() {
        yield return new WaitForSeconds(5f);
        audioSource.clip = audioClipOld;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
}
