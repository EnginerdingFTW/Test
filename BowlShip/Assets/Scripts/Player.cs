using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public int playerNum;								//The player controlling this player
	public int health = 100;							//How much health the ship has left (permanent damage)
	public int shield = 20;								//How much shields the ship has (possible to recharge with powerups)
	public int startingHealth = 100;					//The typical max health, unless given a powerup
	public int maxShield = 20;							//The maximum amount of shields a ship can have
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
	[HideInInspector] public static string fireButton = "Fire";		//The string used to allow the ship to fire, changable to allow one or two player options per controller
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
	public GameObject explosion;						//Explosion to instantiate when player dies

	//Boosting
	public bool canBoost = true;						//The player can use up it's shield to boost if it has more than half its shield left
	public float boostMultiplier = 2.5f;				//How much faster does the boost make the ship go
	public float boostTime = 1.0f;						//The length the boost is in effect
	public int minShieldForBoost = 10;					//How much shield the player must have to boost
	public float originalScale;							//The original y scale proportion of the ship
	private bool playerInputBoost = false;				//Used for A.I. boost input
	public float boostScaleChange = 1.1f;				//How much longer the ship looks while boosting (same animation for multiple scaled ships)

	//Drifting
	public float thrust = 1.0f;							//The trigger movement input
	public static bool useShieldRecharge = true;		//Allows us to experiment with shield recharge based on movement
	public int shieldCharge = 1;						//How much the shield charges each cycle
	public float shieldChargeRate = 1.0f;				//How fast the shield will recharge
	public int speedShieldChargeAdjustment = 1;			//Used to change how fast the shield recharges based on speed
	public bool canRecharge = true;						//Stops too many threads being instantiated with shieldRecharge IENumerator

	//Audio
	public AudioClip damaged;							//The sound clip to be played when the ship takes damage
	private AudioSource audioSource;					//The audioSource used to play our soundclips
	public float volume = 0.5f;							//How loud all SFX from this script is

	//Animations
	private Animator animator;							//The animator attached to the player

	//HUD
	public Sprite shipIcon;								//the ship to be assigned to the playerIcon
	public Sprite defaultWeaponIcon;					//the default box to be assigned to the weaponIcon
	public Color fullHealthC;							//The color of a full health bar
	public int highHealthThreshhold = 60;				//The value of health when the health color changes to yellow
	public Color midHealthC;							//The color of a mid health bar
	public int midHealthThreshhold = 25;				//The value of health when the health color changes to red
	public Color lowHealthC;							//The color of a low health bar
	public Slider shieldSlider;							//The HUD showing the amount of shield left on the player
	public Slider healthSlider;							//The HUD showing the amount of health left on the player
	public GameObject playerIcon;						//The Hud showing the player's ship, to differentiate who is who
	public GameObject weaponIcon;						//the HUD showing which weapon the player currently has
	public GameObject chargingIcon;						//the HUD showing if the shield is recharging or if the ship is stunned
	private Animator chargingIconAnimator;				//the animator attached to the chargingIcon;
	[HideInInspector] public Image weaponIconImage;		//the image of the weaponIcon gameObject

	private GameController gc;							//The Match's logic center
	public Weapon currentWeapon;		//The current Weapon the wielder has
	private GameObject laserObject;						//The shot object that the Player shoots out, used to make sure Player can't shoot themselves
	private float horiz;								//The horizontal movement input
	private float vert;									//The vertical movement input
	private Vector2 movement;							//the total movement of the player
	private Rigidbody2D rb;								//The ship's Rigidbody component
	private PointEffector2D pe;							//Used to know the ship and other objects back upon collision
	private CircleCollider2D cc;						//This ship's Larger circleCollider trigger for use with pe

	//AI
	public EnemyAI enemyAI;								//Used for computer players

	/// <summary>
	/// Initialize
	/// </summary>
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		pe = GetComponent<PointEffector2D> ();
		cc = GetComponent<CircleCollider2D> ();
		animator = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
		if (GameObject.FindGameObjectWithTag ("GameController") != null) {
			gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		}

		//checking for AI
		enemyAI = this.gameObject.GetComponent<EnemyAI>();
	}
	
	/// <summary>
	/// Sets the Movement of the Player, allows it to fire, drift, and boost.
	/// </summary>
	void Update () {
		if (poweredOn) {
			//Linear Movement
			if (enemyAI != null)
			{
				horiz = enemyAI.horizontal;
				vert = enemyAI.vertical;
			}
			else
			{
				horiz = Input.GetAxis ("Horizontal" + playerNum.ToString ()) * speed;
				vert = Input.GetAxis ("Vertical" + playerNum.ToString ()) * speed;
			}

			//Angular Movement
			if (Mathf.Abs (horiz) > minInput || Mathf.Abs (vert) > minInput) {
				float angle = Mathf.Atan2 (vert, horiz) * Mathf.Rad2Deg + 90;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis (angle, Vector3.forward), Time.deltaTime * rotationSpeed);
			} 

			//New Thrust Adjustment (drift)
			if (enemyAI != null)
			{
				thrust = enemyAI.drift;
			} 
			else 
			{
				thrust = Input.GetAxis ("Thrust" + playerNum.ToString ());			
			}
	
			if (thrust <= 0) 
			{
				man0Drag = 1;
				man1Drag = 1;
				man2Drag = 1;
				movement = new Vector2 (horiz, vert);
				rb.AddForce (movement);
			} 
			else 
			{
				man0Drag = 0;
				man1Drag = 0;
				man2Drag = 0;
			}

			//Shield Recharge Adjustment
			if (useShieldRecharge && canRecharge && canBoost) {
				StartCoroutine ("ShieldRecharge", shieldChargeRate);
			}

			//Boosting
			if (canBoost && thrust <= 0 && shield > minShieldForBoost) {
				playerInputBoost = false;
				if (enemyAI != null)
				{
					playerInputBoost = enemyAI.boost;
				}
				else if (Input.GetAxis ("Boost" + playerNum.ToString ()) > 0.3f)
				{
					playerInputBoost = true;
				}
				if (playerInputBoost) {
					canBoost = false;
					StartCoroutine("Boost");
				}
			}

			//Shooting
			if (canFire && ((Input.GetAxis (fireButton + playerNum.ToString ()) > 0.3f) || (enemyAI != null && enemyAI.fire))) 
			{
				canFire = false;

				//default weapon
				if (weapons.Count == 0) 
				{
					fireRate = defaultFireRate;
					laserObject = (GameObject)Instantiate (defaultLaser, laserInstatiationPoint.transform.position, transform.rotation);
					weaponIconImage.sprite = defaultWeaponIcon;
					laserObject.GetComponent<WeaponFire> ().AttachPlayer (this.gameObject);
					StartCoroutine ("RegulateWeaponFire");
				} 
				else 
				{
					//power up weapon
					currentWeapon = weapons [weapons.Count - 1];
					weaponIconImage.sprite = currentWeapon.GetComponent<SpriteRenderer> ().sprite;
					fireRate = currentWeapon.fireRate;
					if (currentWeapon.isDual) {
						laserObject = (GameObject)Instantiate (currentWeapon.laserType, dualLaserInstatiationPoint1.transform.position, transform.rotation);
						if (laserObject.GetComponent<WeaponFire> () == null) {
							laserObject.GetComponentInChildren<WeaponFire> ().AttachPlayer (this.gameObject);
						} else {
							laserObject.GetComponent<WeaponFire> ().AttachPlayer (this.gameObject);
						}
						laserObject = (GameObject)Instantiate (currentWeapon.laserType, dualLaserInstatiationPoint2.transform.position, transform.rotation);
						if (laserObject.GetComponent<WeaponFire> () == null) {
							laserObject.GetComponentInChildren<WeaponFire> ().AttachPlayer (this.gameObject);
						} else {
							laserObject.GetComponent<WeaponFire> ().AttachPlayer (this.gameObject);
						}
					} else {
						laserObject = (GameObject)Instantiate (currentWeapon.laserType, laserInstatiationPoint.transform.position, transform.rotation);
						if (laserObject.GetComponent<WeaponFire> () == null) {
							laserObject.GetComponentInChildren<WeaponFire> ().AttachPlayer (this.gameObject);
						} else {
							laserObject.GetComponent<WeaponFire> ().AttachPlayer (this.gameObject);
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

		//Pause Menu
		if (gc != null && Input.GetButton ("Pause") && gc.paused == false) {
			gc.Pause ();
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
		if (canBoost) {
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
	}

	/// <summary>
	/// Boost the ship by increasing its speed for a set time. While boosting the ship loses its shield.
	/// </summary>
	IEnumerator Boost () {
		canBoost = false;
		transform.localScale = new Vector3(originalScale, transform.localScale.y * boostScaleChange, 1);
		shield = 0;
		if (gc != null && gc.paused == false) {
			shieldSlider.value = shield;
		}
		speed *= boostMultiplier;
		yield return new WaitForSeconds (boostTime);
		transform.localScale = new Vector3 (originalScale, originalScale, 1);
		canBoost = true;
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
		if (!defeated) {
			audioSource.PlayOneShot (damaged, volume);
		}
		if (shield <= 0) {
			shieldSlider.value = 0;
			health += shield;
			healthSlider.value = health;

			//change healthbar colors
			if (health > highHealthThreshhold && health <= startingHealth) {
				healthSlider.GetComponentsInChildren<Image> () [1].color = fullHealthC;
			} else if (health > midHealthThreshhold && health <= highHealthThreshhold) {
				healthSlider.GetComponentsInChildren<Image> () [1].color = midHealthC;
			} else if (health <= midHealthThreshhold) {
				healthSlider.GetComponentsInChildren<Image> () [1].color = lowHealthC;
			}
			if (health < 1) {
				healthSlider.value = 0;
				healthSlider.GetComponentsInChildren<Image> () [1].color = fullHealthC;
				defeated = true;
				if (gc == null) {
					health = 100;
					shield = maxShield;
					healthSlider.value = health;
					shieldSlider.value = shield;
				} else {
					//Debug.Log ("Player" + playerNum.ToString() + "Just Died");
					gc.CheckEnd (playerNum, -1);
					canBoost = true;
					transform.localScale = new Vector3 (originalScale, originalScale, 1);
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
	/// Hurt the specified damage. Affects shields first, then any remaining damage is done to health. Destroys the player if health drops below 1.
	/// It will play either a shield damaged animation or a player damaged and/or destroyed animation.
	/// This method is overloaded, allows weaponFire to use their "player that shot" variable to pass information along to player then game controller for scoring.
	/// If the gameController is null then must be in menu, thus revive player fully.
	/// </summary>
	/// <param name="damage">The amount of damge to be dealt to the player.</param>
	/// <param name="playerThatShotYou">The other player that fired the weaponFire to Hurt this player.</param>
	public void Hurt (int damage, int playerThatShotYou) {
		shield -= damage;
		shieldSlider.value = shield;
		audioSource.PlayOneShot (damaged, volume);
		if (shield <= 0) {
			shieldSlider.value = 0;
			health += shield;
			healthSlider.value = health;

			//change healthbar colors
			if (health > highHealthThreshhold && health <= startingHealth) {
				healthSlider.GetComponentsInChildren<Image> () [1].color = fullHealthC;
			} else if (health > midHealthThreshhold && health <= highHealthThreshhold) {
				healthSlider.GetComponentsInChildren<Image> () [1].color = midHealthC;
			} else if (health <= midHealthThreshhold) {
				healthSlider.GetComponentsInChildren<Image> () [1].color = lowHealthC;
			}
			if (health < 1) {
				healthSlider.value = 0;
				healthSlider.GetComponentsInChildren<Image> () [1].color = fullHealthC;
				defeated = true;
				if (gc == null) {
					health = 100;
					shield = maxShield;
					healthSlider.value = health;
					shieldSlider.value = shield;
				} else {
					gc.CheckEnd (playerNum, playerThatShotYou);
					Debug.Log ("Player " + playerNum.ToString () + " Died");
					GameObject explodedAnim = (GameObject)Instantiate (explosion, transform.position, transform.rotation);
					explodedAnim.transform.localScale = new Vector3 (1f, 1f, 0);
					canBoost = true;
					transform.localScale = new Vector3 (originalScale, originalScale, 1);
					gameObject.SetActive (false); 
				}
				return;
			} else {
				animator.SetTrigger ("HealthHurt");
			}
			shield = 0;
			return;
		} else {
			animator.SetTrigger ("ShieldHurt");
		}
	}

	/// <summary>
	/// Check if the player collides with anything, hurt the player if the collision is another player, and bounce the player off of the collision. 
	/// </summary>
	/// <param name="coll">The Collision against the player.</param>
	void OnCollisionEnter2D (Collision2D coll) {
		if (coll.gameObject.CompareTag ("Player")) { //Change damage based on speed?
			Hurt (playerCollisionDamage, coll.gameObject.GetComponent<Player>().playerNum);
			foreach (ContactPoint2D loc in coll.contacts)
			{
				CreateExplosionAnimation(new Vector3(loc.point.x, loc.point.y, 0), 1.0f);
			}
		}
		//provide force between player and object
		cc.enabled = true;
		pe.enabled = true;
		if (gameObject.activeSelf) {
			StartCoroutine ("RegulateCollisionForce");				//bug here for some reason
		}
	}
		

	/// <summary>
	/// Regulates the weapon fire. Will change a boolean after the fireRate is finished
	/// </summary>
	/// <returns>The time between firing.</returns>
	IEnumerator RegulateWeaponFire () {
		//Debug.Log("can fire reseting1");
		yield return new WaitForSeconds (1.0f / fireRate);
		canFire = true;
		//Debug.Log("can fire reseting2");
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
	/// freezes the player for the set amount of time, called by StunBolt and EMP
	/// </summary>
	/// <param name="stunTime">Stun time.</param>
	public IEnumerator Stunned (float stunTime) {
		animator.SetBool ("Stunned", true);
		chargingIconAnimator.SetBool ("Stunned", true);
		yield return new WaitForSeconds (stunTime);
		chargingIconAnimator.SetBool ("Stunned", false);
		animator.SetBool ("Stunned", false);
		poweredOn = true;
	}

	public IEnumerator ShieldRecharge (float rechargeRate) {
		canRecharge = false;
		shield += shieldCharge * (int)(rb.velocity.magnitude / speedShieldChargeAdjustment);
		if (shield >= maxShield) {
			shield = maxShield;
		} else {
			if (gc != null && gc.paused == false) {
				chargingIconAnimator.SetBool ("Charging", true);
			}
		}
		if (gc != null && gc.paused == false) {
			shieldSlider.value = shield;
		}
		yield return new WaitForSeconds (rechargeRate);
		if (gc != null && gc.paused == false) {
			chargingIconAnimator.SetBool ("Charging", false);
		}
		canRecharge = true;
	}

	/// <summary>
	/// Resets the ships ability to boost and to recharge.
	/// </summary>
	void OnDisable () {
		canRecharge = true;
		canBoost = true;
	}

	/// <summary>
	/// Assigns the HUD. This function is utilized by the GameController to assign each player their HUD before the game starts.
	/// </summary>
	/// <param name="healthS">Health s.</param>
	/// <param name="shieldS">Shield s.</param>
	/// <param name="chargingIcon">Charging icon.</param>
	/// <param name="weaponIcon">Weapon icon.</param>
	/// <param name="playerIcon">Player icon.</param>
	public void AssignHUD (Slider healthS, Slider shieldS, GameObject chargingIcon, GameObject weaponIcon, GameObject playerIcon) {
		this.healthSlider = healthS;
		this.shieldSlider = shieldS;
		this.weaponIcon = weaponIcon;
		weaponIconImage = this.weaponIcon.GetComponent<Image> ();
		this.playerIcon = playerIcon;
		this.playerIcon.GetComponentsInChildren<Image> ()[1].sprite = shipIcon;
		this.chargingIcon = chargingIcon;
		chargingIconAnimator = this.chargingIcon.GetComponent<Animator> ();
	}

	/// <summary>
	/// Toggles the use of shield recharging, used by options menu
	/// </summary>
	public static void toggleShieldRecharge () {
		useShieldRecharge = !useShieldRecharge;
	}

	public void CreateExplosionAnimation(Vector3 point, float damageRatio)
	{
		Vector3 tempPos = new Vector3(point.x, point.y, 0);
		Vector3 tempRot = new Vector3(0, 0, Random.Range(0, 360));
		GameObject temp = Instantiate(explosion, tempPos, Quaternion.EulerAngles(tempRot)) as GameObject;
		temp.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f) * damageRatio;
		temp.GetComponent<AudioSource>().volume = 0.1f;
	}
}
