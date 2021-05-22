using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicSpell : MonoBehaviour
{
    public float speed;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Ladder")) return;
        DestroySpell();
    }
    
    void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed / 8);
    }
    
    private void DestroySpell()
    {
        Destroy(gameObject);
    }
}
