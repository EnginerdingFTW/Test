using UnityEngine;
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
	public float minInput = 0.2f;						//How far the joystick must be moved before the ship reacts

	public int man = 0;									//Maneuverability value of the ship
	public float man0Drag = 1.0f;						//The default maneuverability drag
	public float man0Speed = 10.0f;						//The default meneuverability speed
	public float man0Rotation = 6.0f;					//How quickly the ship turns
	public float man1Drag = 5.0f;						//The upgraded maneuverability drag
	public float man1Speed = 100.0f;					//The upgraded meneuverability speed
	public float man1Rotation = 10.0f;					//How quickly the ship turns
	public float man2Drag = 50.0f;						//The max maneuverability drag
	public float man2Speed = 1000.0f;					//The max meneuverability speed
	public float man2Rotation = 100.0f;					//How quickly the ship turns
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

	//May 23rd Thrust Update
	public float thrust = 1.0f;							//The trigger movement input
	public static bool useThrust = true;				//Allows us to experiment with and without 2nd trigger as gas pedal
	public static bool useShieldRecharge = true;		//Allows us to experiment with shield recharge based on movement
	public int shieldCharge = 1;						//How much the shield charges each cycle
	public float shieldChargeRate = 1.0f;				//How fast the shield will recharge
	public int speedShieldChargeAdjustment = 1;			//Used to change how fast the shield recharges based on speed
	public bool canRecharge = true;						//Stops too many threads being instantiated with shieldRecharge IENumerator

	//Audio
	public AudioClip damaged;							//The sound clip to be played when the ship takes damage
	private AudioSource audioSource;					//The audioSource used to play our soundclips
	public float volume = 0.5f;							//How loud all SFX from this script is

	//HUD
	public Slider shieldSlider;							//The HUD showing the amount of shield left on the player
	public Slider healthSlider;							//The HUD showing the amount of health left on the player

	private GameController gc;							//The Match's logic center
	private Weapon currentWeapon;						//The current Weapon the wielder has
	private GameObject laserObject;						//The shot object that the Player shoots out, used to make sure Player can't shoot themselves
	private float horiz;								//The horizontal movement input
	private float vert;									//The vertical movement input
	private Vector2 movement;							//the total movement of the player
	private Rigidbody2D rb;								//The ship's Rigidbody component
	private PointEffector2D pe;							//Used to know the ship and other objects back upon collision
	private CircleCollider2D cc;						//This ship's Larger circleCollider trigger for use with pe

	/// <summary>
	/// Initialize
	/// </summary>
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		pe = GetComponent<PointEffector2D> ();
		cc = GetComponent<CircleCollider2D> ();
		audioSource = GetComponent<AudioSource> ();
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}
	
	/// <summary>
	/// Sets the Movement of the Player, allows it to fire.
	/// </summary>
	void Update () {

		if (poweredOn) {
			
			//Linear Movement
			horiz = Input.GetAxis ("Horizontal" + playerNum.ToString ()) * speed;
			vert = Input.GetAxis ("Vertical" + playerNum.ToString ()) * speed;
			if (!useThrust) {
				movement = new Vector2 (horiz, vert);
				rb.AddForce (movement);
			}

			//Angular Movement
			if (Mathf.Abs(horiz) > minInput || Mathf.Abs(vert) > minInput) {
				float angle = Mathf.Atan2 (vert, horiz) * Mathf.Rad2Deg + 90;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis (angle, Vector3.forward), Time.deltaTime * rotationSpeed);
			} 

			//Thrust Adjustment
			if (useThrust) {
				thrust = Input.GetAxis ("Thrust" + playerNum.ToString ());
				if (thrust < 0) {
					thrust += 1;
				}
				if (thrust <= 0.1) {
					man0Drag = 0;
					man1Drag = 0;
					man2Drag = 0;
				} else {
					man0Drag = 1;
					man1Drag = 1;
					man2Drag = 1;
				}
				movement = new Vector2 (horiz * thrust, vert * thrust);
				rb.AddForce (movement);
			}

			//Shield Recharge Adjustment
			if (useShieldRecharge && canRecharge) {
				StartCoroutine ("ShieldRecharge", shieldChargeRate);
			}

			//Shooting
			if (canFire && Input.GetButton ("Fire" + playerNum.ToString ())) {
				canFire = false;

				//default weapon
				if (weapons.Count == 0) {
					fireRate = defaultFireRate;
					laserObject = (GameObject) Instantiate (defaultLaser, laserInstatiationPoint.transform.position, transform.rotation);
					laserObject.GetComponent<WeaponFire>().AttachPlayer (this.gameObject);
					StartCoroutine ("RegulateWeaponFire");
				} else {
				
					//power up weapon
					currentWeapon = weapons [weapons.Count - 1];
					fireRate = currentWeapon.fireRate;
					if (currentWeapon.isDual) {
						laserObject = (GameObject) Instantiate (currentWeapon.laserType, dualLaserInstatiationPoint1.transform.position, transform.rotation);
						if (laserObject.GetComponent<WeaponFire> () == null) {
							laserObject.GetComponentInChildren<WeaponFire> ().AttachPlayer (this.gameObject);
						} else {
							laserObject.GetComponent<WeaponFire>().AttachPlayer (this.gameObject);
						}
						laserObject = (GameObject) Instantiate (currentWeapon.laserType, dualLaserInstatiationPoint2.transform.position, transform.rotation);
						if (laserObject.GetComponent<WeaponFire> () == null) {
							laserObject.GetComponentInChildren<WeaponFire> ().AttachPlayer (this.gameObject);
						} else {
							laserObject.GetComponent<WeaponFire>().AttachPlayer (this.gameObject);
						}
					} else {
						laserObject = (GameObject) Instantiate (currentWeapon.laserType, laserInstatiationPoint.transform.position, transform.rotation);
						if (laserObject.GetComponent<WeaponFire> () == null) {
							laserObject.GetComponentInChildren<WeaponFire> ().AttachPlayer (this.gameObject);
						} else {
							laserObject.GetComponent<WeaponFire>().AttachPlayer (this.gameObject);
						}
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
	/// If the gameController is null then must be in menu, thus revive player fully.
	/// </summary>
	/// <param name="damage">The amount of damge to be dealt to the player.</param>
	public void Hurt (int damage) {
		shield -= damage;
		shieldSlider.value = shield;
		audioSource.PlayOneShot(damaged, volume);
		if (shield <= 0) {
			shieldSlider.value = 0;
			health += shield;
			healthSlider.value = health;
			if (health < 1) {
				healthSlider.value = 0;
				defeated = true;
				if (gc == null) {
					health = 100;
					shield = maxShield;
					healthSlider.value = health;
					shieldSlider.value = shield;
				} else {
					gc.CheckEnd (playerNum);
					gameObject.SetActive (false); //put in animation?
					//destroyed animation
				}
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
		if (coll.gameObject.activeSelf) {
			StartCoroutine ("RegulateCollisionForce");				//bug here for some reason
		}
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

	/// <summary>
	/// freezes the player for the set amount of time, called by StunBolt
	/// </summary>
	/// <param name="stunTime">Stun time.</param>
	public IEnumerator Stunned (float stunTime) {
		yield return new WaitForSeconds (stunTime);
		poweredOn = true;
	}

	public IEnumerator ShieldRecharge (float rechargeRate) {
		canRecharge = false;
		shield += shieldCharge * (int)(rb.velocity.magnitude / speedShieldChargeAdjustment);
		if (shield > maxShield) {
			shield = maxShield;
		}
		shieldSlider.value = shield;
		yield return new WaitForSeconds (rechargeRate);
		canRecharge = true;
	}

	/// <summary>
	/// Assigns the HUD. This function is utilized by the GameController to assign each player their HUD before the game starts.
	/// </summary>
	/// <param name="healthS">Health s.</param>
	/// <param name="shieldS">Shield s.</param>
	public void AssignHUD (Slider healthS, Slider shieldS) {
		this.healthSlider = healthS;
		this.shieldSlider = shieldS;
	}

	/// <summary>
	/// Toggles the use of thrust, used by options menu
	/// </summary>
	public static void toggleThrust () {
		useThrust = !useThrust;
	}

	/// <summary>
	/// Toggles the use of shield recharging, used by options menu
	/// </summary>
	public static void toggleShieldRecharge () {
		useShieldRecharge = !useShieldRecharge;
	}
}
