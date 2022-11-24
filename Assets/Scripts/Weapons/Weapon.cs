using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public int level;
    public float attackCooldown;
    public float baseAttackDuration;
    public Player playerOwner;
    public bool onCooldown;
    public float baseDamage = 10;
    public GameObject spriteRef;
    //ranged 
    float baseProjectileSpeed = 10;

    protected ContactFilter2D enemyFilter = new ContactFilter2D();

    public float chargeTime { get; private set;} = .5f;
    public enum WeaponType
    {
        Sword,
        Staff,
        Book,       
        Bow
    }
    public WeaponType type;
    public enum WordType
    {
        Poison,
        Root,
        Doom,
        Electrify,
        Slow,
        Petrify
    }
    public List<WordType> wordList;
    public float[] wordMagnitudePrimary; //magnitude da aplica��o ex 5s de slow, slow sendo uma keyword fixa ou seja sempre slowa em 30%
    public float[] wordMagnitudeSecondary; //chance da aplica��o  ex 15% chance on hit
    public void SetWeapon(int Level, List<WordType> wordList, float[] wordMagnitudePrimary, float[] wordMagnitudeSecondary, float baseDamage, WeaponType type)
    {

    }
    public void WordsEffectPermanent(WordType word)
    {

    }
    public void WordsEffectOnHit(List<WordType> wordList)
    {
        //dar um jeito de passar pro inimigo esses efeitos
        int i = 0;
        foreach(WordType word in wordList)
        {
            if(word.Equals(WordType.Poison))
            {
                //

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

    virtual public void Attack(Vector2 aimDirection, float playerComboTimerMulti = 1f, float playerDamageMulti = 1f)
    {
        WordsEffectOnHit(wordList);
    }



    public IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        onCooldown = false;
    }

    public void HitEnemy(Enemy enemy)
    {

    }
    public void HitWall()
    {

    }
    public void StopAttack()
    {

    }
    public void SendProjectile(Vector2 aimDirection)
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
        //playerOwner = LevelManager.Instance.localManager.playerRef;
        enemyFilter.SetLayerMask(LayerMask.NameToLayer("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
