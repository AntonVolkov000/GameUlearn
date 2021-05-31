using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NeutralHero : MonoBehaviour
{
    public PlayerController player;

    private void OnMouseEnter()
    {
        player.isNeutralObject = true;
    }

    private void OnMouseExit()
    {
        player.isNeutralObject = false;
    }
}
