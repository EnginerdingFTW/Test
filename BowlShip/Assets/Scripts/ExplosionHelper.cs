using UnityEngine;
using System.Collections;

public class ExplosionHelper : MonoBehaviour {
		//this script damages anything within a collider, used for explosion radius's


	public bool exploded = false;
	public int damage;

	// Update is called once per frame
	void OnTriggerStay2D(Collider2D other)
	{
		if (exploded && other.tag == "Player")
		{
			other.GetComponent<Player>().Hurt(damage);
		}
	}
}
