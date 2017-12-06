using Assets.Scripts;
using ProgressBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IUnits
{
    #region Variables
    // Private
    [Header("Health & Energy")]
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private GameObject energyBar;
    [SerializeField]
    private float playerHealth = 100;
    [SerializeField]
    private float playerEnergy = 100;
    [SerializeField]
    private float energyRegen = 1f;

    [Header("Items")]
    [SerializeField]
    private int hpGain = 20;
    [SerializeField]
    private int ammoGain = 40;
    [SerializeField]
    private int grenadeGain = 2;
    [SerializeField]
    private int energyGain = 20;

    private ProgressBarBehaviour healthBarBehaviour;
    private ProgressBarBehaviour energyBarBehaviour;
    private float totalPlayerEnergy;

    private AmmoManager ammoManager;

    // Public
    public const int MAX_HP = 100;
    public const int MAX_ENERGY = 100;
    #endregion

    #region Start - Update

    private void Start()
    {
        ammoManager = GetComponentInChildren<AmmoManager>();

        totalPlayerEnergy = playerEnergy;

        healthBarBehaviour = healthBar.GetComponent<ProgressBarBehaviour>();
        energyBarBehaviour = energyBar.GetComponent<ProgressBarBehaviour>();

        healthBarBehaviour.maxSize = MAX_HP;
        energyBarBehaviour.maxSize = MAX_ENERGY;

        energyBarBehaviour.IncrementValue(playerEnergy);
        healthBarBehaviour.IncrementValue(playerHealth);
    }

    private void Update()
    {
        if (totalPlayerEnergy < MAX_ENERGY)
        {
            EnergyRegen(energyRegen);
        }
    }

    #endregion

    #region Energy and Health get/setters

    public void SetPlayerEnergy(float playerEnergy)
    {
        energyBarBehaviour.Value = totalPlayerEnergy - playerEnergy;
        totalPlayerEnergy -= playerEnergy;

        this.playerEnergy = playerEnergy;
    }

    public float GetPlayerEnergy()
    {
        return playerEnergy;
    }

    public void SetPlayerHealth(float playerHealth)
    {
        healthBarBehaviour.Value = playerHealth;

        this.playerHealth = playerHealth;
    }

    public float GetPlayerHealth()
    {
        return playerHealth;
    }

    #endregion

    public void Hurt(float damage)
    {
        //damage *= 10;
        playerHealth -= damage;

        //healthBarBehaviour.DecrementValue(damage);
        healthBarBehaviour.Value = playerHealth;

        CheckIfAlive();
    }

    private void CheckIfAlive()
    {
        if (playerHealth <= 0)
        {
            GameManager.INSTANCE.GameOver = true;
            Debug.Log("You are dead, not a big surprise.");
        }
    }

    public bool CanUseEnergy()
    {
        return totalPlayerEnergy > 0;
    }

    public bool CanUseEnergyDash()
    {
        // TODO sometimes when the energy is less than 10 you can still dash
        return totalPlayerEnergy > 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HP")
        {
            if (playerHealth < MAX_HP)
            {
                IncreaseHealth();
                Destroy(other.gameObject);
            }
        }
        else
        {
            if (other.gameObject.tag == "Ammo")
            {
                ammoManager.IncreaseAmmo(other.gameObject, ammoGain, grenadeGain);
            }
            else
            {
                if(other.gameObject.tag == "Energy")
                {
                    if (totalPlayerEnergy < MAX_ENERGY)
                    {
                        IncreaseEnergy();
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }

    private void IncreaseHealth()
    {
        int hpIncrease = 0;

        if(playerHealth + hpGain <= MAX_HP)
        {
            hpIncrease = hpGain;
            playerHealth += hpIncrease;
        }
        else
        {
            hpIncrease = MAX_HP - (int)playerHealth;
            playerHealth += hpIncrease;
        }

        healthBarBehaviour.IncrementValue(hpIncrease);
    }

    private void IncreaseEnergy()
    {
        int energyIncrease = 0;

        if (totalPlayerEnergy + energyGain <= MAX_ENERGY)
        {
            energyIncrease = energyGain;
            totalPlayerEnergy += energyIncrease;
        }
        else
        {
            energyIncrease = MAX_ENERGY - (int)totalPlayerEnergy;
            totalPlayerEnergy += energyIncrease;
        }

        energyBarBehaviour.IncrementValue(energyIncrease);
    }
    
    private void EnergyRegen(float energyRegen)
    {
        // TODO test the use of energy after depleted, e.g: you can't dash when you reach 0 and energy regens above 10
        float energy = Time.deltaTime / energyRegen;
        totalPlayerEnergy += energy;
        energyBarBehaviour.Value = totalPlayerEnergy;
    }
}
