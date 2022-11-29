using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArrow : MonoBehaviour
{
    public float damage;
    public int pierceCount = 1;
    //public List<Words> etc...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHitBox"))
        {
            // deal damage
            collision.gameObject.GetComponentInParent<Player>().ReceiveDamage(1f);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Destroy(this.gameObject);
        }
    }
}
