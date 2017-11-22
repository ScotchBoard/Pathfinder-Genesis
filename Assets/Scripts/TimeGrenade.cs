using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGrenade : MonoBehaviour
{
    public GameObject forceFieldEffect;
    public float delay = 3f;

    public bool launched = false;

    private float countdown;
    private bool hasExploded = false;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Explode();
            hasExploded = true;
        }
        
        if (launched)
        {
            countdown -= Time.deltaTime;

            if (countdown <= 0 && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        Instantiate(forceFieldEffect, transform.position, transform.rotation);

        launched = false;

        Destroy(gameObject);
    }
}
