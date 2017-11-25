using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE = null;

    public bool GameOver { get; set; }
    public bool IsReviving { get; set; }

    void Awake()
    {
        //Check if instance already exists
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        else
        {
            if (INSTANCE != this)
            {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }
}
