using UnityEngine;
using System.Collections;

public class IgnoreCollisionOfTag : MonoBehaviour {
	
	string[] tags = {"Player", "Floor", "Enemy"};

	void OnCollisionEnter2D(Collision2D collision)
	{
		foreach (string tag in tags)
		{
			if (collision.collider.tag == tag)
			{
				Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
			}
		}
	}
}
