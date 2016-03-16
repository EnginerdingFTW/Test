using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public GameObject bigAsteroid;				//big asteroid prefab
	public GameObject smallAsteroid;			//smal ".."
	public float powerupChance = 0.5f;			//probability that a big asteroid drops a powerup
	public float rotateSpeed = 1.0f;			//rotation speed of the asteroid
	public float damage = 30.0f;				//damage done to player if hit
	public float smallAstDamageRatio = 0.33f;	//ratio of small asteroid damage to large
	public float spawnSpeed = 5.0f;				//velocity of small asteroids spawned
	public int numberOfSmallAsteroids = 3;		//number of small asteroids maximum spawned when big one destroyed

	// Use this for initialization
	void Start () 
	{
		this.transform.parent = GameObject.Find("Asteroids").transform;
		rotateSpeed *= Random.Range(0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(Vector3.forward, Time.deltaTime * rotateSpeed);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		//if the asteroid is being destroyed....check if this is a big asteroid, if so spawn small ones and possibly a powerup
		if (coll.collider.tag == "WeaponFire")
		{
			if (bigAsteroid == this.gameObject)
			{
				if (Random.Range(0.0f, 1.0f) >= powerupChance)
				{
					//instan
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
				Destroy(this.gameObject);
			} 
			else 
			{
				//play destroy animation and wait to destroy?
			}
		}

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
	}
}
