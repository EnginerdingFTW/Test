using UnityEngine;
using System.Collections;

public class TopDownPlayer : MonoBehaviour {

	public float speed = 5;						//How fast the player moves
	public float rollFactor = 2;				//How much faster the player moves when rolling

	private bool rolling = false;				//is the character rolling?
	private bool moving = false;				//is the character moving?
	private bool attacking = false;				//is the character attacking?
	private GameController gc;					//the gameController of the game
	private Animator animator;					//animator component
	private Rigidbody2D rb;						//rigidbody component
	private float vertical;						//vertical input
	private float horizontal;					//horizontal input
	private Vector3 movement;					//the movement vector of the player

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame, used for physics
	void FixedUpdate () {
		if (!rolling && !attacking) {
			horizontal = Input.GetAxis ("Horizontal");
			vertical = Input.GetAxis ("Vertical");
		}

		//Movement
		if ((Mathf.Abs (horizontal) >= 0.1f || Mathf.Abs (vertical) >= 0.1f) && !attacking) {
			movement = new Vector3 (horizontal, vertical, 0).normalized;
			rb.velocity = movement * speed;
			animator.SetBool ("Moving", true);
			moving = true;
			if (Mathf.Abs(horizontal) >= Mathf.Abs(vertical)) {
				if (horizontal > 0) {
					animator.SetBool ("FaceRight", true);
					animator.SetBool ("FaceUp", false);
					animator.SetBool ("FaceLeft", false);
					animator.SetBool ("FaceDown", false);
				} else {
					animator.SetBool ("FaceRight", false);
					animator.SetBool ("FaceUp", false);
					animator.SetBool ("FaceLeft", true);
					animator.SetBool ("FaceDown", false);
				}
			} else {
				if (vertical > 0) {
					animator.SetBool ("FaceRight", false);
					animator.SetBool ("FaceUp", true);
					animator.SetBool ("FaceLeft", false);
					animator.SetBool ("FaceDown", false);
				} else {
					animator.SetBool ("FaceRight", false);
					animator.SetBool ("FaceUp", false);
					animator.SetBool ("FaceLeft", false);
					animator.SetBool ("FaceDown", true);
				}
			}
		} else {
			if (!rolling) {
				rb.velocity = new Vector3 (0, 0, 0);
				animator.SetBool ("Moving", false);
				moving = false;
			}
		}
	}

	// is called once per frame
	void Update () {
		if (Input.GetAxis ("Item") > 0.1f && !moving && !attacking) {
			animator.SetTrigger ("PerspectiveSwitch");
			attacking = true;
		}

		if (Input.GetAxis ("Jump") > 0.1f && !rolling && moving && !attacking) {
			rolling = true;
			gc.shouldSwitch = false;
			speed = speed * rollFactor;
			animator.SetBool ("Moving", false);
			moving = false;
			if (horizontal >= 0) {
				animator.SetBool ("FaceRight", true);
				animator.SetBool ("FaceUp", false);
				animator.SetBool ("FaceLeft", false);
				animator.SetBool ("FaceDown", false);
				animator.SetBool ("Rolling", true);
			} else {
				animator.SetBool ("FaceRight", false);
				animator.SetBool ("FaceUp", false);
				animator.SetBool ("FaceLeft", true);
				animator.SetBool ("FaceDown", false);
				animator.SetBool ("Rolling", true);
			}
		}

		if (Input.GetAxis ("Attack") > 0.1f && !rolling && !attacking) {
			gc.shouldSwitch = false;
			attacking = true;
			animator.SetTrigger ("Attack");
		}
	}

	/// <summary>
	/// Called by animation to show that the character is done rolling
	/// </summary>
	public void DoneRolling() {
		rolling = false;
		gc.shouldSwitch = true;
		speed = speed / rollFactor;
		animator.SetBool ("Rolling", false);
	}

	/// <summary>
	/// Called by animation to show that the character is done attacking
	/// </summary>
	public void DoneAttacking() {
		attacking = false;
		gc.shouldSwitch = true;
	}

	/// <summary>
	/// Called by animations to show that the player has finished its animation
	/// </summary>
	public void DonePerspectiveAnimation() {
		attacking = false;
		gc.SwitchPerspective ();
	}
}
