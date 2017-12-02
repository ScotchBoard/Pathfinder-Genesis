//this shoots a projectile and updates the RotationalAmmoIndicator

using UnityEngine;
using System.Collections;

public class ShootScript : MonoBehaviour {

	public GameObject Projectile; //the thing that is being shot
	public int Ammo; //current ammount of ammo
	public int MaxAmmo; //max ammo
	public float Force; //force of the projectile being shot

	public GameObject SpawnLocation; //location the projectile is created
	public RotationalAmmoIndicator RAI; //the rotational ammo indicator

	//set the amount of ammo
	void Awake()
	{
		RAI.Count = Ammo;
		RAI.MaxCount = MaxAmmo;
	}

	//shoot the projectile
	public void Shoot()
	{
		if (Ammo > 0)
		{
			GameObject ThisProjectile = Instantiate(Projectile,SpawnLocation.transform.position,SpawnLocation.transform.rotation) as GameObject;
			ThisProjectile.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * Force);

			Ammo = Ammo -1;
			RAI.ChangeAmmo(Ammo);
		}
	}

	//reload the gun
	public void ReLoad()
	{
		Ammo = MaxAmmo;
		RAI.ChangeAmmo(Ammo);
	}
	
}
