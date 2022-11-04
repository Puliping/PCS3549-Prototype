using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public int level;
    public Player playerOwner;
    public float chargeTime { get; private set;} = .5f;
    public enum WeaponType
    {
        Sword,
        Staff,
        Book,       
        Bow
    }
    public WeaponType type;
    public enum WordsType
    {
        Burnning,
        Poison,
        Projectiles
    }
    public List<WordsType> wordsList;
    public void SetWeapon(int Level, List<WordsType> wordsList)
    {
    }
    public void WordsEffectPermanent(WordsType word)
    {

    }
    public void WordsEffectOnHit(WordsType word)
    {

    }
    public void Attack()
    {

    }
    public void ChargedAttack()
    {

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
    public void SendProjectile()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
