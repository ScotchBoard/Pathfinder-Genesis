using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{ 
    [SerializeField]
    private float TimeValue = 0.01f;
    [SerializeField]
    private float duration = 5f;

    [HideInInspector]
    public static bool isActive;

    private void Start()
    {
        StartCoroutine(DestroyItself());
    }
    
    IEnumerator DestroyItself()
    {
        isActive = true;
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
        isActive = false;
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag != "Player" && collider.gameObject.layer != 13 && collider.gameObject.layer != 11)
        {
            if(collider.gameObject.GetComponent<Rigidbody>())
            {
                collider.gameObject.GetComponent<TimeBehaviour>().SaveData();
                collider.gameObject.GetComponent<TimeBehaviour>().TimeController(TimeValue);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.layer != 13 && other.gameObject.layer != 11)
        {
            if (other.gameObject.GetComponent<Rigidbody>())
            {
                other.gameObject.GetComponent<TimeBehaviour>().TimeController(1f);
            }
        }
    }
}
