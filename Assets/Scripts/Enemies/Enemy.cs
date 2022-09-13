using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public float hp;
    [SerializeField]
    public float contactDamage;
    private List<Player> players_being_damaged = new List<Player>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitBox"))
        {
            Player player = other.GetComponent<HitBoxPlayer>().player;
            ContactDamage(player);
            player.damageToReceive += contactDamage;
            if (!players_being_damaged.Contains(player))
                players_being_damaged.Add(player);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitBox"))
        {
            Player player = other.GetComponent<HitBoxPlayer>().player;
            player.damageToReceive -= contactDamage;
            players_being_damaged.Remove(player);
        }
    }


    private void UpdateContactDamage()
    {
        foreach (Player p in players_being_damaged)
        {
            p.damageToReceive -= contactDamage;
        }
    }

    private void OnDestroy()
    {
        UpdateContactDamage();
    }

    public void ContactDamage(Player player)
    {
        player.TakeDamage(contactDamage);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            morreu();
        }
        // faz o urro @marcin
    }

    public void morreu()
    {
        Debug.Log("F no chat // morreu");
        transform.parent.gameObject.SetActive(false);
        // morreu
    }
}
