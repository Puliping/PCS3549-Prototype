using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{

    bool cooldown = false;
    Quaternion hitboxRotation = new();
    Vector2 playerPos2D = new();
    // Start is called before the first frame update
    protected override void Start()
    {
        playerOwner = LevelManager.Instance.localManager.playerRef;
        Debug.Log("Sword");
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void Attack(Vector2 aimDirection, float playerComboTimerMulti = 1f, float playerDamageMulti = 1f)
    {
        hitboxRotation.SetLookRotation(aimDirection, playerOwner.transform.up);
        StartCoroutine(AnimationCoroutine(hitboxRotation));
        base.Attack(aimDirection, playerComboTimerMulti, playerDamageMulti);
    }

    IEnumerator AnimationCoroutine(Quaternion hitboxRotation)
    {
        activeCollider = hitboxCollider[combo];
        spriteRef.SetActive(true);
        activeCollider.gameObject.SetActive(true);
        //toca animacion
        if(combo == 1)
            animator.Play("Sword1Animation",0,baseAttackDuration);
        else if (combo == 2)
            animator.Play("Sword1Animation");
        else if (combo == 3)
            animator.Play("Sword1Animation");
        else if (combo == 4)
            animator.Play("Sword1Animation");
        yield return new WaitForSeconds(baseAttackDuration);
        spriteRef.SetActive(false);
        activeCollider.gameObject.SetActive(false);
        activeCollider = null;
    }
}
