using UnityEngine;
using System.Collections;

public class Weapon : Collectible {

	public float timer = 5.0f;
	public bool isTimer = false;
	

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			this.pickedUp = true;
		}
	}
}
