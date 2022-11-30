using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    [SerializeField]
    protected GameObject arrowPrefab;
    private Arrow projectileScript;
    private int pierceCount = 1;

    public override void Attack(Vector2 aimDirection, float playerComboTimerMulti = 1f, float playerDamageMulti = 1f)
    {
        base.Attack(aimDirection, playerComboTimerMulti, playerDamageMulti);
        GameObject projectile = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        projectileScript = projectile.GetComponentInChildren<Arrow>();
        loadArrowDamageAndEffects(projectileScript, playerDamageMulti);
        projectile.GetComponent<Rigidbody2D>().velocity = aimDirection * 10;
        projectile.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);

    }

    public void loadArrowDamageAndEffects(Arrow projectileScript, float playerDamageMulti = 1f)
    {
        //sets damage and word effects on arrow prefab pre spawn
        damage = baseDamage * playerDamageMulti;
        pierceCount = 1;
        projectileScript.parent = this;
        projectileScript.damage = damage;
        projectileScript.pierceCount = pierceCount;
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
