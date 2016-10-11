using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Utility : Collectable {

	public int shieldCharge = 0;						//How much shields to recharge on the pickedUp Player
	public int maneuverabilityChange = 0;				//How much to change the player's maneuverability
	public AudioClip sound;								//Play this sound when its picked up by a player

	private Player player;								//The player to be affected by powerUp

	/// <summary>
	/// if the player collides with this object, the player picks up this object.
	/// picking up entails making this object no longer collidable or visible
	/// </summary>
	/// <param name="other">Other collider</param>
	void OnTriggerEnter2D(Collider2D other)		
	{
		if (other.tag == "Player" && !other.gameObject.GetComponent<Player>().victor)
		{
			this.pickedUp = true;
			player = other.gameObject.GetComponent<Player> ();
			player.health += shieldCharge;
			player.healthSlider.value = player.health;
			player.healthSlider.GetComponentsInChildren<Image> () [1].color = Color.cyan;
			player.man += maneuverabilityChange;
			this.GetComponent<Collider2D>().enabled = false;
			this.GetComponent<SpriteRenderer>().enabled = false;
			AudioSource.PlayClipAtPoint (sound, new Vector3(0,0,0));
			this.gamecontroller.RemoveCollectableFromList(this.gameObject);
		}
	}
}
