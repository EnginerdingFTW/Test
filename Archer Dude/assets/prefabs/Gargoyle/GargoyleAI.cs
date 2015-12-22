using UnityEngine;
using System.Collections;

public class GargoyleAI : MonoBehaviour {

	private Animator animator;				//Gargoyles animator
	private GameObject player;				//The Player, used to get location
	private int fireCount = 0;				//How much prep time needed before a fire attack
	private bool inBreak = false;			//Used for logic, gives player some break time in between attacks
	private float attackChoice;				//The rolled value for what attack it should be
	private bool attacking = false;			//Is the gargoyle in an Attack sequence?
	private Rigidbody2D rb;					//Gargoyles rigidbody2d
	private bool standby = false;			//Should the Gargoyle remain in the same spot on the screen?
	private int distanceFromPlayer = 7;		//The distance the gargoyle is from the player (not constantly updated, only when needed)
	
	public float startingYposition = 3.24f;	//The normal height of the Gargoyle
	public int startingXposition = 7;		//The normal X distance from the player
	public int health = 3;					//How many hits to the tail it takes to kill the Gargoyle
	public float clawAttackSpeed = 4.0f;	//How fast the Gargoyle sweeps across the screen in its Claw Attack
	public float breakTime = 4;				//How many seconds in between Attack sequences
	public float fireAttackChance = 0.5f;	//The chance for a fire attack, the remaining percent is the chance for a claw attack

	// Use this for initialization
	void Start () {
		this.animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("Player");
	}

	//All decisions are made here
	void Update () {
		//While on "Standby" The gargoyle remains in place on screen
		if (standby) {
			transform.position = new Vector3 (player.transform.position.x + startingXposition, transform.position.y, transform.position.z);
			//Otherwise it will move into that position
		} else if (!attacking) {
			distanceFromPlayer = (int) (transform.position.x - player.transform.position.x);
			rb.velocity = new Vector2 (0, 0);
			if (distanceFromPlayer < startingXposition) {
				standby = true;
				distanceFromPlayer = startingXposition;
				StartCoroutine("GivePlayerABreak");
			} 

		}
		//This will roll a random value to decide whether to use a fire attack or claw attack
		if (!inBreak && !attacking && standby) {
			attackChoice = Random.value;
			if (attackChoice < fireAttackChance) {
				attacking = true;
				IncreaseFireCount();
			}
			else if (attackChoice >= fireAttackChance) {
				attacking = true;
				SwoopBack();
			}
		}
	}

	//Give the player some time in between attacks
	IEnumerator GivePlayerABreak () {
		inBreak = true;
		attacking = false;
		yield return new WaitForSeconds (breakTime);
		inBreak = false;
	}

	//Calculate the distance from the player, move on after sweeps through in a claw attack
	IEnumerator AttackRun () {
		distanceFromPlayer = (int) (transform.position.x - player.transform.position.x);
		if (distanceFromPlayer < -20) {
			rb.velocity = new Vector2 (0, 0);
			transform.position = new Vector3 (player.transform.position.x + 25, startingYposition, transform.position.z);
			animator.SetBool ("Swoop", false);
			attacking = false;
		} else {
			rb.velocity = new Vector2 (-clawAttackSpeed, 0);
			yield return new WaitForSeconds(0.1f);
			StartCoroutine("AttackRun");
		}
	}

	//Used to destroy from Death animation
	void DestroyThis () {
		Destroy (this.gameObject);
	}

	//Used to flick the tail once in hurt animation
	void TurnHurtOff () {
		animator.SetBool ("Hurt", false);
	}

	//Used to reset fire count in animation
	void ResetFireCount () {
		animator.SetInteger ("Fire", 0);
		fireCount = 0;
	}

	//Used to increase fire count in animation, won't attack until its so high. i.e, the higher the value found in animator, the longer the prep before a fire attack
	void IncreaseFireCount () {
		fireCount++;
		animator.SetInteger ("Fire", fireCount);
	}

	//Used in animator to begin the Claw Attack sequence
	void SwoopBack () {
		animator.SetBool ("Swoop", true);
	}

	//Used in animation to continue the Claw Attack sequence
	void FinishedSwoop () {
		distanceFromPlayer = 25;
		transform.position = new Vector3 (player.transform.position.x + distanceFromPlayer, player.transform.position.y - 1f, transform.position.z);
		standby = false;
		StartCoroutine("AttackRun");
	}

	//Keeps track of the Gargoyle's Health, maybe make the Gargoyle also flash red?
	public void TailHit () {
		health -= 1;
		animator.SetBool ("Hurt", true);
		if (health < 1) {
			animator.SetBool ("Death", true);
		}
	}
}
