﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public int playerNum;								//The player controlling this player
	public int health = 100;							//How much health the ship has left (permanent damage)
	public int shield = 100;							//How much shields the ship has (possible to recharge with powerups)
	public int maxShield = 100;							//The maximum amount of shields a ship can have
	public float speed = 4.0f;							//How fast the ship can accelerate
//	public float maxVelocity = 10.0f;					//The topSpeed of the ship, shouldn't need to use this
	public float rotationSpeed = 5.0f;					//How fast the ship can rotate

	public int man = 0;									//Maneuverability value of the ship
	public float man0Drag = 0.0f;						//The default maneuverability drag
	public float man0Speed = 4.0f;						//The default meneuverability speed
	public float man0Rotation = 2.0f;					//How quickly the ship turns
	public float man1Drag = 1.0f;						//The upgraded maneuverability drag
	public float man1Speed = 10.0f;						//The upgraded meneuverability speed
	public float man1Rotation = 6.0f;					//How quickly the ship turns
	public float man2Drag = 5.0f;						//The max maneuverability drag
	public float man2Speed = 100.0f;					//The max meneuverability speed
	public float man2Rotation = 10.0f;					//How quickly the ship turns
	public int maxMan = 2;								//The maximum maneuverability value of the ship

	public bool  poweredOn = true;						//can the ship move, rotate and fire?
	public bool canFire = true;							//boolean to restrict fireRate of ship
	public float fireRate = 2;							//How fast the ship can fire (1s / firerate between shots)
	public float defaultFireRate = 2;					//The basic weapon's fire rate
	public int playerCollisionDamage = 10;				//The amount of damage done to THIS ship after hitting another player
	public float forceTime = 0.5f;						//How fast the player and collider is forced back after hitting an object
	public float invTime = 0.5f;						//Shouldn't need this, invincibility after being hit
	public bool defeated = false;						//Was the ship destroyed?
	public List<Weapon> weapons;						//A list of collected weapons
	public GameObject defaultLaser;						//The default laser weapon for the ship
	public GameObject laserInstatiationPoint;			//Where the laser is fired from, in comparison to the player origin
	public GameObject dualLaserInstatiationPoint1;		//Where the left laser is fired from
	public GameObject dualLaserInstatiationPoint2;		//Where the right laser is fired from

	//Audio
	public AudioClip damaged;							//The sound clip to be played when the ship takes damage
	private AudioSource audioSource;					//The audioSource used to play our soundclips

	//HUD
	public Slider shieldSlider;							//The HUD showing the amount of shield left on the player
	public Slider healthSlider;							//The HUD showing the amount of health left on the player

	private Weapon currentWeapon;						//The current Weapon the wielder has
	private float horiz;								//The horizontal movement input
	private float vert;									//The vertical movement input
	private Vector2 movement;							//the total movement of the player
	private Rigidbody2D rb;								//The ship's Rigidbody component
	private PointEffector2D pe;							//Used to know the ship and other objects back upon collision
	private CircleCollider2D cc;						//This ship's Larger circleCollider trigger for use with pe
	private GameController gc;							//The Match's logic center

	/// <summary>
	/// Initialize
	/// </summary>
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		pe = GetComponent<PointEffector2D> ();
		cc = GetComponent<CircleCollider2D> ();
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		audioSource = GetComponent<AudioSource> ();
	}
	
	/// <summary>
	/// Sets the Movement of the Player, allows it to fire.
	/// </summary>
	void Update () {

		if (poweredOn) {
			
			//Linear Movement
			horiz = Input.GetAxis ("Horizontal" + playerNum.ToString ()) * speed;
			vert = Input.GetAxis ("Vertical" + playerNum.ToString ()) * speed;
			movement = new Vector2 (horiz, vert);
			rb.AddForce (movement);

			//Angular Movement
			if (horiz != 0 || vert != 0) {
				float angle = Mathf.Atan2 (vert, horiz) * Mathf.Rad2Deg + 90;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis (angle, Vector3.forward), Time.deltaTime * rotationSpeed);
			} 

			//Shooting
			if (canFire && Input.GetButton ("Fire" + playerNum.ToString ())) {
				canFire = false;

				//default weapon
				if (weapons.Count == 0) {
					fireRate = defaultFireRate;
					Instantiate (defaultLaser, laserInstatiationPoint.transform.position, transform.rotation);
					StartCoroutine ("RegulateWeaponFire");
				} else {
				
					//power up weapon
					currentWeapon = weapons [weapons.Count - 1];
					fireRate = currentWeapon.fireRate;
					if (currentWeapon.isDual) {
						Instantiate (currentWeapon.laserType, dualLaserInstatiationPoint1.transform.position, transform.rotation);
						Instantiate (currentWeapon.laserType, dualLaserInstatiationPoint2.transform.position, transform.rotation);
					} else {
						Instantiate (currentWeapon.laserType, laserInstatiationPoint.transform.position, transform.rotation);
					}
					StartCoroutine ("RegulateWeaponFire");
					if (!currentWeapon.isTimer) {
						currentWeapon.timer--;
					}
					if (currentWeapon.timer <= 0.0f) {
						weapons.Remove (currentWeapon);		//destroy weapon if it runs out of ammo
					}
				}
			}
		}

		//Maintain MaxValues
		if (shield > maxShield) {
			shield = maxShield;
		}
		//		if (rb.velocity.magnitude > maxVelocity) {
		//			//might want to set this, should already be done by linear drag
		//		}
		if (man > maxMan) {
			man = maxMan;
		}
		if (man < 0) {
			man = 0;
		}

		//Apply Maneuverability
		switch (man) {
		case 1:
			speed = man1Speed;
			rb.drag = man1Drag;
			rb.angularDrag = man1Rotation;
			break;
		case 2:
			speed = man2Speed;
			rb.drag = man2Drag;
			rb.angularDrag = man2Rotation;
			break;
		default:
			speed = man0Speed;
			rb.drag = man0Drag;
			rb.angularDrag = man0Rotation;
			break;
		} 
	}

	/// <summary>
	/// Hurt the specified damage. Affects shields first, then any remaining damage is done to health. Destroys the player if health drops below 1.
	/// It will play either a shield damaged animation or a player damaged and/or destroyed animation.
	/// </summary>
	/// <param name="damage">The amount of damge to be dealt to the player.</param>
	public void Hurt (int damage) {
		shield -= damage;
		shieldSlider.value = shield;
		audioSource.PlayOneShot(damaged);
		if (shield <= 0) {
			shieldSlider.value = 0;
			health += shield;
			healthSlider.value = health;
			if (health < 1) {
				healthSlider.value = 0;
				defeated = true;
				gc.CheckEnd (playerNum);
				gameObject.SetActive (false); //put in animation?
				//destroyed animation
				return;
			} else {
				//Invincible for a bit?, damage animation
			}
			shield = 0;
			return;
		}
		//shield damage animation.
	}

	/// <summary>
	/// Check if the player collides with anything, hurt the player if the collision is another player, and bounce the player off of the collision.
	/// </summary>
	/// <param name="coll">The Collision against the player.</param>
	void OnCollisionEnter2D (Collision2D coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			Hurt (playerCollisionDamage);
		}
		//provide force between player and object
		cc.enabled = true;
		pe.enabled = true;
		StartCoroutine ("RegulateCollisionForce");
	}

	/// <summary>
	/// Regulates the weapon fire. Will change a boolean after the fireRate is finished
	/// </summary>
	/// <returns>The time between firing.</returns>
	IEnumerator RegulateWeaponFire () {
		yield return new WaitForSeconds (1.0f / fireRate);
		canFire = true;
	}

	/// <summary>
	/// Regulates the collision force.
	/// </summary>
	/// <returns>The time the point effector is active upon collision.</returns>
	IEnumerator RegulateCollisionForce () {
		yield return new WaitForSeconds (forceTime);
		pe.enabled = false;
		cc.enabled = false;
	}
}
