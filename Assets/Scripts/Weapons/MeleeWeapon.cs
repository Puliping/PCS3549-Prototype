using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    protected int combo = 0;
    [SerializeField]
    protected float[] comboMult;
    protected Coroutine comboCoroutine;
    [SerializeField]
    protected float baseComboTimer = 2;

    [SerializeField]
    protected WeaponHitbox Hitbox;

    [SerializeField]
    protected Animator animator;

    protected IEnumerator ComboTimerCoroutine(float comboTimer)
    {
        yield return new WaitForSeconds(comboTimer);
        combo = 0;
    }

    public override void Attack(Vector2 aimDirection, float playerComboTimerMulti = 1f, float playerDamageMulti = 1f)
    {
        Hitbox.clearEnemyList();
        if (!onCooldown)
        {
            if (combo == 3)
                combo = 0;
            combo++;
            if (comboCoroutine != null)
                StopCoroutine(comboCoroutine);
            comboCoroutine = StartCoroutine(ComboTimerCoroutine(baseComboTimer * playerComboTimerMulti));
            base.Attack(aimDirection, playerComboTimerMulti, playerDamageMulti);

            //TODO: nao deixar atualizar entre comecar o golpe e acertar
            // ideias: cd de ataque sempre maior que animacao total -> usar cd variavel para efeitos maiores como skills da arcana
            // passar o dano para a hitbox?
            damage = baseDamage * comboMult[combo] * playerDamageMulti;


            onCooldown = true;
            StartCoroutine(AttackCooldown());
 
            //playerOwner.PlayAnimation(type, combo);
        }
    }

    public void MeleeAttackResolve(Enemy enemy)
    {
        HitEnemy(enemy, damage);
        //// na1 estah acertando as slaime
        //Physics2D.OverlapCollider(activeCollider, enemyFilter, hits);
        //foreach (Collider2D target in hits)
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(playerOwner.transform.position, target.transform.position, LayerMask.NameToLayer("Walls"));
        //    if (!hit.collider)
        //    {
        //        Enemy enemyComponent = target.GetComponentInChildren<Enemy>();
        //        enemyComponent.TakeDamage(damage);
        //        // stun e knockback
        //        Debug.Log("hit " + target.name + " for " + damage + " damage");
        //    }
        //}
        //System.Array.Clear(hits, 0, 100);
        

        //dar dano 
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
