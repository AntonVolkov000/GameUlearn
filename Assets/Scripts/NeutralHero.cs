using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NeutralHero : MonoBehaviour, IPointerClickHandler
{
    public PlayerController player;
    public void OnPointerClick(PointerEventData eventData)
    {
        player.isNeutralObject = true;
    }
}
