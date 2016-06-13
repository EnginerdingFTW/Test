using UnityEngine;
using System.Collections;

public class StunBolt : WeaponFire {

	public float laserSpeed = 10.0f; 					//the speed at which the laser shoots out
	public float timeStunned = 3.0f;
	public int damage = 10;
	

	/// <summary>
	/// Immediately set the laser to move forward at whatever rate given. Each prefab can set this rate differently.
	/// Additionally starts a despawn timer in case it stays on screen too long?
	/// </summary>
	void Start () {
		GetComponent<Rigidbody2D> ().velocity = transform.up * -laserSpeed;
	}

	void OnTriggerEnter2D(Collider2D other)	
	{
		if (hasShot && other.tag == "Player" && other.gameObject != shootingPlayer)
		{
					//Debug.Log("in");
			other.gameObject.GetComponent<Player>().Hurt(damage, shootingPlayer.GetComponent<Player>().playerNum);
			Player temp = other.GetComponent<Player>();
				//only stun the player if they aren't already stunned
			if (temp.poweredOn)
			{
				//freezes the pass player for the set amount of time
				temp.poweredOn = false;
				temp.StartCoroutine("Stunned", timeStunned);
			}	
			Destroy (this);
		}	
	}
}
