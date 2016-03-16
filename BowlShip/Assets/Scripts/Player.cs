using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public int playerNum;								//The player controlling this player
	public int health = 100;							//How much health the ship has left
	public int shield = 10;								//How much shields the ship has
	public float speed = 4.0f;							//How fast the ship can accelerate
	public float rotationSpeed = 5.0f;					//How fast the ship can rotate
	public int man = 1;										//Maneuverability of the ship
	public float fireRate = 2;							//How fast the ship can fire (1s / firerate between shots)
	public List<Weapon> weapons;						//An array of collected weapons

	private float horiz;								//The horizontal movement input
	private float vert;									//The vertical movement input
	private Vector2 movement;							//the total movement of the player
	private Rigidbody2D rb;								//The ship's Rigidbody component

	/** Use this for initialization
	 * 
	 * */
	void Start () {
		playerNum = 1;
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Linear Movement
		horiz = Input.GetAxis ("Horizontal" + playerNum.ToString()) * speed;
		vert = Input.GetAxis ("Vertical" + playerNum.ToString()) * speed;
		movement = new Vector2(horiz, vert);
		rb.AddForce(movement);

		//Angular Movement
		if(horiz != 0 || vert != 0)
		{
			float angle = Mathf.Atan2(vert, horiz) * Mathf.Rad2Deg + 90;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis (angle, Vector3.forward), Time.deltaTime * rotationSpeed);
		} 
	}
}
