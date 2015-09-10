using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Animator animatorCape;
	public Animator animatorPlayer;	
	public Rigidbody2D rb2d;
	public GameObject bow;
	public GameObject stringMarker;
	public GameObject rightArm;
	public GameObject leftArm;
	public GameObject body;
	public GameObject head;
	public GameObject oldArrow;
	public GameObject arrowInsantiation;
	public Sprite bodySprite;
	public Sprite headSprite;
	public bool shooting = false;
	public float CHAOS_CONTROL = 1f;
	public float speed;
	public AudioClip bowFire;
	public AudioClip bowDraw;
	public AudioClip step;
	public AudioClip jumpHut;
	public AudioClip ow;

	private AudioSource source;
	private BoxCollider2D boxCollider;
	private Vector3[] arrowTransform = new Vector3[2];
	private Sprite[] oldSprites = new Sprite[2];
	public bool shootAnimationDone;	
	private int oldBowLayer;
	private bool mouse = false;
	private bool swapped = false;
	private bool shotAnimationDone = false;
	private bool death = false;
	
	// Use this for initialization
	void Start () 
	{	
		oldSprites[0] = bodySprite;
		oldSprites[1] = headSprite;
		shootAnimationDone = false;

		source = GetComponent<AudioSource>();
		animatorPlayer = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
	}
	
	void SetState()
	{
		Time.timeScale = CHAOS_CONTROL;
		shootAnimationDone = true;
		oldArrow.SetActive(false);
		GameObject newArrow = Instantiate(arrowInsantiation, oldArrow.transform.position, oldArrow.transform.rotation) as GameObject;
		newArrow.transform.parent = bow.transform;
	}

	void Death()
	{
		death = true;
	}
	void Footstep()
	{
		source.PlayOneShot(step, 0.2f);
	}
	void ShotDone()
	{
		shotAnimationDone = true;
	}
	void JumpDone()
	{
		animatorPlayer.SetBool("Jump", false);
		animatorCape.SetBool ("Jump", false);
	}
	void SlideDone()
	{
		animatorPlayer.SetBool("Slide", false);
		animatorCape.SetBool ("Slide", false);
	}
	void BowDraw()
	{
		source.PlayOneShot (bowDraw, 2f);
	}

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

	void SwapArmPositions()
	{
		Vector3 temp = leftArm.transform.localPosition;
		leftArm.transform.localPosition = rightArm.transform.localPosition;
		rightArm.transform.localPosition = temp;
	}

	void SetTimeScale(float timeScale)
	{
		Time.timeScale = timeScale;
	}


	// This is called after animations so it will override any changes to position/rotation by the animations will
	// be overridden.
	void LateUpdate () 
	{	
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
		
		
		if (Input.GetMouseButtonDown(0) && !death)
		{	
			GameObject camera = GameObject.Find("Main Camera");
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camera.transform.position.z));
			if (boxCollider.bounds.Contains(mousePos))
			{
				mouse = true;
				animatorPlayer.SetBool("shootArrow", true);	
			}		
		}
		if (Input.GetMouseButtonUp(0) && shootAnimationDone)
		{
			Time.timeScale = 1;
			animatorPlayer.SetBool("shot", true);	
			shootAnimationDone = false;
			mouse = false;
			shooting = true;
			source.PlayOneShot(bowFire, 0.5f);
		}		


		
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
