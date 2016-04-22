using UnityEngine;
using System.Collections;

public class Nuke : WeaponFire {

	public GameObject explosion;
	public int damage;
	
		//instantiate giant explosion
	void Kaboom()
	{
		GameObject temp = Instantiate(explosion, this.transform.position, Quaternion.identity) as GameObject;
		temp.transform.localScale = temp.transform.localScale * 4;
	}

		//if the nuke collided with anything, explode, do damage (exploded to true), and destroy yourself
	void OnTriggerEnter2D(Collider2D other)	
	{
		if (hasShot && other.gameObject != shootingPlayer && (other.tag == "Player" || other.tag == "Asteroid"))
		{
			Kaboom();
			GetComponentInParent<ExplosionHelper>().exploded = true;
			Destroy(this.gameObject, 0.1f);
		}
	}
}
