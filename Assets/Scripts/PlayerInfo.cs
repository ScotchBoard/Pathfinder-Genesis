using Assets.Scripts;
using ProgressBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IUnits
{
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private GameObject energyBar;
    [SerializeField]
    private float playerHealth = 100;
    [SerializeField]
    private float playerEnergy = 100;

    private ProgressBarBehaviour healthBarBehaviour;
    private ProgressBarBehaviour energyBarBehaviour;
    private float totalPlayerEnergy;

    private void Start()
    {
        totalPlayerEnergy = playerEnergy;

        healthBarBehaviour = healthBar.GetComponent<ProgressBarBehaviour>();
        energyBarBehaviour = energyBar.GetComponent<ProgressBarBehaviour>();

        energyBarBehaviour.IncrementValue(playerEnergy);
        healthBarBehaviour.IncrementValue(playerEnergy);
    }

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

    public void Hurt(int damage)
    {
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
}
