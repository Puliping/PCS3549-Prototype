using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeWeapon : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject hitbox;
    [SerializeField]
    private Enemy enemy;

    private float attackDuration;
    void Start()
    {
        attackDuration = enemy.attackDuration;
    }

    public void Activate()
    {
        StartCoroutine(HitBoxManager(attackDuration));
    }

    IEnumerator HitBoxManager(float duration)
    {

        yield return new WaitForSeconds(duration);
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(HitBoxManager(attackDuration));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log("Collision tag: "+collision.gameObject.tag);
        if (collision.gameObject.CompareTag("PlayerHitBox") )
        {
            collision.gameObject.GetComponentInParent<Player>().ReceiveDamage(1f);
        }
    }
    void Update()
    {
        
    }
}
