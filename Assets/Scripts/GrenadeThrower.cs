using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [SerializeField]
    private float throwForce = 20f;
    [SerializeField]
    private GameObject grenadePrefab;

    private GameObject grenade;
    private AmmoManager ammoManager;

    private void Start()
    {
        if (GetComponentInParent<AmmoManager>() != null)
        {
            ammoManager = GetComponentInParent<AmmoManager>();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ThrowGrenade();
        }
    }

    private void ThrowGrenade()
    {
        if (grenade == null && !ForceField.isActive && ammoManager.CanThrowNade())
        {
            grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);

            grenade.gameObject.GetComponent<TimeGrenade>().launched = true;

            grenade.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.VelocityChange);

            ammoManager.ThrowNade();
        }
    }
}
