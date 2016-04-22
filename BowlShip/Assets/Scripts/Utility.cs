using UnityEngine;
using System.Collections;

public class Utility : Collectable {

	public int shieldCharge = 0;						//How much shields to recharge on the pickedUp Player
	public int maneuverabilityChange = 0;				//How much to change the player's maneuverability

	private Player player;								//The player to be affected by powerUp

	/// <summary>
	/// if the player collides with this object, the player picks up this object.
	/// picking up entails making this object no longer collidable or visible
	/// </summary>
	/// <param name="other">Other collider</param>
	void OnTriggerEnter2D(Collider2D other)		
	{
		if (other.tag == "Player")
		{
			this.pickedUp = true;
			player = other.gameObject.GetComponent<Player> ();
			player.shield += shieldCharge;
			player.shieldSlider.value = player.shield;
			player.man += maneuverabilityChange;
			this.GetComponent<Collider2D>().enabled = false;
			this.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}
