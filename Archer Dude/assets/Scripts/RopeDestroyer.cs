using UnityEngine;
using System.Collections;


/// <summary>
/// Destroys the rope when hit by an arrow
/// </summary>
public class RopeDestroyer : MonoBehaviour {

		//If an arrow hits the rope, destroy that peice
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if(coll.collider.tag == "Arrow")
		{
			Destroy (this.gameObject.GetComponent<HingeJoint2D>());
		}
		if (coll.collider.tag == "Player" || coll.collider.tag == "Enemy")
		{
			Physics2D.IgnoreCollision(coll.collider, GetComponent<BoxCollider2D>());
		}
	}
}
