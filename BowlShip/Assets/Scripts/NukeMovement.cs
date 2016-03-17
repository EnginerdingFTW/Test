using UnityEngine;
using System.Collections;

public class NukeMovement : MonoBehaviour {

	public float speed;
	public float acc;
	// Use this for initialization
	void Start () 
	{
		GetComponent<Rigidbody2D> ().velocity = transform.up * speed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.gameObject.GetComponent<Rigidbody2D>().velocity += this.gameObject.GetComponent<Rigidbody2D>().velocity.normalized * acc;
	}
}
