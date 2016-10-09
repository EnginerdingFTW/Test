using UnityEngine;
using System.Collections;

public class SwitchShipAI : MonoBehaviour {

	private BetterCharacterSelection bcs;			//the script for this menu
	public int playerNum;							//the player (1-8) that this icon is for
	public int direction;							//1 for right, 0 for left

	// Use this for initialization
	void Start () {
		bcs = GameObject.Find("CharacterSelection").GetComponent<BetterCharacterSelection> ();
	}

	/// <summary>
	/// If the cursor is shot, change the AI's selected ship
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "WeaponFire") {
			bcs.SwitchAI (playerNum - 1, direction);
			Destroy (other.gameObject);
		}
	}
}
