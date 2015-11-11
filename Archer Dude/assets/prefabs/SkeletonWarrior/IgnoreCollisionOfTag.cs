using UnityEngine;
using System.Collections;

/// <summary>
/// this entire class has the object the script is attached to ignore collisions with specific tags.
/// </summary>
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
