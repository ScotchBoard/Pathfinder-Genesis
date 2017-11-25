using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour, IUnits
{
    [SerializeField]
    private int enemyHealth = 3;

    public void Hurt(int damage)
    {
        enemyHealth -= damage;

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
