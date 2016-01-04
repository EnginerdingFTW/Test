using UnityEngine;
using System.Collections;

public class UrgalAI : MonoBehaviour {

	private float timer = 0;					//timer to keep track of how long the urgal has been away from the player
	private GameObject player;					//player object

	public float despawnTimer;					//amount of time the skeleton must be out of the despawn distance to be destroyed
	public float despawnDistance;				//distance away from player before despawn timer starts

	void Start () {
		player = GameObject.Find("Player");
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

	void DestroyThis () {
		Destroy (this.gameObject);					//Used in death animation to remove dead Urgal from the game
	}
}
