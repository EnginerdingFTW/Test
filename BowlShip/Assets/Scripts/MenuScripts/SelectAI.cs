using UnityEngine;
using System.Collections;

public class SelectAI : MonoBehaviour {

	private BetterCharacterSelection bcs;
	public int playerNum;
	public int difficulty;

	// Use this for initialization
	void Start () {
		bcs = GameObject.Find("CharacterSelection").GetComponent<BetterCharacterSelection> ();
	}

	/// <summary>
	/// If the cursor is shot, lock in that AI player in the chosen difficulty
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "WeaponFire") {
			bcs.SetAI (playerNum - 1, difficulty);
			Destroy (other.gameObject);
		}
	}
}
