using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public float hp;
    [SerializeField]
    public float contactDamage;
    public GameObject fireworks;
    private List<Player> players_being_damaged = new List<Player>();
    [SerializeField]
    private GameObject textDamage_GO;
    [SerializeField]
    private TextMeshProUGUI textDamage;

    private EnemyBrain brain;
    // Start is called before the first frame update
    void Start()
    {
        brain = GetComponentInParent<EnemyBrain>();
        GameModeController.Instance.enemiesAlive++;
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

        brain.AggroEnemy();
        textDamage_GO.SetActive(true);
        textDamage.text = "-" + damage.ToString();
        StartCoroutine(CooldownText());

        if (hp <= 0)
        {
            morreu();
            return;
        }
        


        // faz o urro @marcin
    }
    IEnumerator CooldownText()
    {
        yield return new WaitForSeconds(1f);
        textDamage_GO.SetActive(false);
    }
    public void morreu()
    {
        Debug.Log("F no chat // morreu");
        transform.parent.gameObject.SetActive(false);
        // morreu
        GameModeController.Instance.EnemyDeath();
    }

    private void OnDisable()
    {
        Instantiate(fireworks,this.transform.position,Quaternion.identity);
    }
}
