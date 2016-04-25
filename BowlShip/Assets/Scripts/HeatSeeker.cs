﻿using UnityEngine;
using System.Collections;

public class HeatSeeker : WeaponFire {

	public float rocketSpeed = 10.0f; 				//the speed at which the laser shoots out
	public GameObject[] players;					//All the other players playing
	public int damage = 10;							//How much damage the rocket does to the player
	public float turnSpeed = 2.0f;						//How fast the rocket turns towards the player

	private Transform potentialTarget;				//A possible Player to follow
	private Transform target;						//The target that's locked on
	private Rigidbody2D rb;							//The attached Rigidbody used to set velocity

	/// <summary>
	/// Immediately set the laser to move forward at whatever rate given. Each prefab can set this rate differently.
	/// Additionally starts a despawn timer in case it stays on screen too long?
	/// </summary>
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = transform.up * rocketSpeed;
		transform.Rotate(new Vector3(0, 0, 180));
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
//				targetRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
//				transform.rotation = Quaternion.Slerp(Quaternion.Euler(transform.rotation.z, 0, 0), Quaternion.Euler(targetRotation.z, 0, 0), Time.deltaTime * 2.0f);    
				Vector3 vectorToTarget = target.position - transform.position;
				float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
				Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
				transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turnSpeed);
			}
			rb.velocity = transform.up * rocketSpeed;
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
			Destroy(this.gameObject);
		}
	}
}