using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TrapType
{
    DPS, // damage: 0.3f maybe
    Burst // damage: 30f maybe
};

public class Traps : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private TrapType trapType;
    [SerializeField]
    private float damage;

    private bool takingDamage = false;
    private GameObject player;

    #endregion

    private void Update()
    {
        if(takingDamage && trapType == TrapType.DPS)
        {
            player.GetComponent<PlayerInfo>().Hurt(damage);
        }
        else
        {
            if(takingDamage && trapType == TrapType.Burst)
            {
                player.GetComponent<PlayerInfo>().Hurt(damage);
                takingDamage = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            takingDamage = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            takingDamage = false;
        }
    }
}
