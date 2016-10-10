using UnityEngine;
using System.Collections;

public class HeatSeeker : WeaponFire {

	public float rocketSpeed = 10.0f; 				//the speed at which the laser shoots out
	public GameObject[] players;					//All the other players playing
	public int damage = 10;							//How much damage the rocket does to the player
	public float turnSpeed = 2.0f;					//How fast the rocket turns towards the player
	public float timeAlive = 8.0f;					//How long until the missile times out.

	private bool timesUp = false;					//False while the missile is targeting an enemy
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
		StartCoroutine ("Timeout", timeAlive);
	}

	/// <summary>
	/// Always turns towards the closest enemy
	/// </summary>
	void Update ()
	{
		if (!timesUp && hasShot) {
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
			other.gameObject.GetComponent<Player>().CreateExplosionAnimation(this.transform.position, ((float) damage) / 10);
			other.gameObject.GetComponent<Player>().Hurt(damage, shootingPlayer.GetComponent<Player>().playerNum);	//apply damage
			Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// As soon as the rocket launches, start a timer, once the times up the rocket no longer targets a player and flies straight.
	/// </summary>
	/// <param name="timeAlive">Time alive.</param>
	IEnumerator Timeout (float timeAlive) {
		yield return new WaitForSeconds (1.0f);
		timeAlive--;
		if (timeAlive <= 0.0f) {
			timesUp = true;
		} else {
			StartCoroutine ("Timeout", timeAlive);
		}
	}
}
