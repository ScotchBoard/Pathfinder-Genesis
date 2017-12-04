using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    [SerializeField]
    private int bulletAmmo;
    [SerializeField]
    private int maxBulletAmmo;
    [SerializeField]
    private int grenadeAmmo = 3;
    [SerializeField]
    private float reloadTime = 2f;

    public Text ammoInUseText, ammoInTotalText, grenadeAmmoText;
    public const int MAXAMMO = 100, MAXNADES = 5;

    private int ammoInUse, ammoInTotal, ammoDifference;
    private bool reloading = false;

    private void Awake()
    {
        ammoInUse = bulletAmmo;
        ammoInTotal = maxBulletAmmo;
    }

    private void Start()
    {
        StartCoroutine(ReloadTime(reloadTime));
    }

    private void Update()
    {
        if (!reloading)
        {
            ammoInUseText.text = ammoInUse.ToString();
            ammoInTotalText.text = ammoInTotal.ToString();
            grenadeAmmoText.text = grenadeAmmo.ToString();
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

    public void ThrowNade()
    {
        if(grenadeAmmo > 0)
        {
            grenadeAmmo--;
        }
    }

    public void Reload()
    {
        if(ammoInUse == 0)
        {
            if(ammoInTotal >= bulletAmmo)
            {
                ammoInUse = bulletAmmo;
                ammoInTotal -= bulletAmmo;
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
            if (ammoInTotal + ammoInUse >= bulletAmmo)
            {
                ammoDifference = bulletAmmo - ammoInUse;
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

    public void IncreaseAmmo(GameObject item, int ammo, int grenades)
    {
        // Increase the ammo
        bool usedAmmo = false;

        if(ammoInTotal + ammo < MAXAMMO)
        {
            ammoInTotal += ammo;
            usedAmmo = true;
        }
        else
        {
            if(ammoInTotal < MAXAMMO)
            {
                ammoInTotal = MAXAMMO;
                usedAmmo = true;
            }
        }
        
        // Increase the grenades
        if(grenadeAmmo + grenades < MAXNADES)
        {
            grenadeAmmo += grenades;
            usedAmmo = true;
        }
        else
        {
            if (grenadeAmmo < MAXNADES)
            {
                grenadeAmmo = MAXNADES;
                usedAmmo = true;
            }
        }

        // Destroy the ammo box
        if(usedAmmo)
        {
            Destroy(item);
        }
    }

    public bool CanFire()
    {
        return ammoInUse > 0 && !reloading;
    }

    public bool CanThrowNade()
    {
        return grenadeAmmo > 0;
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
