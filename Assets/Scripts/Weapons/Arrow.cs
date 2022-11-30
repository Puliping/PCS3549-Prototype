using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Weapon parent;
    public float damage;
    public int pierceCount = 1;
    //public List<Words> etc...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject enemy = collision.gameObject;
        if(enemy.layer == LayerMask.NameToLayer("Enemy"))
        {
            // deal damage
            Enemy enemyComponent = enemy.GetComponentInChildren<Enemy>();
            parent.HitEnemy(enemyComponent, damage);

            Debug.Log("hit " + enemy.name);
            //reduce pierce count and destroy if zero
            pierceCount--;
            if(pierceCount == 0)
                Destroy(this.gameObject);
        } else if (enemy.layer == LayerMask.NameToLayer("Walls")){
            Destroy(this.gameObject);
        }
    }
}
