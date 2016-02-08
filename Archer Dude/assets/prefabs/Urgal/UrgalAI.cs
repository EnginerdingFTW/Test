using UnityEngine;
using System.Collections;

public class UrgalAI : MonoBehaviour {

	private float timer = 0;					//timer to keep track of how long the urgal has been away from the player
	private GameObject player;					//player object
	private Rigidbody2D rb;

	public float speed = 1f;					//the speed of which the Urgal moves while stepping forward
	public float despawnTimer;					//amount of time the skeleton must be out of the despawn distance to be destroyed
	public float despawnDistance;				//distance away from player before despawn timer starts

	void Start () {
		player = GameObject.Find("Player");
		rb = GetComponent<Rigidbody2D> ();
	}

	/// <summary>
	/// all timer are within fixed update to keep track of death counters, spawn timers. Also updates position
	/// </summary>
	void FixedUpdate ()
	{
		timer += Time.deltaTime;
		if (timer > despawnTimer) {		//despawn timer
			Destroy (this.gameObject);
		}	
		if (Vector3.Distance (player.transform.position, this.transform.position) < despawnDistance) {	//despawn distance check
			timer = 0;
		}
	}

	/**
	 * Used by animation to set the Urgal's speed while walking forward (Trying to replace root motion)
	 */
	void Move () {
		rb.velocity = new Vector2 (-speed, rb.velocity.y);
	}

	/**
	 * Used by animation to set the Urgal's speed while walking forward (Trying to replace root motion)
	 */
	void Stop () {
		rb.velocity = new Vector2 (0, rb.velocity.y);
	}
		
	/**
	 * Used in death animation to remove dead Urgal from the game
	 */
	void DestroyThis () {
		Destroy (this.gameObject);
	}
}
