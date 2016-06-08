using UnityEngine;
using System.Collections;

public class ExplosionHelper : MonoBehaviour {
		//this script damages anything within a collider, used for explosion radius's


	public bool exploded = false;
	public int damage;

	/// <summary>
	/// Sets the bool exploded to true and activates the proper collider
	/// </summary>
	public void Explode () {
		exploded = true;
		GetComponent<CircleCollider2D> ().enabled = true;
	}

	// Update is called once per frame
	void OnTriggerStay2D(Collider2D other)
	{
		if (exploded && other.tag == "Player")
		{
			other.GetComponent<Player>().Hurt(damage);
		}
	}
}
