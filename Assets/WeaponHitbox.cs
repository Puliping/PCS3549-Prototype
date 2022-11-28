using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    [SerializeField]
    private MeleeWeapon weapon;
    private List<Enemy> enemyList = new List<Enemy>();
    private Enemy currentEnemy;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentEnemy = collision.gameObject.GetComponent<Enemy>();
        if (currentEnemy == null)
        {
            Debug.Log("errou");
            return;
        }
        if(!enemyList.Contains(currentEnemy))
        {
            enemyList.Add(currentEnemy);
            RaycastHit2D hit = Physics2D.Raycast(weapon.transform.position, currentEnemy.transform.position, LayerMask.NameToLayer("Walls"));
            //if (!hit.collider)
            {
                weapon.MeleeAttackResolve(currentEnemy);
            }
        }
    }

    public void clearEnemyList()
    {
        enemyList.Clear();
    }
}
