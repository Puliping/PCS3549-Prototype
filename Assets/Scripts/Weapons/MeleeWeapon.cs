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
    public Collider2D[] hitboxCollider = new Collider2D[5];
    public Collider2D activeCollider;
    Collider2D[] hits = new Collider2D[100];

    public Animator animator;

    IEnumerator ComboTimerCoroutine(float comboTimer)
    {
        yield return new WaitForSeconds(comboTimer);
        combo = 0;
    }

    public override void Attack(Vector2 aimDirection, float playerComboTimerMulti = 1f, float playerDamageMulti = 1f)
    {
        if (combo == 3)
            combo = 0;
        combo++;
        if(comboCoroutine != null)
            StopCoroutine(comboCoroutine);
        comboCoroutine = StartCoroutine(ComboTimerCoroutine(baseComboTimer*playerComboTimerMulti));
        base.Attack(aimDirection, playerComboTimerMulti, playerDamageMulti);
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
            RaycastHit2D hit = Physics2D.Raycast(playerOwner.transform.position, target.transform.position, LayerMask.NameToLayer("Walls")); //Codigo mudado aq
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
    protected override void Start()
    {
        Debug.Log("MeleeWeapon");
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
