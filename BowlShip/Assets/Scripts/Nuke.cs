using UnityEngine;
using System.Collections;

public class Nuke : MonoBehaviour {

	public GameObject explosion;
	public float speed;
	public float acc;
	public int damage;
	
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
	
	void Kaboom()
	{
		GameObject temp = Instantiate(explosion, this.transform.position, Quaternion.identity) as GameObject;
		temp.transform.localScale = temp.transform.localScale * 4;
	}

	void OnTriggerEnter2D(Collider2D other)	
	{
		if (other.tag != "Boundary")
		{	
			Kaboom();
			Debug.Log("boom");
			Destroy(this.transform.parent.gameObject, 0.1f);
		}
	}
}
