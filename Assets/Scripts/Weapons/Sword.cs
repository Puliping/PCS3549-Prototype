using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{
    [SerializeField]
    private GameObject parentRef;

    public override void Attack(Vector2 aimDirection, float playerComboTimerMulti = 1f, float playerDamageMulti = 1f)
    {
        if(!onCooldown)
        {
            base.Attack(aimDirection, playerComboTimerMulti, playerDamageMulti);
            StartCoroutine(AnimationCoroutine(aimDirection));
        }
    }

    protected IEnumerator AnimationCoroutine(Vector2 aimDirection)
    {
        //TODO matar essa corrotina se chamar de novo, e fazr Hitbox active false
        //parentRef.transform.position.Set(0, 0, 0);
        //this.transform.rotation.eulerAngles.Set(0, 0, 90);
        this.transform.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg) - 90);
        hitboxSprite.gameObject.SetActive(true);
        //toca animacion
        if(combo == 1)
            animator.Play("Sword1Animation");
        else if (combo == 2)
            animator.Play("Sword1Animation");
        else if (combo == 3)
            animator.Play("Sword1Animation");
        else if (combo == 4)
            animator.Play("Sword1Animation");
        yield return new WaitForSeconds(baseAttackDuration);
        hitboxSprite.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        onCooldown = false;
        playerOwner = LevelManager.Instance.localManager.playerRef;
        Debug.Log("Sword");
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("AttackSpeedMult", 1/(baseAttackDuration * 2f));
    }
}
