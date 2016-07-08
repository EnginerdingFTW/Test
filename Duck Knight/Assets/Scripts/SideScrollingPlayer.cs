using UnityEngine;
using System.Collections;

public class SideScrollingPlayer : MonoBehaviour
{

	public float speed = 5;									//How fast the player moves left or right
	public float jumpPower = 10;							//How much force the player jumps with
	public float rollFactor = 1.5f;							//How much faster the player moves when rolling

	private bool crouching = false;							//is the player crouching?
	private bool rolling = false;							//is the character rolling?
	private bool onGround = true;							//is the player on the ground?
	private bool moving = false;							//is the player moving?
	private bool attacking = false;							//is the player attacking?

	private GameController gc;								//the gameController of the game
	private Animator animator;								//animator component
	private Rigidbody2D rb;									//rigidbody component
	private float vertical;									//vertical input
	private float horizontal;								//horizontal input


	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}

	// is called once per frame
	void Update ()
	{
		if (Input.GetAxis ("Item") > 0.1f && !moving && !attacking && !rolling) {
			animator.SetTrigger ("PerspectiveSwitch");
			attacking = true;
		}
		if (Input.GetAxis ("Attack") > 0.1f && !attacking && onGround && !rolling && !crouching) {
			attacking = true;
			gc.shouldSwitch = false;
			animator.SetTrigger ("Attack");
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!attacking) {
			horizontal = Input.GetAxis ("Horizontal");
			vertical = Input.GetAxis ("Vertical");
		

			//Move Left and Right
			if (!rolling) {
				if (Mathf.Abs (horizontal) >= 0.1f) {
					rb.velocity = new Vector3 (horizontal * speed, rb.velocity.y, 0);
					animator.SetBool ("Moving", true);
					moving = true;
				} else {
					rb.velocity = new Vector3 (0, rb.velocity.y, 0);
					animator.SetBool ("Moving", false);
					moving = false;
				}

				if (horizontal > 0) {
					animator.SetBool ("FaceRight", true);
				} else if (horizontal < 0) {
					animator.SetBool ("FaceRight", false);
				}
			

				//Crouching
				if (vertical < -0.1f && onGround && !attacking) {
					rb.velocity = new Vector3 (0, 0, 0);
					moving = false;
					crouching = true;
					animator.SetBool ("Crouching", true);
				} else {
					if (!rolling) {
						crouching = false;
						animator.SetBool ("Crouching", false);
					}
				}
			} else {
				if (horizontal > 0) {
					rb.velocity = new Vector3 (speed * rollFactor, rb.velocity.y, 0);
				} else {
					rb.velocity = new Vector3 (-speed * rollFactor, rb.velocity.y, 0);
				}
			}

			//Jumping or Rolling
			if (Input.GetAxis ("Jump") > 0.1f && onGround && !attacking && !rolling) {
				if (!crouching) {
					onGround = false;
					gc.shouldSwitch = false;
					animator.SetTrigger ("Jump");
					rb.velocity = new Vector3 (rb.velocity.x, jumpPower, 0);
				} else {
					if (Mathf.Abs (horizontal) > 0.1f) {
						gc.shouldSwitch = false;
						rolling = true;
						animator.SetBool ("Rolling", true);
						if (horizontal > 0) {
							rb.velocity = new Vector3 (speed * rollFactor, rb.velocity.y, 0);
						} else {
							rb.velocity = new Vector3 (-speed * rollFactor, rb.velocity.y, 0);
						}
					}
				}
			}
		}
		else {
			//rb.velocity = new Vector3 (0, 0, 0);
			gc.shouldSwitch = false;
		}
	}

	/// <summary>
	/// Called by animations to show that the player is done attacking
	/// </summary>
	public void DoneAttacking ()
	{
		attacking = false;
		gc.shouldSwitch = true;
	}

	/// <summary>
	/// Called by animation to show that the character is done rolling
	/// </summary>
	public void DoneRolling ()
	{
		animator.SetBool ("Rolling", false);
		rolling = false;
		gc.shouldSwitch = true;
	}

	/// <summary>
	/// Called by animations to show that the player has finished its animation
	/// </summary>
	public void DonePerspectiveAnimation ()
	{
		attacking = false;
		gc.SwitchPerspective ();
	}

	/// <summary>
	/// Checks if the player is on the ground
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerStay2D (Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Floor")) {
			gc.shouldSwitch = true;
			onGround = true;
		}
	}
}
