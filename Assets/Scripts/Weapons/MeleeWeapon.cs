using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public int combo = 0;
    public float[] comboMult;
    Coroutine comboCoroutine;
    float baseComboTimer = 2;

    public GameObject hitbox1;
    public GameObject hitbox2;
    public GameObject hitbox3;
    public GameObject hitboxCharged;
    public Collider2D[] hitboxCollider;
    public Collider2D activeCollider;
    Collider2D[] hits = new Collider2D[100];
    ContactFilter2D enemyFilter = new ContactFilter2D();

    IEnumerator ComboTimerCoroutine(float comboTimer)
    {
        yield return new WaitForSeconds(comboTimer);
        combo = 0;
    }

    public virtual void MeleeAttack(Vector2 aimDirection, float playerComboTimerMulti = 1f, float playerDamageMulti = 1f)
    {
        base.Attack();
        if (!onCooldown)
        {
            damage = baseDamage * comboMult[combo] * playerDamageMulti;
            onCooldown = true;
            StartCoroutine(AttackCooldown());
            //playerOwner.PlayAnimation(type, combo);
            StartCoroutine(DelayedHit(baseAttackDuration));
        }
    }

    public void MeleeAttackResolve()
    {
        Physics2D.OverlapCollider(activeCollider, enemyFilter, hits);
        foreach (Collider2D target in hits)
        {
            RaycastHit2D hit = Physics2D.Raycast(playerOwner.transform, target.transform, playerOwner.wallLayer);
            if (!hit.collider)
            {
                Enemy enemyComponent = target.GetComponentInChildren<Enemy>();
                enemyComponent.TakeDamage(damage);
                // stun e knockback

                Debug.Log("hit " + target.name + " for " + damage + " damage");
            }
        }
        System.Array.Clear(hits, 0, 100);
    }

    IEnumerator DelayedHit(float duration)
    {
        yield return new WaitForSeconds(duration / 3);
        MeleeAttackResolve();
    }

    // Start is called before the first frame update
    void Start()
    {
        hitboxCollider[1] = hitbox1.GetComponent<Collider2D>();
        hitboxCollider[2] = hitbox2.GetComponent<Collider2D>();
        hitboxCollider[3] = hitbox3.GetComponent<Collider2D>();
        hitboxCollider[4] = hitboxCharged.GetComponent<Collider2D>();
        enemyFilter.SetLayerMask(playerOwner.enemyLayers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
