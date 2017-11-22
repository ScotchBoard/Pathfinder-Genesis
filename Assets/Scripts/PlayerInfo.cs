using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private int playerHealth = 10;
    public static int playerHealthStatic;
    public static bool gameOver;
    public static bool isReviving = false;

    [SerializeField]
    private int enemyHealth = 3;

    private void Start()
    {
        gameOver = false;
        playerHealthStatic = playerHealth;
    }

    private void Update()
    {
        //Debug.Log("Health: " + playerHealth);

        if (playerHealthStatic <= 0)
        {
            gameOver = true;
        }
    }

    public void Hurt(int damage)
    {
        if (gameObject.tag == "Player")
        {
            playerHealthStatic -= damage;
        }
        else
        {
            enemyHealth -= damage;
        }
        CheckIfAlive();
    }

    private void CheckIfAlive()
    {
        if(gameObject.tag == "Enemy" && enemyHealth <=0)
        {
            Destroy(gameObject);
        }
        else
        {
            if (gameObject.tag == "Player" && playerHealthStatic <= 0)
            {
                gameOver = true;
                Debug.Log("You are dead, not a big surprise.");
            }
        }
    }
}
