using UnityEngine;
using System.Collections;

public class Weapon : Collectable {

	public float timer = 5.0f;			//if isTimer is false, this is the quantity of bullets of the weapon, else it is a timer
	public bool isTimer = false;		//^
	public float fireRate;				//how fast this weapon can fire (Set in each individual weapon)	

/// <summary>
/// if the player collides with this object, the player picks up this object.
/// picking up entails making this object no longer collidable or visible
/// </summary>
/// <param name="other">Other collider</param>
	void OnTriggerEnter2D(Collider2D other)		
	{
		if (other.tag == "Player")
		{
			this.pickedUp = true;
			other.GetComponent<Player>().weapons.Add(this);
			this.GetComponent<Collider2D>().enabled = false;
			this.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}
