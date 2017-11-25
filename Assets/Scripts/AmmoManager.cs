﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    [SerializeField]
    private int ammo;
    [SerializeField]
    private int clips;
    [SerializeField]
    private float reloadTime = 2f;

    public Text ammoInUseText, ammoInTotalText;

    private int ammoInUse, ammoInTotal, ammoDifference;
    private bool reloading = false;

    private void Start()
    {
        StartCoroutine(ReloadTime(reloadTime));
    }

    private void Awake()
    {
        ammoInUse = ammo;
        ammoInTotal = clips * ammo;
    }

    private void Update()
    {
        if (!reloading)
        {
            ammoInUseText.text = ammoInUse.ToString();
            ammoInTotalText.text = ammoInTotal.ToString();
        }

        if (Input.GetKey(KeyCode.R) || ammoInUse == 0)
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
        if (ammoInUse > 0 && !reloading)
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
            reloading = true;
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
            reloading = true;
        }
    }

    public bool CanFire()
    {
        return ammoInUse > 0 && !reloading;
    }

    IEnumerator ReloadTime(float time)
    {
        while (true)
        {
            if (reloading)
            {
                yield return new WaitForSeconds(time);
                reloading = false;
            }
            else
            {
                yield return new WaitForSeconds(0f);
            }
        }
    }
}
