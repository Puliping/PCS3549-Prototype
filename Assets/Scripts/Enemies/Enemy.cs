using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class Enemy : MonoBehaviour
{


    public float HP;
    public float base_speed;
    public float sighting_time = 0.5f;
    public float attackCooldown = 3f;
    public float attackDuration = 0.5f;
    public float attack_range = 2f;
    public float sight_range = 3f;
    [SerializeField]
    public GameModeController gameModeController; //TO-DO make slimes able to get player/minions objects references during runtime

    [SerializeField]
    GameObject fireworks;
    protected GameObject aggroTarget = null;
    protected bool chasing_player = false;

    protected EnemyMoviment moviment;
    protected bool onAttackCooldown=false;
    public States enemyState = States.Patrol;
    public enum States
    {
        Follow,
        Patrol,
        Attack
    }



    public virtual void Start()
    {
        moviment = GetComponent<EnemyMoviment>();
        StartCoroutine(LineOfSightLoop());
        gameModeController = GameModeController.Instance;
        gameModeController.enemiesAlive++;
    }

    protected virtual bool CanAttack()
    {
        if (aggroTarget && !onAttackCooldown)
        {
            if ( Vector2.Distance(aggroTarget.transform.position,transform.position) < attack_range)
            {
                return true;
            }
        }
        return false;
    }


    protected virtual IEnumerator AttackCooldown()
    {
        onAttackCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        onAttackCooldown = false;

    }
    protected virtual IEnumerator AttackDuration()
    {
        moviment.SetSpeedModifier(0f);
        yield return new WaitForSeconds(attackDuration);
        moviment.SetSpeedModifier(1f);
        enemyState = States.Follow;

    }


    public virtual void Attack()
    {

        /*
         * To do.
         */
        if (!CanAttack()) return;
        
        StartCoroutine(AttackCooldown());
        Coroutine attackCoroutine = StartCoroutine(AttackDuration());
    }

 
    public virtual void Patrol()
    {
        /* By default, enemies don't move. */
        enemyState = States.Patrol;
        if (aggroTarget) enemyState = States.Follow;
        //if (moviment.ReachedEndOfPath()) moviment.SetTargetPosition(transform.position + Vector3.zero, 0f);
        
        
    }

    public virtual void Follow()
    {
        /* By default, enemies don't move*/
        enemyState = States.Follow;
        if (!aggroTarget && moviment.ReachedEndOfPath() )
        {
            enemyState = States.Patrol;
        }
        //if(aggroTarget) moviment.SetTargetPosition(GetAggroTarget().transform.position + Vector3.zero, 1f);
    }





    protected IEnumerator LineOfSightLoop()
    {
        /* Default loop for detecting players in sight. It is called every [sighting_time] seconds.
         * If the enemy has a target, it is considered to be aggroed, and will check for enemies 10x more frequently.
         * If it is aggroed, it will first only check if it can see the target. If it can't, it will search for other players/minions in sight.*/
        float time_modifier = 1f;
        GameObject target;
        while (true)
        {
            target = GetAggroTarget();
            time_modifier = (target) ? 0.1f : 1f;
            yield return new WaitForSeconds(sighting_time*time_modifier);
            if (target)
            {
                if (!CanSeePlayer(target))
                    ClosestPlayerInSight();
            }
            else
                ClosestPlayerInSight();
        }
    }

    public GameObject ClosestPlayerInSight()
    {
        /* Returns a single gameobject containing the closest Player in line of sight. Also updates "aggroTarget".*/
        List<GameObject> player_list = GetAllPlayers();
        //Sorts list by distance to the enemy.
        player_list.Sort(delegate (GameObject a, GameObject b) {
            return Vector2.Distance(this.transform.position, a.transform.position).CompareTo(Vector2.Distance(this.transform.position, b.transform.position));
        });


        foreach (GameObject p in player_list)
        {

            if (CanSeePlayer(p))
            {
                aggroTarget = p;
                return p;
            }
        }
        
        aggroTarget = null;
        return null;
    }

    protected Vector2 endpos=Vector2.down;
    public bool CanSeePlayer(GameObject player_to_find)
    {
        /**Checks if enemy can see a specific gameobject, within a specified sight_range. If there are walls between enemy/gameobject, returns false.*/

        float final_sight_range = (aggroTarget) ? sight_range*3 : sight_range;

        endpos = this.transform.position + (player_to_find.transform.position - this.transform.position).normalized * final_sight_range;

        if (Vector2.Distance(player_to_find.transform.position, this.transform.position) > final_sight_range)
            return false; //Don't even do raycasts if player is too far away to be seen.

        
        LayerMask hitLayers = 1 << LayerMask.NameToLayer("Walls") | 1 << LayerMask.NameToLayer("Player");
        RaycastHit2D hit = Physics2D.Linecast(transform.position, endpos, hitLayers);// Raycasts a line between the enemy and player. Checks if there is a wall in the middle.
        
        if (hit.collider != null)
        {
            
            return hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("PlayerHitBox"); //If there is a wall between enemy/player, returns false.
        }
        return false;
    }

    public GameObject GetAggroTarget()
    {
        return aggroTarget;
    }

    void OnDrawGizmos()
    {
        // Draw line from enemy to player.
        Gizmos.color = (aggroTarget) ? Color.red : Color.blue;
        Gizmos.DrawLine(transform.position, endpos);
    }

    protected List<GameObject> GetAllPlayers() 
    {
        return  gameModeController.GetPlayers().ToList<GameObject>() ;
    }

    public virtual void TakeDamage(float damage)
    {
        HP = HP - damage;
        if (HP < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        GameModeController.Instance.EnemyDeath();
        /** Called by ReceiveDamage() when HP<0.*/

        Instantiate(fireworks,transform.position,transform.rotation);
        Destroy(this.gameObject, 0.0f); //Destroys after 0.0 seconds.
        
    }


    public virtual void UpdateVisual()
    {

    }

    public virtual void UpdateAnimations()
    {

    }

    public virtual void Update()
    {

        if (CanAttack()) enemyState = States.Attack;

        switch (enemyState) {
            case States.Patrol:
                Patrol();
                break;
            case States.Follow:
                Follow();
                break;
            case States.Attack:
                Attack();
                break;
        }
            

        UpdateAnimations();
        UpdateVisual();
    }


}
