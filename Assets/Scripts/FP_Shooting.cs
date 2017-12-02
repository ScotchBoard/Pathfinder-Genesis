using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FP_Shooting : MonoBehaviour
{
    //Drag in the Bullet Emitter from the Component Inspector.
    public GameObject bulletEmitter;

    private float nextFire = 0.0F;
    [SerializeField]
    private float fireRate = 0.5F;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private GameObject bulletFire;

    private ParticleSystem[] fireParticles;

    private AmmoManager ammoManager;

    //Drag in the Bullet Prefab from the Component Inspector.
    public GameObject bullet;

    //Enter the Speed of the Bullet from the Component Inspector.
    public float bulletForwardForce;

    private void Start()
    {
        ammoManager = GetComponent<AmmoManager>();
        fireParticles = bulletFire.GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time > nextFire && ammoManager.CanFire())
        {
            nextFire = Time.time + fireRate;

            FireParticles();

            //The Bullet instantiation happens here.
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(bullet, bulletEmitter.transform.position, bulletEmitter.transform.rotation) as GameObject;

            //Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
            //This is EASILY corrected here, you might have to rotate it from a different axis and or angle based on your particular mesh.
            //Temporary_Bullet_Handler.transform.Rotate(Vector3.left * 90);

            //Retrieve the Rigidbody component from the instantiated Bullet and control it.
            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

            //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
            Temporary_RigidBody.AddForce(transform.forward * bulletForwardForce);

            //Lower the ammo capacity
            ammoManager.Fire();

            //ammoImage.fillAmount -= 0.2f / ammo;

            Temporary_Bullet_Handler.GetComponent<BulletDamage>().Attacker("Player");

            //Basic Clean Up, set the Bullets to self destruct after 3 Seconds.
            Destroy(Temporary_Bullet_Handler, 6.0f);
        }
    }

    private void FireParticles()
    {
        foreach (var particle in fireParticles)
        {
            particle.Play();
        }
    }
}
