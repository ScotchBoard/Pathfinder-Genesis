using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGrenade : MonoBehaviour
{
    // Private
    [SerializeField]
    private GameObject forceFieldEffect;
    [SerializeField]
    private float delay = 3f;

    private float countdown;
    private bool hasExploded = false;

    // Public
    public bool Launched { get; set; }

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
        
        if (Launched)
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

        Launched = false;

        Destroy(gameObject);
    }
}
