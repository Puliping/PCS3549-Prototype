using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject enemy = collision.gameObject;
        if(enemy.layer == LayerMask.NameToLayer("Enemy"))
        {
            // deal damage
            Enemy enemyComponent = enemy.GetComponentInChildren<Enemy>();
            enemyComponent.TakeDamage(1f); // FF

            Debug.Log("hit " + enemy.name);
        } else if (enemy.layer == LayerMask.NameToLayer("Walls")){
            Destroy(this.gameObject);
        }
    }
}
