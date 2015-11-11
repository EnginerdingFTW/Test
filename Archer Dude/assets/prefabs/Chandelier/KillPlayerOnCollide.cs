using UnityEngine;
using System.Collections;

public class KillPlayerOnCollide : MonoBehaviour {

		//Whenever the object this is attached to collides with the player, MAKE HIM DIE A HORIBBLE DEATH!
	void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			collision.GetComponent<Animator>().SetTrigger("Death");
		}
	}
}
