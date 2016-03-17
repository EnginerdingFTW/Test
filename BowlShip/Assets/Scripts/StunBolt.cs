﻿using UnityEngine;
using System.Collections;

public class StunBolt : MonoBehaviour {

	public float laserSpeed = 10.0f; 				//the speed at which the laser shoots out
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
		if (other.tag == "Player")
		{
			other.gameObject.GetComponent<Player>().Hurt(damage);
			Player temp = other.GetComponent<Player>();
			/*if (temp.poweredOn)
			{

			}
			*/
		}	
	}

	IEnumerator stun(float time, Player temp)
	{
		
		yield return new WaitForSeconds(time);
	}
}
