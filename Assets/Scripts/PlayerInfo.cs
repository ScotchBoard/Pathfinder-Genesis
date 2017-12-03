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

    private ProgressBarBehaviour healthBarBehaviour;
    private ProgressBarBehaviour energyBarBehaviour;
    private float totalPlayerEnergy;

    private AmmoManager ammoManager;

    // Public
    public const int MAXHP = 100;
    #endregion

    #region Start - Update
    private void Start()
    {
        ammoManager = GetComponentInChildren<AmmoManager>();

        totalPlayerEnergy = playerEnergy;

        healthBarBehaviour = healthBar.GetComponent<ProgressBarBehaviour>();
        energyBarBehaviour = energyBar.GetComponent<ProgressBarBehaviour>();

        energyBarBehaviour.IncrementValue(playerEnergy);
        healthBarBehaviour.IncrementValue(playerEnergy);
    }

    private void Update()
    {
        if (totalPlayerEnergy < 100)
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

    public void Hurt(int damage)
    {
        damage *= 10;
        playerHealth -= damage;

        healthBarBehaviour.DecrementValue(damage);

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
        // TODO energy item
        if (other.gameObject.tag == "HP")
        {
            if (playerHealth < MAXHP)
            {
                IncreaseHealth();
                Destroy(other.gameObject);
            }
        }
        else
        {
            if (other.gameObject.tag == "Ammo")
            {
                ammoManager.IncreaseAmmo(other.gameObject, ammoGain);
            }
        }
    }

    private void IncreaseHealth()
    {
        int hpIncrease = 0;

        if(playerHealth + hpGain <= MAXHP)
        {
            hpIncrease = hpGain;
            playerHealth += hpIncrease;
        }
        else
        {
            hpIncrease = MAXHP - (int)playerHealth;
            playerHealth += hpIncrease;
        }

        healthBarBehaviour.IncrementValue(hpIncrease);
    }

    private void EnergyRegen(float energyRegen)
    {
        // TODO test the use of energy after depleted, e.g: you can't dash when you reach 0 and energy regens above 10
        float energy = Time.deltaTime / energyRegen;
        totalPlayerEnergy += energy;
        energyBarBehaviour.Value = totalPlayerEnergy;
    }
}
