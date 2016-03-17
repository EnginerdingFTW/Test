using UnityEngine;
using System.Collections;

public class Nuke : MonoBehaviour {

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
		if (other.tag != "Boundary")
		{
			Kaboom();
			GetComponentInChildren<ExplosionHelper>().exploded = true;
			Destroy(this.transform.parent.gameObject, 0.1f);
		}
	}
}
