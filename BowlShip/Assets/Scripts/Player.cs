using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public int playerNum;								//The player controlling this player
	public int health = 100;							//How much health the ship has left (permanent damage)
	public int shield = 100;							//How much shields the ship has (possible to recharge with powerups)
	public float speed = 4.0f;							//How fast the ship can accelerate
	public float rotationSpeed = 5.0f;					//How fast the ship can rotate
	public int man = 1;									//Maneuverability of the ship
	public float fireRate = 2;							//How fast the ship can fire (1s / firerate between shots)
	public float defaultFireRate = 2;					//The basic weapon's fire rate
	public float invTime = 0.5f;						//Shouldn't need this, invincibility after being hit
	public List<Weapon> weapons;						//A list of collected weapons

	private Weapon currentWeapon;						//The current Weapon the wielder has
	private float horiz;								//The horizontal movement input
	private float vert;									//The vertical movement input
	private bool canFire = true;						//boolean to restrict fireRate of ship
	private Vector2 movement;							//the total movement of the player
	private Rigidbody2D rb;								//The ship's Rigidbody component

	/// <summary>
	/// Initialize
	/// </summary>
	void Start () {
		playerNum = 1;
		rb = GetComponent<Rigidbody2D> ();
	}
	
	/// <summary>
	/// Sets the Movement of the Player, allows it to fire.
	/// </summary>
	void Update () {

		//Linear Movement
		horiz = Input.GetAxis ("Horizontal" + playerNum.ToString()) * speed;
		vert = Input.GetAxis ("Vertical" + playerNum.ToString()) * speed;
		movement = new Vector2(horiz, vert);
		rb.AddForce(movement);

		//Angular Movement
		if(horiz != 0 || vert != 0)
		{
			float angle = Mathf.Atan2(vert, horiz) * Mathf.Rad2Deg + 90;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis (angle, Vector3.forward), Time.deltaTime * rotationSpeed);
		} 

		//Shooting
		if (canFire && Input.GetButton("Fire" + playerNum.ToString())) {
			canFire = false;

			//default weapon

			if (weapons.Count == 0) {
				fireRate = defaultFireRate;
				//instantiate default laser prefab
				StartCoroutine ("RegulateWeaponFire");
			} else {
				
				//power up weapon
			
				currentWeapon = weapons [weapons.Count - 1];
				fireRate = currentWeapon.fireRate;
				//instantiate weapon's laser
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

	/// <summary>
	/// Hurt the specified damage. Affects shields first, then any remaining damage is done to health. Destroys the player if health drops below 1.
	/// It will play either a shield damaged animation or a player damaged and/or destroyed animation.
	/// </summary>
	/// <param name="damage">The amount of damge to be dealt to the player.</param>
	public void Hurt (int damage) {
		shield -= damage;
		if (shield < 0) {
			health += shield;
			if (health < 1) {
				//Player Destroyed, destroyed animation
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
	/// Regulates the weapon fire. Will change a boolean after the fireRate is finished
	/// </summary>
	/// <returns>The time between firing.</returns>
	IEnumerator RegulateWeaponFire () {
		yield return new WaitForSeconds (1.0f / fireRate);
		canFire = true;
	}
}
