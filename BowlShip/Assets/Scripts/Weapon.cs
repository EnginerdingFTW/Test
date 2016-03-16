using UnityEngine;
using System.Collections;

public class Weapon : Collectible {

	public float timer = 5.0f;			//if isTimer is false, this is the quantity of bullets of the weapon, else it is a timer
	public bool isTimer = false;		//^
	

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			this.pickedUp = true;
			//other.GetComponent<Player>();
		}
	}
}
