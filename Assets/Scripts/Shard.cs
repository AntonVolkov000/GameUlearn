using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    public bool taken;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !taken)
        {
            other.gameObject.GetComponent<PlayerController>().CountShards++;
            this.gameObject.SetActive(false);
            taken = true;
        }
    }
}
