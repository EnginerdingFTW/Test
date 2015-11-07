using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

	public int NUMBER_OF_INTERPOLATION_POINTS;		//number of points to create the prediction line of the arrow
	public float speed;								//speed at which the arrow is fired.
	public float despawnTime;						//amount of time before the arrow despawns after a this time
	public float despawnTimeAfterCollision;			//amount of time before the arrow despawns after a collision
	public float AfterColissionSpeedRatio = 1f;		//if the arrow hits something while its going through the air, how much will it's speed reduce?
	public AudioClip concreteBounce;				//sound if the arrow hits concrete
	
	private AudioSource source;						//audio palyer
	private Player player;							//player script attached to the player gameobject
	private Rigidbody2D rb2d;						//rigidbody of the array
	private GameObject leftArm;						//left arm of the player, used to fix an animation problem
	private bool shot = false;						//shot animation
	private float timer = 0;						//timer for despawnTime
	private float timeAlive = 0;					//timer for despawnTimeAftercollision


	void Start () 
	{	
			//get components
		rb2d = GetComponent<Rigidbody2D>();
		player = GameObject.Find("Player").GetComponent<Player>();
		leftArm = GameObject.Find("RangerLeftUpperArm_0");
		source = GetComponent<AudioSource>();
	}
	
	void FixedUpdate () 
	{
		if (rb2d != null)		//if there is a rigidbody, need to update prediction line
		{
			if (!shot)		//if the arrow is not show, update the prediction line based on it's rotation
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
			if (shot)			//if the arrow was shot, destroy the prediction line and update the arrow's rotation based on velocity
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
			if (rb2d.velocity.magnitude > 0)			//updating timeAlive
			{
				timeAlive = CheckForDestroy(timeAlive, despawnTime);
			}
		}
		else 
		{
			timer = CheckForDestroy(timer, despawnTimeAfterCollision);
		}
	}

		//detroy the arrow if it has been "alive" longer that the despawn timer
	float CheckForDestroy(float clock, float despawn)
	{
		if (clock > despawn)
		{
			Destroy(this.gameObject);
		}
		clock += Time.deltaTime;
		return clock;
	}
	
		// d = v_o * t + 1/2 * a * t^2, simple physics
		// plots all of the vertecies in the line renderer based on the number of interpolation points
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

		//destroy the rigidbody and collider of the arrow when the arrow hit's an Enemy
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.tag == "Dead")
		{
			Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), other.collider);
		}

		if (other.collider.tag != "Player" && other.collider.tag != "Rope" && other.collider.tag != "Untagged" && other.collider.tag != "Dead")
		{
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
