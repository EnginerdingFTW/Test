using UnityEngine;
using System.Collections;

public class Weapon : Collectable {

	public float timer = 5.0f;			//if isTimer is false, this is the quantity of bullets of the weapon, else it is a timer
	public bool isTimer = false;		//^
	public bool isDual = false;			//if isDual is true, the weapon will instantiate out of two separate points
	public float fireRate;				//how fast this weapon can fire (Set in each individual weapon)
	public GameObject laserType;		//The laser that is instantiated with this powerup
	public AudioClip sound;				//The sound to be played when picked up by a player

/// <summary>
/// if the player collides with this object, the player picks up this object.
/// picking up entails making this object no longer collidable or visible
/// </summary>
/// <param name="other">Other collider</param>
	void OnTriggerEnter2D(Collider2D other)		
	{
		if (other.tag == "Player")
		{
			AudioSource.PlayClipAtPoint (sound, new Vector3(0,0,0));
			this.pickedUp = true;
			other.GetComponent<Player>().weapons.Add(this);
			this.GetComponent<Collider2D>().enabled = false;
			this.GetComponent<SpriteRenderer>().enabled = false;
			if (isTimer) {
				StartCoroutine ("KeepTrackOfTime");
			}
			this.gamecontroller.RemoveCollectableFromList(this.gameObject);
		}
	}

	/// <summary>
	/// Keeps the track of time since picked up by Player.
	/// </summary>
	/// <returns>A second. </returns>
	IEnumerator KeepTrackOfTime () {
		while (true) {
			yield return new WaitForSeconds (1.0f);
			timer -= 1.0f;
		}
	}
}
