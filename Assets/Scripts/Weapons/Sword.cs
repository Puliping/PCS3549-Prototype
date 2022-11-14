using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{

    bool cooldown = false;
    Quaternion hitboxRotation = new();
    Vector2 playerPos2D = new();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack(Vector2 aimDirection, float playerComboTimerMulti = 1f, float playerDamageMulti = 1f)
    {
        hitboxRotation.SetLookRotation(aimDirection, playerOwner.transform.up);
        StartCoroutine(AnimationCoroutine(hitboxRotation));
        base.MeleeAttack(aimDirection, playerComboTimerMulti, playerDamageMulti);
    }

    IEnumerator AnimationCoroutine(Quaternion hitboxRotation)
    {
        activeCollider = hitboxCollider[combo];
        //toca animação
        yield return new WaitForSeconds(baseAttackDuration);
        activeCollider = null;
    }
}
