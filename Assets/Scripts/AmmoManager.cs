using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    [SerializeField]
    private int ammo;
    [SerializeField]
    private int clips;

    public Text ammoInUseText, ammoInTotalText;

    private int ammoInUse, ammoInTotal, ammoDifference;

    private void Awake()
    {
        ammoInUse = ammo;
        ammoInTotal = clips * ammo;
    }

    private void Update()
    {
        ammoInUseText.text = ammoInUse.ToString();
        ammoInTotalText.text = ammoInTotal.ToString();

        if (Input.GetKey(KeyCode.R))
        {
            Reload();
        }
    }

    public int AmmoCount()
    {
        return ammoInUse;
    }

    public void Fire()
    {
        if (ammoInUse > 0)
        {
            ammoInUse--;
        }
    }

    public void Reload()
    {
        if(ammoInUse == 0)
        {
            if(ammoInTotal >= ammo)
            {
                ammoInUse = ammo;
                ammoInTotal -= ammo;
            }
            else
            {
                ammoInUse = ammoInTotal;
                ammoInTotal = 0;
            }
        }
        else
        {
            if (ammoInTotal + ammoInUse >= ammo)
            {
                ammoDifference = ammo - ammoInUse;
                ammoInUse += ammoDifference;
                ammoInTotal -= ammoDifference;
            }
            else
            {
                ammoInUse += ammoInTotal;
                ammoInTotal = 0;
            }
        }
    }

    public bool CanFire()
    {
        return ammoInUse > 0;
    }
}
