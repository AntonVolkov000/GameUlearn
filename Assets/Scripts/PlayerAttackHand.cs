using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHand : MonoBehaviour
{
    private Collider2D other;
    private bool inRadius;

    public void StartDamage()
    {
        if (other == null) return;
        if (!inRadius) return;
        if (other.GetComponent<EnemySkeleton>() != null)
            other.GetComponent<EnemySkeleton>().GetDamage();
        else if (other.GetComponent<EnemyCrab>() != null)
            other.GetComponent<EnemyCrab>().GetDamage();
        else if (other.GetComponent<EnemyReaper>() != null)
            other.GetComponent<EnemyReaper>().GetDamage();
        else if (other.GetComponent<EnemySlime>() != null)
            other.GetComponent<EnemySlime>().GetDamage();
        else if (other.GetComponent<EnemyWitch>() != null)
            other.GetComponent<EnemyWitch>().GetDamage();
        else if (other.GetComponent<EnemyWizard>() != null)
            other.GetComponent<EnemyWizard>().GetDamage();
    }
        
    private void OnTriggerStay2D(Collider2D other)
    {
        inRadius = true;
        this.other = other;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        inRadius = false;
    }
}
