
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class EnemyBrain : MonoBehaviour
{


    public GameObject player;
    private EnemyMovement internalMovement;
    private bool idlePathCooldown=true;
    private bool isIdle = true;

    // Start is called before the first frame update
    void Start()
    {
        internalMovement = GetComponent<EnemyMovement>();
        spawn_position = transform.position + Vector3.zero;
        StartCoroutine(SeePlayerLoop(0.8f));
        
    }

    public float aggro_range = 5f; //Distance to see player
    private Vector2 endpos = Vector2.down;
    public bool seeingPlayer = false;
    bool CanSeePlayer()
    {
        float aggro_range_multiplier = (chasing_player) ? 3f : 1f; //seeks player much further while chasing him.
        seeingPlayer = false;
        endpos = this.transform.position + (player.transform.position - this.transform.position).normalized * aggro_range * aggro_range_multiplier;
        LayerMask hitLayers = 1 << LayerMask.NameToLayer("Walls") | 1 << LayerMask.NameToLayer("Player");
        RaycastHit2D hit = Physics2D.Linecast(transform.position, endpos, hitLayers);

        
        if (hit.collider != null)
        {
            seeingPlayer = hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("PlayerHitBox");
            return seeingPlayer;
        }
        return false;
    }


    void OnDrawGizmos()
    {
        // Draw line from enemy to player.
        Gizmos.color = (seeingPlayer) ? Color.red : Color.blue;
        Gizmos.DrawLine(transform.position, endpos);
    }

    public bool chasing_player = false;
    void Update()
    {
        if (seeingPlayer)
        {
            chasing_player = true;
            internalMovement.SetTargetPosition(player.transform.position+Vector3.zero,1f); //Adds zero to pass a copy of player position
        }

        else
        {
            if (internalMovement.ReachedEndOfPath())chasing_player=false;

            if (idlePathCooldown && !chasing_player)
            {
                internalMovement.SetTargetPosition(GetIdlePosition(),0.4f);
                StartCoroutine(pathCooldown());
            }
        }
    }

    private IEnumerator SeePlayerLoop(float aggro_time)
    {
        float time_modifier = 1f;
        while (true)
        {
            time_modifier = (chasing_player) ? 0.1f : 1f; //Tracks player much faster while chasing him.
            yield return new WaitForSeconds(aggro_time);
            CanSeePlayer();
        }
    }

    // Cooldown for idle paths while not doing anything.
    private IEnumerator pathCooldown()
    {
        idlePathCooldown = false;
        yield return new WaitForSeconds(4f);
        idlePathCooldown = true;
    }

    private Vector3 spawn_position;
    private Vector3 GetIdlePosition()
    {
        return spawn_position + (Vector3)(Random.insideUnitCircle * 2);
    }

}
