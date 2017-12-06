using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drone : MonoBehaviour
{
    [SerializeField]
    private float heightDistance = 2f;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float step;

    private TimeBehaviour timeBehaviour;
    private EnemyAI enemyAI;

    private void Start()
    {
        timeBehaviour = GetComponent<TimeBehaviour>();
        enemyAI = GetComponent<EnemyAI>();

        enemyAI.WaitForAttack();
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, heightDistance, transform.position.z);
        if (GameManager.INSTANCE.GameOver)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            if (!timeBehaviour.IsSlowed)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step * Time.deltaTime);

                enemyAI.LookAtTarget();
            }
            else
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

}
