using UnityEngine;
using System.Collections;

public class PuzzleInsertionRemoval : MonoBehaviour {

	void OnCollisionStay2D(Collision2D other)
	{
		Collide(other.collider);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Collide(other.collider);
	}

	void OnCollisionExit2D(Collision2D other)
	{
		Collide(other.collider);
	}

	void Collide(Collider2D other)
	{
		if (other.tag == "Wall" || other.tag == "Floor")
		{
			other.gameObject.active = false;
		} 	
		else
		{
			Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other);	
		}
		Debug.Log("Collided");
	}
}
