using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax; //The boundaries for the gamescreen so the player can't leave
	
}

public class PlayerController : MonoBehaviour
{
	public float speed;  //Speed of ship, can set from inspector
	public float tilt;   //amount of tilt by ship when moving
	public Boundary boundary; //variable that uses all the variables in the boundary class

	public GameObject shot;     //the object shot
	public Transform shotSpawn; //where the shot spawns
	public float fireRate;		//how fast the ship can fire
	private float nextFire;		//next time the ship can fire

	void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource>().Play();
		}
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;

		GetComponent<Rigidbody>().position = new Vector3 
		(	
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
			0.0f,		
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);

		GetComponent<Rigidbody>().rotation = Quaternion.Euler ( 0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}
}

