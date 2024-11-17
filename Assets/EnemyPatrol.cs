using Mirror;
using Telepathy;
using UnityEngine;

public class EnemyPatrol : NetworkBehaviour
{

    public float firerate = 0.5f;
    public float fireratetime = 1f;
    public Transform firepoint;
    public GameObject bulletPrefab;

    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;
    public float rotationSpeed = 5f;
    public int damageAmount = 10;

    public float detectionRange = 2f; 
    public float returnRange = 3f;   
    public LayerMask playerLayer;    
    private Transform currentTarget;
    private Transform chaseTarget;     

    void Start()
    {
        currentTarget = pointA;
    }

    void Update()
    {
        if(!isServer) { return; }
        FindClosestPlayerInRange();
        bulletshoot();
        if (chaseTarget != null)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void FindClosestPlayerInRange()
    {
       
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);

        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (Collider player in playersInRange)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

     
        if (closestPlayer != null)
        {
            chaseTarget = closestPlayer;
        }
        else
        {
            chaseTarget = null;
        }

      
        if (chaseTarget != null && Vector3.Distance(transform.position, chaseTarget.position) > returnRange)
        {
            chaseTarget = null;
        }
    }

    void Patrol()
    {
        Vector3 targetPosition = new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z);

        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    void ChasePlayer()
    {
        Vector3 targetPosition = new Vector3(chaseTarget.position.x, transform.position.y, chaseTarget.position.z);

        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.transform.parent.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null )
            {
                //Debug.Log("Player is in the trigger zone!");
                playerHealth.TakeDamage(damageAmount * Time.deltaTime);
                //Debug.Log("Player is taking damage from enemy!");
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, returnRange);
    }




    /////////////bullet spwan
    [Server]
    private void cmdbulletspawn()
    {
        GameObject bullet = Instantiate(bulletPrefab,firepoint.position, firepoint.rotation);
        NetworkServer.Spawn(bullet);
    }
  
    private void bulletshoot()
    {
        if (fireratetime > 0)
        {
            fireratetime -= Time.deltaTime;

        }
        else
        {
            cmdbulletspawn();
            fireratetime = 1/firerate;
        }
       
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
      
    }





}
