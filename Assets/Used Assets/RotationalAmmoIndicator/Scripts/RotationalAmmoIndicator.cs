//creates ammo indicators and rotates the button while ammo changes

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RotationalAmmoIndicator : MonoBehaviour {

	[Header("Image variables")]
	public GameObject IndicatorGameObject; //a game object that contains the image of one ammo Indicator...see AmmoIndicator in prefabs folder
	public Color IndicatorColor1; //the color the ammo should be when it exists
	public Color IndicatorColor2; //the color the ammo should be when it does not exists

	[Header("Rotation variables")]
	public bool CounterClockWise = true; //which way it should rotate
	private float RotateDirection = -1f; //just a variable to store clockwise/ counter clockwise
	public float RotateSpeed; //how fast it will rotate when changing
	public float Rotationaloffset; // an offset of it's rotation ...180 to flip upside down

	[Header("Ammo variables")]
	public float FadeSpeed; // how fast the colors change

    [SerializeField]
    private GameObject weapon;
    
	//these variables are set by the gun or w/e is shooting
	[HideInInspector]
	public int Count = 12;  
	[HideInInspector]
	public int MaxCount = 12;
    
    private int ammo;
    private AmmoManager ammoManager;

	//stores each image
	private List<Image> Images =  new List<Image>(); 

	// setup everything
	void Start () 
	{
        ammoManager = weapon.GetComponent<AmmoManager>();
        ammo = ammoManager.AmmoCount();
        MaxCount = ammo;
        Count = ammo;

		for (int i = 0; i < Count ; i ++ )
		{
			GameObject ThisIndicator = Instantiate(IndicatorGameObject,gameObject.transform.position, Quaternion.Euler(0f, 0f, ((360f/(float)MaxCount) * i))) as GameObject;
			ThisIndicator.transform.parent = gameObject.transform;
			ThisIndicator.transform.localScale = new Vector3(1f,1f,1f);

			Images.Add (gameObject.transform.GetChild(i).gameObject.GetComponent<Image>());
		}


		if (CounterClockWise)
		{
			RotateDirection = -1f;
		}
		else
		{
			RotateDirection = 1f;
			Images.Reverse();
		}
	
	}

    // Update the button...yay!
    void FixedUpdate () 
	{
        ammo = ammoManager.AmmoCount();
        Count = ammo;
        if (Count > 0)
		{

			for(int i = 0; i < Images.Count; i++)
			{

				if (i <= Count - 1)
				{
					Images[i].color = Color.Lerp (Images[i].color,IndicatorColor1,FadeSpeed * Time.deltaTime);
				}
				else
				{
					Images[i].color = Color.Lerp (Images[i].color,IndicatorColor2,FadeSpeed * Time.deltaTime);
				}

			}

            /*
			if (CounterClockWise)
			{
				gameObject.transform.localRotation = Quaternion.Lerp (gameObject.transform.localRotation, Quaternion.Euler(new Vector3(0f, 0f,(((RotateDirection * 360f) / (float)MaxCount) * ((float)Count - 1f) + Rotationaloffset))), RotateSpeed * Time.deltaTime);
			}
			else
			{
				gameObject.transform.localRotation = Quaternion.Lerp (gameObject.transform.localRotation, Quaternion.Euler(new Vector3(0f, 0f, (((RotateDirection * 360f) / (float)MaxCount) * ((float)Count) + Rotationaloffset))), RotateSpeed * Time.deltaTime);
			}*/

		}
		else if (Count == 0)
		{

			for(int i = 0; i < Images.Count; i++)
			{
				Images[i].color = Color.Lerp (Images[i].color,IndicatorColor2,FadeSpeed * Time.deltaTime);
			}

			
			//gameObject.transform.localRotation = Quaternion.Lerp (gameObject.transform.localRotation, Quaternion.Euler(new Vector3(0f + Rotationaloffset, 0f, 0f)), RotateSpeed * Time.deltaTime);
		}

		
	}

	//change the ammo
	public void ChangeAmmo(int Ammo)
	{
		Count  = Ammo;

		Count = Mathf.Clamp(Count,0,MaxCount);
	}

	//override!
	public void ChangeAmmo()
	{
		Count  = Count -1;
		
		Count = Mathf.Clamp(Count,0,MaxCount);
	}
	

}
