using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

	public float speed;
	public int NUMBER_OF_INTERPOLATION_POINTS;
	public float despawnTime;
	public float despawnTimeAfterCollision;	
	public float AfterColissionSpeedRatio = 1f;
	public AudioClip concreteBounce;
	
	private AudioSource source;
	private bool shot = false;
	private Player player;	
	private Rigidbody2D rb2d;
	private GameObject leftArm;
	private int counter = 0;
	private float timer = 0;
	private float timeAlive = 0;
	// Use this for initialization
	void Start () 
	{
		rb2d = GetComponent<Rigidbody2D>();
		player = GameObject.Find("Player").GetComponent<Player>();
		leftArm = GameObject.Find("RangerLeftUpperArm_0");
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (rb2d != null)
		{
			if (!shot)
			{
				float rot = leftArm.transform.rotation.eulerAngles.z - 90;
				if (rot < 0)
				{
					rot += 360;
				}
				Vector3 unitVector = new Vector3 (Mathf.Cos(rot * Mathf.Deg2Rad), Mathf.Sin(rot  * Mathf.Deg2Rad), 0);
				UpdateTrajectory(transform.position, unitVector * speed, Physics.gravity);
				if (player.shooting)
				{
					this.transform.parent = null;
					rb2d.isKinematic = false;
					rb2d.velocity = new Vector2(unitVector.x * speed, unitVector.y * speed);
					shot = true;
				}
			}
			if (shot)
			{
				if (GetComponent<BoxCollider2D>() != null)
				{
					GetComponent<BoxCollider2D>().enabled = true;
				}
				Vector3 velocity = new Vector3(rb2d.velocity.x, rb2d.velocity.y, 0);
				float angle = Mathf.Atan(velocity.y / velocity.x) * Mathf.Rad2Deg;
				if (velocity.x < 0)
				{
					angle += 180;
				}
				transform.rotation = Quaternion.Euler(0, 0, angle + 45);		
				Destroy(GetComponent<LineRenderer>());
			}
			if (rb2d.velocity.magnitude > 0)
			{
				timeAlive = CheckForDestroy(timeAlive, despawnTime);
			}
		}
		else 
		{
			timer = CheckForDestroy(timer, despawnTimeAfterCollision);
		}
	}

	float CheckForDestroy(float clock, float despawn)
	{
		if (clock > despawn)
		{
			Destroy(this.gameObject);
		}
		clock += Time.deltaTime;
		return clock;
	}
	
	//d = v_o * t + 1/2 * a * t^2, simple physics, a might just be gravity.
	void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity, Vector3 acceleration)
	{
		float timeDelta = 1.0f / initialVelocity.magnitude;
		
		LineRenderer lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(NUMBER_OF_INTERPOLATION_POINTS);

		Vector3 position = initialPosition;
		Vector3 velocity = initialVelocity;
		for (int i = 0; i < NUMBER_OF_INTERPOLATION_POINTS; i++)
		{
			lineRenderer.SetPosition(i, position);
		
			position += velocity * timeDelta + 0.5f * acceleration * timeDelta * timeDelta;
			velocity += acceleration * timeDelta;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.tag == "Dead")
		{
			Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), other.collider);
		}

		if (other.collider.tag != "Player" && other.collider.tag != "Rope" && other.collider.tag != "Untagged" && other.collider.tag != "Dead")
		{
			this.transform.parent = other.transform;
			if (!other.gameObject.name.Contains("Skeleton"))
			{
				Destroy(this.rb2d);	
				source.PlayOneShot(concreteBounce);
			}
			else 
			{
				rb2d.velocity *= AfterColissionSpeedRatio;
			}
			Destroy(GetComponent<BoxCollider2D>());
			if (GetComponent<LineRenderer>() != null)
			{
				Destroy(GetComponent<LineRenderer>());
			}
		}
	}
}
