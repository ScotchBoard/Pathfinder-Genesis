using Assets.Scripts;
using ProgressBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour, IUnits
{
    [SerializeField]
    private float enemyHealth = 3;
    [SerializeField]
    private GameObject healthBar;

    private ProgressBarBehaviour healthBarBehaviour;

    private void Start()
    {
        healthBarBehaviour = healthBar.GetComponent<ProgressBarBehaviour>();

        healthBarBehaviour.maxSize = enemyHealth;
        healthBarBehaviour.IncrementValue(enemyHealth);
    }

    public void Hurt(float damage)
    {
        enemyHealth -= damage;
        healthBarBehaviour.DecrementValue(damage);

        CheckIfAlive();
    }

    private void CheckIfAlive()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
