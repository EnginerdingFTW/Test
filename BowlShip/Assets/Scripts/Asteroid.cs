using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float powerupChance = 0.5f;	
	public float rotateSpeed = 1.0f;
	public GameObject bigAsteroid;
	public float damage = 30.0f;

	// Use this for initialization
	void Start () 
	{
		rotateSpeed *= Random.Range(0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(Vector3.forward, Time.deltaTime * rotateSpeed);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.collider.tag == "WeaponFire")
		{
			if (bigAsteroid == this.gameObject)
			{
				if (Random.Range(0.0f, 1.0f) >= powerupChance)
				{
					//instantiate powerup
				}
				//instantiate small asteroids spreading out.
			} 
			else 
			{
				//play destroy animation and wait to destroy?
			}
		}

		if (coll.collider.tag == "Player")
		{
			if (bigAsteroid == this.gameObject)
			{
				//coll.collider.gameObject.GetComponent<Player>().
			}	
			else
			{

			}	
		}
	}
}
