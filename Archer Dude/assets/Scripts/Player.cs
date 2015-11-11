using UnityEngine;
using System.Collections;


/// <summary>
/// The player controller script
/// </summary>
public class Player : MonoBehaviour {

	[HideInInspector]
	public Rigidbody2D rb2d;							//rigidbody of the player
	public Animator animatorCape;						//Cape animator component-separate from player
	public Animator animatorPlayer;						//Player animator component
	public GameObject bow;								//bow of player
	public GameObject stringMarker;						//string of the bow
	public GameObject rightArm;							//right arm of player
	public GameObject leftArm;							//left arm of player
	public GameObject body;								//torso of player
	public GameObject head;								//head of player
	public GameObject oldArrow;							//an arrow that was last shot from the bow
	public GameObject arrowInsantiation;				//the arrow used for instantiation
	public Sprite bodySprite;							//current body sprite of player
	public Sprite headSprite;							//head spriate of the player
	public bool shooting = false;						//is the shooting animation currently going?
	public float CHAOS_CONTROL = 1f;					//time factor for when you are shooting (slows time)
	public float speed;									//moving speed of player
	public AudioClip bowFire;							//audio - firing bow
	public AudioClip bowDraw;							//audio - drawing bow
	public AudioClip step;								//audio - walking
	public AudioClip jumpHut;							//audio - when jump
	public AudioClip ow;								//audio - getting hit
	public BoxCollider2D aimBox;						//collider that must be touched to fire the bow
	public bool shootAnimationDone;						//true when shoot animation is done

	private AudioSource source;							//main audio source in scene
	private BoxCollider2D boxCollider;					//collider of the player
	private Sprite[] oldSprites = new Sprite[2];		//temp variable for swapping body and head sprites when the player faces other direction
	private int oldBowLayer;							//temp variable used for moving the bow's layer up during animation
	private bool mouse = false;							//true when mouse click is still down
	private bool swapped = false;						//true when the left and right arm's positions are swapped (during reverse)
	private bool shotAnimationDone = false;				//true when shotAnimation done
	private bool death = false;							//true when player is DEAD
	
	// Use this for initialization
	void Start () 
	{	
			//saving old sprites to array for swapping
		oldSprites[0] = bodySprite;
		oldSprites[1] = headSprite;
		shootAnimationDone = false;

			//Getting components
		source = GetComponent<AudioSource>();
		animatorPlayer = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
	}
	
/// <summary>
/// This function is called at the end of the shoot animation (drawing of the arrow). It slows
/// time down and instantiates a new arrow (the 1 that will actually be show, the last 1 was just for the animation).
/// </summary>
	void SetState()
	{
		Time.timeScale = CHAOS_CONTROL;
		shootAnimationDone = true;
		oldArrow.SetActive(false);
		GameObject newArrow = Instantiate(arrowInsantiation, oldArrow.transform.position, oldArrow.transform.rotation) as GameObject;
		newArrow.transform.parent = bow.transform;
	}

/// <summary>
/// YOU MUST DIE.
/// </summary>
	void Death()
	{
		death = true;
	}

/// <summary>
/// Plays footsteps sound within the walking animation
/// </summary>
	void Footstep()
	{
		source.PlayOneShot(step, 0.2f);
	}
/// <summary>
/// Called after the Shot animation is completed (arrow is fired)
/// </summary>
	void ShotDone()
	{
		shotAnimationDone = true;
	}

/// <summary>
/// Called after jump animation finishes
/// </summary>
	void JumpDone()
	{
		animatorPlayer.SetBool("Jump", false);
		animatorCape.SetBool ("Jump", false);
	}

/// <summary>
/// Called after the slide animation is done
/// </summary>
	void SlideDone()
	{
		animatorPlayer.SetBool("Slide", false);
		animatorCape.SetBool ("Slide", false);
	}

/// <summary>
/// called during bow draw for sound effect.
/// </summary>
	void BowDraw()
	{
		source.PlayOneShot (bowDraw, 2f);
	}

/// <summary>
/// Function to swap the current head and body spites of the player with other sprites.
/// This is for when the player looks backwards.
/// </summary>
	void SwapSprites()
	{
		Sprite temp;
		temp = body.GetComponent<SpriteRenderer>().sprite;
		body.GetComponent<SpriteRenderer>().sprite = oldSprites[0];
		oldSprites[0] = temp;
		temp = head.GetComponent<SpriteRenderer>().sprite;
		head.GetComponent<SpriteRenderer>().sprite = oldSprites[1];
		oldSprites[1] = temp;
	}

/// <summary>
/// Swaps the positions of the left and right arms. This is used when the player is shooting and looks backwards.
/// </summary>
	void SwapArmPositions()
	{
		Vector3 temp = leftArm.transform.localPosition;
		leftArm.transform.localPosition = rightArm.transform.localPosition;
		rightArm.transform.localPosition = temp;
	}

/// <summary>
/// Sets the rate of time.
/// </summary>
/// <param name="timeScale"> new value for time </param>
	void SetTimeScale(float timeScale)
	{
		Time.timeScale = timeScale;
	}

/// <summary>
/// After the player dies, the menu is pulled up.
/// </summary>
	void Restart()
	{
		Application.LoadLevel("Menu");
	}

/// <summary>
/// Late update was used because we needed animations to be overwritten, so everything takes place after the animations.
/// </summary>
	void LateUpdate () 
	{	
			//if the player is doing the run animation and not dead, run at the set speed, otherwise stop moving.
		if (animatorPlayer.GetBool("run"))
		{
			if (!death)
			{
				rb2d.velocity = new Vector3(speed, 0, 0);
			}
			else 
			{
				rb2d.velocity = new Vector3(0, 0, 0);
			}
		}
		else 
		{
			rb2d.velocity = new Vector3(0, 0, 0);
		}

			//Touch input for jump and slide
		if (Input.touchCount > 1 && Input.GetTouch(0).phase == TouchPhase.Moved && animatorPlayer.GetBool ("run")) 
		{
			// Get movement of the finger since last frame
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

				if(touchDeltaPosition.y > 0f)
				{
					if(animatorPlayer.GetBool ("Jump") == false)
					{
						source.PlayOneShot (jumpHut, 0.2f);
					}
					animatorPlayer.SetBool("Jump", true);
					animatorCape.SetBool ("Jump", true);
				}

				if(touchDeltaPosition.y < 0f)
				{
					animatorPlayer.SetBool("Slide", true);
					animatorCape.SetBool ("Slide", true);
				}
		}

			//Inputs for keyboard, used while creating game - W causes jump animation and S causes slide.
		if (Input.GetKey(KeyCode.W) && animatorPlayer.GetBool("run"))
		{
			if(animatorPlayer.GetBool ("Jump") == false)
			{
				source.PlayOneShot (jumpHut, 0.2f);
			}
			animatorPlayer.SetBool("Jump", true);
			animatorCape.SetBool ("Jump", true);
		}
		if (Input.GetKey(KeyCode.S) && animatorPlayer.GetBool("run"))
		{
			animatorPlayer.SetBool("Slide", true);
			animatorCape.SetBool ("Slide", true);
		}

			//after the shot animation is complete, the player returns to the running state. Parameters must be reset that keep
			//track of animations that are happening and sprites
		if (shotAnimationDone)
		{
			animatorPlayer.SetBool("shootArrow", false);
			animatorPlayer.SetBool("shot", false);
			shootAnimationDone = false;
			shooting = false;
			shotAnimationDone = false;
			oldArrow.SetActive(true);
			
			if (swapped)
			{
				SwapSprites();
				SwapArmPositions();
				swapped = false;
			}
		}
		
			//Shooting controls, if the user clicks withing the aimbox, start the shooting animation, when the user lets go
			//fire in the direction opposite of the mouse position.
		if (Input.GetMouseButtonDown(0) && !death)
		{	
			GameObject camera = GameObject.Find("Main Camera");
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camera.transform.position.z));

			if (aimBox.bounds.Contains(mousePos))
			{
				mouse = true;
				animatorPlayer.SetBool("shootArrow", true);	
			}		
		}


		//Input.GetMouseButtonUp(0)
		if (Input.GetMouseButtonUp(0) && shootAnimationDone)
		{
			Time.timeScale = 1;
			animatorPlayer.SetBool("shot", true);	
			shootAnimationDone = false;
			mouse = false;
			shooting = true;
			source.PlayOneShot(bowFire, 0.5f);
		}




//		if (Input.touchCount < 1 && shootAnimationDone)
//		{
//			Time.timeScale = 1;
//			animatorPlayer.SetBool("shot", true);	
//			shootAnimationDone = false;
//			mouse = false;
//			shooting = true;
//			source.PlayOneShot(bowFire, 0.5f);
//		}		



			// this entire section is when the player is doing the about to fire the arrow (arrow is already drawn) animation
			// using the mouse/finger position, a prediction line for shooting must be drawn AND sprite components must be
			// properly set if the user has the player shooting backwards.
		if (((mouse && shootAnimationDone) || shooting) && !death)
		{
			bool flip = false;
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
			float angle = Mathf.Atan((mousePos.y - bow.transform.position.y) / (mousePos.x - bow.transform.position.x)) * Mathf.Rad2Deg;				
			if (mousePos.x > bow.transform.position.x)
			{
				angle -= 180;
			}
			angle += 90;
			leftArm.transform.rotation = Quaternion.Euler(0, 0, angle);
		
			if (angle > 20 || angle < -20)
			{
				head.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
				if (angle < 0)
				{
					flip = true;
				}
			}
			else 
			{
				if (angle + 90 > 0)
				{
					head.transform.rotation = Quaternion.Euler(0, 0, -70);
				}	
				else 
				{
					head.transform.rotation = Quaternion.Euler(0, 0, -120);
				}
			}

			if ((head.transform.rotation.eulerAngles.z > 90 && head.transform.rotation.eulerAngles.z < 270) && !swapped)
			{
				SwapSprites();
				SwapArmPositions();
				swapped = true;
			}
			if ((head.transform.rotation.eulerAngles.z < 90 || head.transform.rotation.eulerAngles.z > 270) && swapped)
			{

				SwapSprites();
				SwapArmPositions();
				swapped = false;
			}

			float angle2 = Mathf.Atan((rightArm.transform.position.y - stringMarker.transform.position.y) / (rightArm.transform.position.x - stringMarker.transform.position.x)) * Mathf.Rad2Deg + 90;
			if (stringMarker.transform.position.x < rightArm.transform.position.x)
			{
				angle2 -= 180;
			}	
			rightArm.transform.rotation = Quaternion.Euler(0, 0, angle2);

			if (flip)
			{
				head.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
			}
		}
	}


/// <summary>
/// Currently, if the player collides with an enemy or anything that we want to kill him, he dies.
/// </summary>
/// <param name="collision"> collision parameter of what hit the players collider </param>
	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.collider.tag == "Enemy" || collision.collider.tag == "Death")
		{
			animatorPlayer.SetTrigger("Death");
			animatorPlayer.SetBool("run", false);
			source.PlayOneShot(ow, 0.3f);
		}
	}
}
