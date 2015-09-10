using UnityEngine;
using System.Collections;

public class KillPlayerOnCollide : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.collider.tag == "Player")
		{
			GetComponent<Animator>().SetTrigger("Death");
		}
	}
}
