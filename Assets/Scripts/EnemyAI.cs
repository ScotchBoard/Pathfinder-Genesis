using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float bulletForwardForce;
    [SerializeField]
    private float heightDistance = 1.5f;
    [SerializeField]
    private float attackSpeed = 0.3f;
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private float playerRespawnWaitTime = 4f;
    [SerializeField]
    private float attackRange = 80f;
    [SerializeField]
    private float playerInRangeDistance = 120f;

    private GameObject bullet;
    private NavMeshAgent navMeshAgent;
    private TimeBehaviour timeBehaviour;
    private RaycastHit hit;
    private int layer_mask;

    private Vector3 offsetAttack, offsetRange; // Distance between this and player

    private bool canAttack = true;
    private Transform target;

    private Animator animator;

    #endregion

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        timeBehaviour = GetComponent<TimeBehaviour>();
        target = player.transform;
        layer_mask = LayerMask.GetMask("Player"); // So it only hits the player

        animator = GetComponent<Animator>();

        StartCoroutine(WaitForAttack());
    }

    private void Update()
    {
        if (navMeshAgent != null)
        {
            if (GameManager.INSTANCE.GameOver)
            {
                navMeshAgent.isStopped = true;
            }
            else
            {
                if (!timeBehaviour.IsSlowed && CheckPlayerInRange())
                {
                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(target.position);

                    LookAtTarget();
                }
                else
                {
                    navMeshAgent.isStopped = true;
                }

                // Attack();
            }
        }
    }

    // TODO animations
    private void Animations()
    {
    }

    private bool CheckDistanceForAttack()
    {
        offsetAttack = transform.position - player.transform.position;

        return (offsetAttack.magnitude <= attackRange);
    }

    private bool CheckPlayerInRange()
    {
        offsetRange = transform.position - player.transform.position;

        return (offsetRange.magnitude <= playerInRangeDistance);
    }

    private void Attack()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        int layerMask = 1 << 8;
        //layerMask = ~layerMask;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            /*
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            */
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.tag == "Player")//hitObject.GetComponent<PlayerInfo>())
            {
                bullet = Instantiate(bulletPrefab);
                bullet.transform.position = transform.position + new Vector3(0, heightDistance, 0);
                bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForwardForce);
                bullet.GetComponent<BulletDamage>().Attacker(gameObject.tag);
            }
        }
        /*
        if (Physics.Raycast(transform.position, Vector3.forward, Mathf.Infinity, layerMask))
        {
            Debug.Log("The ray hit the player");
            if (Physics.SphereCast(ray, 0.75f, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.tag == "Player")//hitObject.GetComponent<PlayerInfo>())
                {
                    bullet = Instantiate(bulletPrefab);
                    bullet.transform.position = transform.position + new Vector3(0, heightDistance, 0);
                    bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForwardForce);
                    bullet.GetComponent<BulletDamage>().Attacker(gameObject.tag);
                }
            }
        }*/
    }


    public void LookAtTarget()
    {
        var targetRotation = Quaternion.LookRotation(target.position - transform.position);

        // Smoothly rotate towards the target point
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public IEnumerator WaitForAttack()
    {
        while (true)
        {
            if (!GameManager.INSTANCE.GameOver)
            {
                if(GameManager.INSTANCE.IsReviving)
                {
                    yield return new WaitForSeconds(playerRespawnWaitTime);
                    GameManager.INSTANCE.IsReviving = false;
                }
                if (CheckDistanceForAttack())
                {
                    Attack();
                }
            }
            canAttack = false;
            yield return new WaitForSeconds(attackSpeed);
            canAttack = true;
        }
    }

}
