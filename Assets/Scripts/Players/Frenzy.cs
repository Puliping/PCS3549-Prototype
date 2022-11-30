using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine;

public class Frenzy : Player
{
    public struct ChargeAndOffset
    {
        public GameObject chargeSprite;
        public float offset;
        public ChargeAndOffset (GameObject chargeSprite, float offset)
        {
            this.offset = offset;
            this.chargeSprite = chargeSprite;
        }
    }

    public float baseSkillCD = 10f;
    public float baseMovSpeedMultiplierPerCharge = .05f;
    public float baseAttackMultiplierPerCharge = .01f;

    // Charge variables
    private int hitsNeededForCharge = 3;
    private int currentHits = 0;
    private int maxCharges = 10;
    private int currentChargeCounter = 0;
    private float chargeUptime = 10f;
    // Actual number of charges is hard-capped by maxCharges
    private int currentCharges => Mathf.Min(currentChargeCounter, maxCharges);

    public GameObject chargeSprite;
    public List<ChargeAndOffset> chargeSpriteList = new List<ChargeAndOffset>();

    private void Update()
    {
        if (chargeSpriteList != null)
        {
            foreach (ChargeAndOffset co in chargeSpriteList)
            {
                co.chargeSprite.transform.RotateAround(transform.position, new Vector3(0, 0, 1), 45 * Time.deltaTime);
            }
        }
    }

    public override void OnSkill()
    {

        if (canUseSkill)
        {
            Debug.Log("OnSkill");
            StartCoroutine(SkillCooldown(baseSkillCD));
            Debug.Log("Not implemented :D");
            // RemoveAllCharges();
        }
    }

    protected override void OnAttack()
    {
        base.OnAttack();
        HitEnemy();
    }

    public override void HitEnemy()
    {
        base.HitEnemy();
        if (++currentHits == hitsNeededForCharge)
        {
            // Just setting an upper bound so the charge number doesn't break anything
            if (currentChargeCounter <= 3 * maxCharges)
            {
                StartCoroutine(Charge());
                currentHits = 0;
            }
        }
    }

    IEnumerator Charge()
    {
        Debug.Log("Frenzy: new charge");
        AddCharge();
        yield return new WaitForSeconds(chargeUptime);
        RemoveCharge();
    }

    void AddCharge()
    {
        float offset = Random.Range(0.05f, 0.9f);
        chargeSpriteList.Add(new ChargeAndOffset(Instantiate(chargeSprite, this.transform), offset));
        chargeSpriteList.Last().chargeSprite.transform.position = new Vector2(transform.position.x + offset, transform.position.y + offset);
        chargeSpriteList.Last().chargeSprite.SetActive(true);
        currentChargeCounter++;
        UpdateModifiers();
    }

    void RemoveCharge()
    {
        // chargeSpriteList[0].chargeSprite.SetActive(false);
        Destroy(chargeSpriteList[0].chargeSprite);
        chargeSpriteList.RemoveAt(0);
        currentChargeCounter--;
        UpdateModifiers();
    }

    void RemoveAllCharges()
    {
        for (int i = 0; i < currentChargeCounter; i++)
        {
            StopCoroutine(Charge());
        }
        currentChargeCounter = 0;
        UpdateModifiers();
    }

    void UpdateModifiers()
    {
        movSpeedMultiplier = currentCharges * baseMovSpeedMultiplierPerCharge;
        attackMultiplier = currentCharges * baseAttackMultiplierPerCharge;
    }

}
