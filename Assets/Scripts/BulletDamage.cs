using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    private string sender;
    private IUnits unit;
    private bool targetAcquired = false;

    private void Start()
    {
        unit = null;
    }

    private void Update()
    {
        Destroy(gameObject, 6.0f);
    }

    public void Attacker(string attacker)
    {
        sender = attacker;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            unit = other.gameObject.GetComponent<PlayerInfo>();
            targetAcquired = true;
        }
        else
        {
            if (other.gameObject.tag == "Enemy" && sender == "Player")
            {
                unit = other.gameObject.GetComponent<EnemyInfo>();
                targetAcquired = true;
            }
        }

        if(other.gameObject.tag == "Player" && sender == "Player")
        {
            targetAcquired = false;
        }

        if (unit != null && targetAcquired)
        {
            unit.Hurt(damage);
            Destroy(gameObject);
            targetAcquired = false;
        }
    }
}
