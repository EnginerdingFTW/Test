using UnityEngine;
using System.Collections;

public class LaserMover : WeaponFire {

	public float laserSpeed = 10.0f; 								//the speed at which the laser shoots out
	public int damage = 10;
	public bool epic = false;										//if laser is epic, it isn't destroyed after hitting a player

	/// <summary>
	/// Immediately set the laser to move forward at whatever rate given. Each prefab can set this rate differently.
	/// Additionally starts a despawn timer in case it stays on screen too long?
	/// </summary>
	void Start () {
		GetComponent<Rigidbody2D> ().velocity = transform.up * -laserSpeed;
		
	}
		
	/// <summary>
	/// Destroy the laser on impact, apply damage if necessary.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other)	
	{
		if (hasShot && other.tag == "Player" && other.gameObject != shootingPlayer) {
			other.gameObject.GetComponent<Player>().CreateExplosionAnimation(this.transform.position, ((float) damage) / 10);

			other.gameObject.GetComponent<Player> ().Hurt (damage, shootingPlayer.GetComponent<Player>().playerNum);	//apply damage
			if (!epic) {
				Destroy (this.gameObject);
			}
		}
	}
}