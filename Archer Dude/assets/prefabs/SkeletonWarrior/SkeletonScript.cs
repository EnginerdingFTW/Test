using UnityEngine;
using System.Collections;

public class SkeletonScript : MonoBehaviour {

	private Animator animatorSkeleton;
	private GameObject player;
	private float timer = 0;	
	private Vector3 constantVelocity;
	private bool isHit = false;
	private float deathCounter = 0;
	private int attackChoice = 0;
	private bool AIComplete = false;
	private AudioSource source;

	public AudioClip skellyDeath;
	public float despawnTimer;
	public float despawnDistance;
	public float deathTimer;
	public float jumpAttackPercent = 0.2f;
	public float flipAttackPercent = 0.2f;

	void Start()
	{
		player = GameObject.Find("Player");
		animatorSkeleton = GetComponent<Animator>();
		Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
		constantVelocity = rb2d.velocity;
		rb2d.velocity = new Vector3(0, 0, 0);
		ChooseAttack();
		source = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (!AIComplete)
		{
			SkeletonAISet();	
		}	
	}

	void FixedUpdate () 
	{
		timer += Time.deltaTime;
		if (timer > despawnTimer)
		{
			Destroy(this.gameObject);
		}	
		if (Vector3.Distance(player.transform.position, this.transform.position) < despawnDistance)
		{
			timer = 0;
		}
		
		if (!isHit)
		{
			this.transform.position += constantVelocity * Time.deltaTime;
		}
		if (isHit)
		{
			deathCounter += Time.deltaTime;
			if (deathCounter > deathTimer)
			{
				Destroy(this.gameObject.GetComponent<BoxCollider2D>());
			}
		}	
	}
	
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

	void SkeletonAISet()
	{
		float t = TimeBeforeCollision(player, this.gameObject);

		if (t < 0.8f && attackChoice == 0)
		{
			animatorSkeleton.SetTrigger("Slash");
			AIComplete = true;
		}
		if (t < 1.1f && attackChoice == 1)
		{
			animatorSkeleton.SetTrigger("Jump");
			AIComplete = true;
		}
		if (t < 1.5f && attackChoice == 2)
		{
			animatorSkeleton.SetTrigger("Flip");
			AIComplete = true;
		}
	}

	float TimeBeforeCollision(GameObject a, GameObject b)
	{
		float aSpeed = a.GetComponent<Rigidbody2D>().velocity.magnitude;
		float bSpeed = b.GetComponent<Rigidbody2D>().velocity.magnitude;

		Vector2 aPos = new Vector2(a.transform.position.x, a.transform.position.y);
		Vector2 bPos = new Vector2(b.transform.position.x, b.transform.position.y);
		
		return (Vector2.Distance(aPos, bPos) / Mathf.Abs(aSpeed - bSpeed));
	}

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

	void ChangeGravityScale(float g)
	{
		this.gameObject.GetComponent<Rigidbody2D>().gravityScale = g;
	}
}
