using UnityEngine;
using System.Collections;

public class HeatSeeker : WeaponFire {

	public float rocketSpeed = 10.0f; 				//the speed at which the laser shoots out
	public GameObject[] players;					//All the other players playing
	public int damage = 10;

	private Transform potentialTarget;				//A possible Player to follow
	private Transform target;						//The target that's locked on
	private Quaternion targetRotation;				//The rotation we want to turn towards
	private Rigidbody2D rb;							//The attached Rigidbody used to set velocity

	/// <summary>
	/// Immediately set the laser to move forward at whatever rate given. Each prefab can set this rate differently.
	/// Additionally starts a despawn timer in case it stays on screen too long?
	/// </summary>
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = transform.up * -rocketSpeed;
	}

	/// <summary>
	/// Always turns towards the closest enemy
	/// </summary>
	void Update ()
	{
		if (hasShot) {
			players = GameObject.FindGameObjectsWithTag ("Player");
			target = GetClosestEnemy (players);
			if (target != null) {
				// Smoothly rotates towards target 
				targetRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);    
			}
			rb.velocity = transform.up * -rocketSpeed;
		}
	}

	Transform GetClosestEnemy (GameObject[] enemies)
	{
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		for(int i = 0; i < enemies.Length; i++)
		{
			potentialTarget = enemies [i].transform;
			if (enemies [i] != shootingPlayer) {
				Vector3 directionToTarget = potentialTarget.position - currentPosition;
				float dSqrToTarget = directionToTarget.sqrMagnitude;
				if (dSqrToTarget < closestDistanceSqr) {
					closestDistanceSqr = dSqrToTarget;
					bestTarget = potentialTarget;
				}
			}
		}

		return bestTarget;
	}

	/// <summary>
	/// Upon colliding with another player, cause damage to them.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other)	
	{
		if (hasShot && other.gameObject != shootingPlayer && other.tag == "Player")
		{
			other.gameObject.GetComponent<Player>().Hurt(damage);	//apply damage
		}
	}
}
