using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public float hp;
    [SerializeField]
    public float contactDamage;
    private Player player;
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
        Debug.Log(other + "   " + other.tag);
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<HitBoxPlayer>().player;
            ContactDamage(player);
            player.damageToReceive += contactDamage;            
        }
        if(other.CompareTag("Minion"))
        {
            
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<HitBoxPlayer>().player;
            player.damageToReceive -= contactDamage;
        }
    }
    public void ContactDamage(Player player)
    {
        player.TakeDamage(contactDamage);
    }
}
