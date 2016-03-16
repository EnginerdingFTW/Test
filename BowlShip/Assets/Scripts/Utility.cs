using UnityEngine;
using System.Collections;

public class Utility : Collectable {

	public float speedChange = 0;						//How much speed to add to the player that picks it up
	public int shieldChange = 0;						//How much shields to recharge on the pickedUp Player
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
			player = other.gameObject.GetComponent<Player> ();
			player.speed += speedChange;
			player.shield += shieldChange;
			player.man += maneuverabilityChange;
		}
	}
}
