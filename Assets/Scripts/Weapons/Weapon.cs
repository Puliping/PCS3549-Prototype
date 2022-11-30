using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected float damage;
    protected int level;
    [SerializeField]
    protected float attackCooldown;
    [SerializeField]
    protected float baseAttackDuration;
    public Player playerOwner;
    protected bool onCooldown = false;
    [SerializeField]
    protected float baseDamage = 10;
    [SerializeField]
    protected GameObject hitboxSprite;
    //ranged 
    [SerializeField]
    protected float baseProjectileSpeed = 10;

    protected ContactFilter2D enemyFilter = new ContactFilter2D();

    public float chargeTime { get; protected set;} = .5f;

    protected enum WeaponType
    {
        Sword,
        Staff,
        Book,       
        Bow
    }
    protected WeaponType type;
    protected enum WordType
    {
        Poison,
        Root,
        Doom,
        Electrify,
        Slow,
        Petrify
    }
    protected List<WordType> wordList;
    protected float[] wordMagnitudePrimary; //magnitude da aplicao ex 5s de slow, slow sendo uma keyword fixa ou seja sempre slowa em 30%
    protected float[] wordMagnitudeSecondary; //chance da aplicao  ex 15% chance on hit
    protected void SetWeapon(int Level, List<WordType> wordList, float[] wordMagnitudePrimary, float[] wordMagnitudeSecondary, float baseDamage, WeaponType type)
    {
        //equipment change
    }
    protected void WordsEffectPermanent(WordType word)
    {
        //called on start and on equipment change
    }
    protected void WordsEffectOnHit(List<WordType> wordList)
    {
        //Setar esses efeitos para aplicar no hit enemy
        int i = 0;
        foreach(WordType word in wordList)
        {
            if(word.Equals(WordType.Poison))
            {

            }
            if (word.Equals(WordType.Root))
            {

            }
            if (word.Equals(WordType.Doom))
            {

            }
            if (word.Equals(WordType.Electrify))
            {

            }
            if (word.Equals(WordType.Slow))
            {

            }
            if (word.Equals(WordType.Petrify))
            {

            }
            i++;
        }
    }

    public virtual void Attack(Vector2 aimDirection, float playerComboTimerMulti = 1f, float playerDamageMulti = 1f)
    {
        //WordsEffectOnHit(wordList);
    }

    public virtual void ChargedAttack(Vector2 aimDirection, float playerComboTimerMulti = 1f, float playerDamageMulti = 1f)
    {
        // TODO charged attack
    }

    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        onCooldown = false;
    }

    public void HitEnemy(Enemy enemy, float damage)
    {
        playerOwner.HitEnemy();
        enemy.TakeDamage(damage);
        Debug.Log("Hit enemy " + enemy + " for " + damage + " damage");
    }
    protected void HitWall()
    {

    }
    protected void StopAttack()
    {

    }
    protected void SendProjectile(Vector2 aimDirection)
    {
        //fazourro
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Debug.Log("Set player ref in Weapon");
        playerOwner = LevelManager.Instance.localManager.playerRef;
        enemyFilter.SetLayerMask(LayerMask.NameToLayer("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
