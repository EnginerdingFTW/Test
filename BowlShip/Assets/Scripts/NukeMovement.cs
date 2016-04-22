using UnityEngine;
using System.Collections;

public class NukeMovement : MonoBehaviour {
		//this script is for the nuke's movement, needed to be seperate from Nuke due to multiple colliders
		
	public float speed;
	public float acc;
	// Use this for initialization
	void Start () 
	{
		GetComponent<Rigidbody2D> ().velocity = transform.up * -speed;
	}
	
	//the nuke is accellerating.
	void Update () 
	{
		this.gameObject.GetComponent<Rigidbody2D>().velocity += this.gameObject.GetComponent<Rigidbody2D>().velocity.normalized * acc;
	}
}
