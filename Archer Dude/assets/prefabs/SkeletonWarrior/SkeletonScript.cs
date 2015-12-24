using UnityEngine;
using System.Collections;

public class SkeletonScript : MonoBehaviour {

	private Animator animatorSkeleton;			//animator of skeleton
	private GameObject player;					//player object
	private Vector3 constantVelocity;			//velocity of which the skeletons(s) are initially instantiated with
	private float deathCounter = 0;				//amount of time before the skeleton dies after being hit
	private float timer = 0;					//timer to keep track of how long the skeleton has been away from the player
	private int attackChoice = 0;				//AI setting, which attack will it choose?
	private bool AIComplete = false;			//did the skeleton perform it's attack last attack?
	private bool isHit = false;					//true when skeleton gets hit.
	private AudioSource source;					//audio source for sound effect playing

	public AudioClip skellyDeath;				//skeleton death sound effect
	public float despawnTimer;					//amount of time the skeleton must be out of the despawn distance to be destroyed
	public float despawnDistance;				//distance away from player before despawn timer starts
	public float deathTimer;					//amount of time between the skeleton being shot and dying occurs
	public float jumpAttackPercent = 0.2f;		//AI setting - percent of which will be the jump attack
	public float flipAttackPercent = 0.2f;		//AI setting - percent of which will be the flip attack
	public float speed = 2;						//AI setting - how quickly the skeleton runs at the player

	public AnimationClip slash;
	public AnimationClip flip;
	public AnimationClip jump;

	void Start()
	{
			//getting components and choosing AI attack
		player = GameObject.Find("Player");
		animatorSkeleton = GetComponent<Animator>();
		Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = new Vector2(-speed, 0);
		constantVelocity = rb2d.velocity;
		source = GetComponent<AudioSource>();
		ChooseAttack();
	}

	void Update()
	{
			//after the skeleton completes it's first AI attack, determine the next attack to perform. performs AI once within range
		if (!AIComplete && this.gameObject != null)
		{
			SkeletonAISet();	
		}	
	}

	/// <summary>
	/// all timer are within fixed update to keep track of death counters, spawn timers. Also updates position
	/// </summary>
	void FixedUpdate () 
	{
		timer += Time.deltaTime;
		if (timer > despawnTimer)		//despawn timer
		{
			Destroy(this.gameObject);
		}	
		if (Vector3.Distance(player.transform.position, this.transform.position) < despawnDistance)	//despawn distance check
		{
			timer = 0;
		}
		
		if (!isHit && animatorSkeleton.GetBool("run") == true)		//position update
		{
			this.transform.position += constantVelocity * Time.deltaTime;
		}
		if (isHit)		//death counter
		{
			deathCounter += Time.deltaTime;
			if (deathCounter > deathTimer)
			{
				Destroy(this.gameObject.GetComponent<BoxCollider2D>());
			}
		}	
	}
	
		//when hit by an arrow, perform all death functions, sound, etc. calls skeleton death.
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.tag == "Arrow")
		{
			GetComponent<Rigidbody2D>().isKinematic = false;
			GetComponent<BoxCollider2D>().enabled = false;
			isHit = true;
			//this.transform.tag = "Dead";
			Destroy(GetComponent<Animator>());
			SkeletonDeath(this.gameObject.transform);
			source.PlayOneShot(skellyDeath, 1f);
		}
	}


/// <summary>
/// Recursive function that goes through every element of the skeleton, turn's it rigidibody to not kinematic, enables the 
/// the colliders of all the children, and set the children to no longer have a parent.
/// this gives the effect of all of the bones of the skeleton flying apart.
/// </summary>
/// <param name="parent"> treating the heirarchy as a k-th tree where the parent is the root </param>
	void SkeletonDeath(Transform parent)
	{
		Transform[] children = new Transform[parent.childCount];
		for (int k = 0; k <  parent.transform.childCount; k++)
		{
			children[k] = parent.transform.GetChild(k);
		}
		foreach (Transform child in children)
		{
			//child.transform.parent = null;
			child.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
			child.gameObject.GetComponent<BoxCollider2D>().enabled = true;
			child.tag = "Dead";
			if (child.transform.childCount != 0)
			{
				SkeletonDeath(child);
			}
		}
	}

		//trigger the correct AI if the time before collision is less than the time of the animation.
	void SkeletonAISet()
	{
		if (animatorSkeleton == null)
		{
			return;
		}
		float t = TimeBeforeCollision(player, this.gameObject);
		if (t < slash.length * 42f / 89f && attackChoice == 0)
		{
			animatorSkeleton.SetTrigger("Slash");
			AIComplete = true;
		}
		if (t < jump.length * 65f / 120f && attackChoice == 1)
		{
			animatorSkeleton.SetTrigger("Jump");
			AIComplete = true;
		}
		if (t < flip.length * 105f / 231f && attackChoice == 2)
		{
			animatorSkeleton.SetTrigger("Flip");
			AIComplete = true;
		}
	}

		//determine the amount of time before the skeleton collides with the player
	float TimeBeforeCollision(GameObject a, GameObject b)
	{
		float aSpeed = a.GetComponent<Rigidbody2D>().velocity.magnitude;
		float bSpeed = constantVelocity.magnitude;

		Vector2 aPos = new Vector2(a.transform.position.x, a.transform.position.y);
		Vector2 bPos = new Vector2(b.transform.position.x, b.transform.position.y);
		float t = Vector2.Distance(aPos, bPos) / Mathf.Abs(aSpeed + bSpeed);
		return t;
	}

		// determine which AI attack to use.
	void ChooseAttack()
	{
		float dec = Random.Range(0f, 1f);
		
		if (dec > (1 - jumpAttackPercent))
		{
			attackChoice = 1;
		}
		if (dec < flipAttackPercent)
		{
			attackChoice = 2;
		}
	}

		//called during the animations to make sure the skeleton does the animation and is not affected by physics.
	void ChangeGravityScale(float g)
	{
		this.gameObject.GetComponent<Rigidbody2D>().gravityScale = g;
	}
}
