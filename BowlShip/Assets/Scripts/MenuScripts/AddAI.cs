using UnityEngine;
using System.Collections;

public class AddAI : MonoBehaviour {

	private BetterCharacterSelection bcs;
	public int playerNum;

	// Use this for initialization
	void Start () {
		bcs = GameObject.Find("CharacterSelection").GetComponent<BetterCharacterSelection> ();
	}

	/// <summary>
	/// If the correct player flys in, call BetterCharacterSelection's Return
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "WeaponFire") {
			bcs.AddAI (playerNum - 1);
			Destroy (other.gameObject);
		}
	}
}
