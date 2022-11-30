using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine;

public class Frenzy : Player
{
    public class ChargeAndOffset
    {
        public GameObject chargeSprite;
        public float distAngle;
        public float angle;
        public float offset;
        public float speed;
        public ChargeAndOffset (GameObject chargeSprite, float angle, float distAngle,float speed)
        {
            offset = 1f;
            this.angle = angle;
            this.chargeSprite = chargeSprite;
            this.distAngle = distAngle;
            this.speed = speed;
        }

        public void  UpdateAngles(float increment)
        {
            this.angle += increment*1.2f*speed;
            this.distAngle += increment*0.75453f*speed;
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
            ChargeAndOffset co;
            SpriteRenderer re;
            for (int i = 0; i < chargeSpriteList.Count; i++)
            {
                co = chargeSpriteList[i];
                float offset = co.offset * (Mathf.Cos(co.distAngle) * Mathf.Cos(co.distAngle) * 0.35f + 0.6f);
                co.chargeSprite.transform.position = new Vector2(transform.position.x + offset * Mathf.Cos(co.angle), transform.position.y + offset * Mathf.Sin(co.angle));

                co.UpdateAngles(4f * Time.deltaTime);
                re = co.chargeSprite.GetComponent<SpriteRenderer>();
                re.color = new Color(1f * (Mathf.Cos(co.angle * 1.3f/4f) * Mathf.Cos(co.angle * 1.3f/4f)), 1f * (Mathf.Cos(co.angle * 0.7f / 4f) * Mathf.Cos(co.angle * 0.7f / 4f)), 1f * (Mathf.Cos(co.angle * 0.2f / 4f) * Mathf.Cos(co.angle * 0.2f / 4f)),1f);
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
        float angle = Random.Range(0f, Mathf.PI*2f);
        float distangle = Random.Range(0f, Mathf.PI * 2f);

        chargeSpriteList.Add(new ChargeAndOffset(Instantiate(chargeSprite, this.transform), angle, distangle, Random.Range(0.5f, 1.4f)));
        //chargeSpriteList.Last().chargeSprite.transform.position = new Vector2(transform.position.x + offset, transform.position.y + offset);
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
