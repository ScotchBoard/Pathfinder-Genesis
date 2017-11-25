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
    private int playerHealth = 10;

    private ProgressBarBehaviour progressBarBehaviour;

    private void Start()
    {
        progressBarBehaviour = healthBar.GetComponent<ProgressBarBehaviour>();
    }

    public void SetPlayerHealth(int playerHealth)
    {
        this.playerHealth = playerHealth;
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public void Hurt(int damage)
    {
        playerHealth -= damage;

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
}
