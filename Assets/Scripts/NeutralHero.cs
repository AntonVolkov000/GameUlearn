using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralHero : MonoBehaviour
{
    private SpriteRenderer sprite;
    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
}
