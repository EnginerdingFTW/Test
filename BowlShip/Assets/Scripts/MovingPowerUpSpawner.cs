using UnityEngine;
using System.Collections;

public class MovingPowerUpSpawner : MonoBehaviour {

	public GameObject pusher;							//The pusher box attached to the spawner
	public float timeTillSpawn = 8.0f;					//How much time until a powerup is spawned
	public GameObject[] powerUps;						//One of the powerUps that can be spawned
	public GameObject[] targets;						//Where the mover has to move to with NavAgent
	public float proximatyToTarget = 0.2f;				//How close does the mover need to get to its target before moving on to the next one
	public float proximatyAngle = 0.05f;				//How close does the angle have to be before moving

	private Rigidbody2D rb;								//The rigidbody of this powerup spawner
	private GameObject currentTarget;					//Where the mover is currently moving
	private int targetNum;								//The current target's number identification
	private int powerUpToSpawn;							//The number associated with the spawned powerup
	private bool onTarget = false;						//Did the mover reach its intended target
	//private NavMeshAgent agent;						//The NavMeshAgent attached to the mover

	//Movement
	public float speed = 2.0f;							//How fast does the ship move
	private float angle;								//what angle the ship must turn to
	private int missed;									//a counter to see make the ship go where it needs to
	public int timeAllowedToRotate = 1000;				//what missed has to go to in order to break out of while loop
	public float rotationSpeed = 0.1f;					//how fast to turn the ship
	public float updateTime = 0.1f;						//how quickly to apply movement

	// Use this for initialization
	void Start () {
		//agent = GetComponent<NavMeshAgent> ();
		targetNum = 0;
		currentTarget = targets [targetNum];
		rb = GetComponent<Rigidbody2D> ();
		StartCoroutine ("SpawnPowerUp");
		StartCoroutine ("MoveAlongPath");
	}

	// Every frame check to see if the powerupSpawner is on the next target
	void Update () {
		if ((transform.position.x < currentTarget.transform.position.x + proximatyToTarget &&
		    transform.position.x > currentTarget.transform.position.x - proximatyToTarget) &&
		    (transform.position.y < currentTarget.transform.position.y + proximatyToTarget &&
		    transform.position.y > currentTarget.transform.position.y - proximatyToTarget)) {
			onTarget = true;
		} else {
			onTarget = false;
		}
	}

	/// <summary>
	/// Spawns the power up.
	/// </summary>
	/// <returns>The time until powerup is spawned.</returns>
	IEnumerator SpawnPowerUp () {
		pusher.transform.localScale = new Vector3 (0.2f, 0.2f, 1.0f);
		yield return new WaitForSeconds (timeTillSpawn / 5);
		pusher.transform.localScale = new Vector3 (0.4f, 0.4f, 1.0f);
		yield return new WaitForSeconds (timeTillSpawn / 5);
		pusher.transform.localScale = new Vector3 (0.6f, 0.6f, 1.0f);
		yield return new WaitForSeconds (timeTillSpawn / 5);
		pusher.transform.localScale = new Vector3 (0.8f, 0.8f, 1.0f);
		yield return new WaitForSeconds (timeTillSpawn / 5);
		pusher.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		yield return new WaitForSeconds (timeTillSpawn / 5);
		powerUpToSpawn = Random.Range(0, powerUps.Length);
		Instantiate (powerUps [powerUpToSpawn], transform.position, Quaternion.identity);
		StartCoroutine ("SpawnPowerUp");
	}

	/// <summary>
	/// Makes the mover target each target in the path and methodically move to each one
	/// </summary>
	IEnumerator MoveAlongPath () {
		currentTarget = targets [targetNum];

		//Find the angle and rotate the mover towards in Slerp
		missed = 0;
		angle = Mathf.Atan2 ((currentTarget.transform.position.y - transform.position.y), (currentTarget.transform.position.x - transform.position.x)) * Mathf.Rad2Deg + 45;
		while (transform.rotation.eulerAngles.z > angle + proximatyAngle || transform.rotation.eulerAngles.z < angle - proximatyAngle) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis (angle, Vector3.forward), Time.deltaTime * rotationSpeed);
			yield return new WaitForSeconds (updateTime);
			missed++;
			if (missed > timeAllowedToRotate) {
				break;
			}
		}
		transform.rotation = Quaternion.Euler (0, 0, angle);

		//Add Force Until the point is reached
		while (!onTarget) {
			rb.velocity = (transform.right - transform.up) * speed;
			yield return new WaitForSeconds (updateTime);
		}
		rb.velocity = new Vector3 (0, 0, 0);

		//Start going to the next target
		targetNum++;
		if (targetNum >= targets.Length) {
			targetNum = 0;
		}
		yield return new WaitForSeconds (updateTime);
		StartCoroutine ("MoveAlongPath");
	}
}
