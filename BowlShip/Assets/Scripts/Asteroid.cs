using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public GameObject bigAsteroid;				//big asteroid prefab
	public GameObject smallAsteroid;			//smal ".."
	public GameObject tinyAsteroid;				
	public GameObject explosion;				//explosion prefab

	public Vector3 explosionSize;				//the size of the explosion caused by an asteroid getting hit
	public float powerupChance = 0.5f;			//probability that a big asteroid drops a powerup, higher number is lower chance
	public float rotateSpeed = 1.0f;			//rotation speed of the asteroid
	public float damage = 30.0f;				//damage done to player if hit
	public float smallAstDamageRatio = 0.33f;	//ratio of small asteroid damage to large
	public float spawnSpeed = 5.0f;				//velocity of small asteroids spawned
	public float tinyAsteroidDestroyTime = 1.0f; //how quickly the tiny asteroids are destroyed
	public int numberOfSmallAsteroids = 3;		//number of small asteroids maximum spawned when big one destroyed
	public float forceTime = 0.3f;				//how long the point effector pushes back objects on collision

	private PointEffector2D pe;					//the point effector to push back objects that it collides with

	// Use this for initialization
	void Start () 
	{
		this.transform.parent = GameObject.Find("Asteroids").transform;
		rotateSpeed *= Random.Range(0.0f, 1.0f);
		if (this.gameObject == tinyAsteroid)
		{
			Destroy(this.gameObject, tinyAsteroidDestroyTime);
		}
		pe = GetComponent<PointEffector2D> ();

		GameObject.Find("GameController").GetComponent<GameController>().SpawnerList.Add(this.gameObject);
	}
	
	// Keep the asteroid rotating
	void Update () 
	{
		transform.Rotate(Vector3.forward, Time.deltaTime * rotateSpeed);
	}


	void Death()
	{
		Destroy(this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		//if the asteroid collides with the player hurt the player
		if (coll.collider.tag == "Player")
		{
			if (bigAsteroid == this.gameObject)
			{
				coll.collider.gameObject.GetComponent<Player>().Hurt((int)damage);
			}	
			else
			{
				coll.collider.gameObject.GetComponent<Player>().Hurt((int)(damage * smallAstDamageRatio));
			}
		}
		pe.enabled = true;
		StartCoroutine ("RegulateForce");
	}

		
	void OnTriggerEnter2D(Collider2D other)	
	{
		if (other.tag == "WeaponFire")
		{
				//if it is the big asteroid, possibly drop a powerup
			if (bigAsteroid == this.gameObject)
			{
				if (Random.Range(0.0f, 1.0f) >= powerupChance)
				{
					GameObject[] powerups = GameObject.Find("GameController").GetComponent<GameController>().powerups;
					Instantiate(powerups[Random.Range(0, powerups.Length)], this.transform.position, Quaternion.identity);
				}

				//instantiate small asteroids spreading out.
				int k = 0;
				int count = Random.Range(1, numberOfSmallAsteroids);
				for (k = 0; k < count; k++)
				{	
					Vector3 newPos = this.transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0.0f);
					GameObject temp = Instantiate(smallAsteroid, newPos, Quaternion.identity) as GameObject;
					Vector3 vel = newPos - this.transform.position;
					temp.GetComponent<Rigidbody2D>().velocity = spawnSpeed * new Vector2(vel.x, vel.y);
				}

				//instantiate tiny asteroids for the purpose of animation only
				for (k = 0; k < 5; k++)
				{	
					Vector3 newPos = this.transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0.0f);
					GameObject temp = Instantiate(tinyAsteroid, newPos, Quaternion.identity) as GameObject;
					Vector3 vel = newPos - this.transform.position;
					temp.GetComponent<Rigidbody2D>().velocity = spawnSpeed * new Vector2(vel.x, vel.y);
				}
				Destroy(this.gameObject);
			} 
			else 
			{
				GameObject thisExplosion = (GameObject) Instantiate(explosion, this.transform.position, Quaternion.identity);
				thisExplosion.transform.localScale = explosionSize;
				Destroy(this.gameObject);
			}
		}
	}

	void OnDestroy()
	{
		GameObject.Find("GameController").GetComponent<GameController>().SpawnerList.Remove(this.gameObject);
	}

	IEnumerator RegulateForce () {
		yield return new WaitForSeconds (forceTime);
		pe.enabled = false;
	}
}
