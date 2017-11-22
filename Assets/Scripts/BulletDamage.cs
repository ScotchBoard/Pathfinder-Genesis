using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    private string sender;

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
        if (other.gameObject.tag == "Player" || (other.gameObject.tag == "Enemy" && sender == "Player"))
        {
            PlayerInfo player = other.gameObject.GetComponent<PlayerInfo>();
            player.Hurt(damage);
            Destroy(gameObject);
        }
    }
}
