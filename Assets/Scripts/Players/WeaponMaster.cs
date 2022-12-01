using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMaster : Player
{
    public List<GameObject> weaponGameObjectList = new List<GameObject>();
    private List<Weapon> weaponList = new List<Weapon>();
    private int currentWeaponNumber = 0;
    private int currentCharges = 0;
    private int usedCharges = 0;
    private int currentChargesForNextWeapon = 0;
    private int maxWeakCharges = 3;
    private int maxMediumCharges = 2;
    private int maxStrongCharges = 1;
    private int maxTotalCharges => maxWeakCharges + maxMediumCharges + maxStrongCharges;

    private float weakChargeMultiplier = 1.1f;
    private float mediumChargeMultiplier = 1.3f;
    private float strongChargeMultiplier = 1.6f;

    protected override void Start()
    {
        LevelManager.Instance.mainCamera = mainCamera;
        fullhp = hp;
        slider.value = 1;
        for (int i = 0; i < weaponGameObjectList.Count; i++)
        {
            weaponGameObject = Instantiate(weaponGameObjectList[i], this.transform.position, Quaternion.identity, transform);
            weapon = weaponGameObject.GetComponentInChildren<Weapon>();
            weaponGameObjectList[i] = weaponGameObject;
            weaponList.Add(weapon);
            weaponGameObjectList[i].SetActive(false);
        }
        weaponGameObjectList[0].SetActive(true);
        weaponGameObject = weaponGameObjectList[0];
        weapon = weaponList[0];
    }

    public override void OnSkill()
    {
        if (canUseSkill)
        {
            nextWeapon();
            StartCoroutine(SkillCooldown());
        }
    }

    protected override void OnAttack()
    {
        // Set attackMultiplier to new value for the next attack
        attackMultiplier = getNextAttackMultiplier();
        Debug.Log("Current attack multiplier: " + attackMultiplier);
        base.OnAttack();
        HitEnemy();

        // Reset attackMultiplier
        attackMultiplier = 1f;
    }

    public override void HitEnemy()
    {
        base.HitEnemy();
        if (currentChargesForNextWeapon <= maxTotalCharges)
        {
            currentChargesForNextWeapon++;
        }
    }

    private float getNextAttackMultiplier()
    {
        // Used all charges
        if (usedCharges >= currentCharges)
        {
            return 1f;
        }
        // Next charge is weak
        else if (usedCharges <= maxWeakCharges)
        {
            usedCharges++;
            return weakChargeMultiplier;
        }
        // Next charge is medium
        else if (usedCharges <= maxWeakCharges + maxMediumCharges)
        {
            usedCharges++;
            return mediumChargeMultiplier;
        }
        // Next charge is strong
        else
        {
            usedCharges++;
            return strongChargeMultiplier;
        }
    }

    public void nextWeapon()
    {
        weaponGameObjectList[currentWeaponNumber].SetActive(false);
        // Makes the list loop back to the first element if it's at its end
        currentWeaponNumber = (currentWeaponNumber + 1) % weaponGameObjectList.Count;

        // I'm pretty sure this doesn't work like this
        weaponGameObjectList[currentWeaponNumber].SetActive(true);
        weaponGameObject = weaponGameObjectList[currentWeaponNumber];
        weapon = weaponList[currentWeaponNumber];

        //weaponGameObject.gameObject.transform.parent = this.gameObject.transform;
        //weaponGameObject.gameObject.transform.localPosition = new Vector3(0, 0, 0);

        // Set accumulated charges for the current weapon and reset charges for the next
        currentCharges = currentChargesForNextWeapon;
        currentChargesForNextWeapon = 0;
    }
}
