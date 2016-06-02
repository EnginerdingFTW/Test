using UnityEngine;
using System.Collections;

public class EMP : WeaponFire {

	//public int damage = 20;						//immediately hit every other player with this damage
	public float NoShieldTime = 16.0f;				//how long every other player can't shield recharge
	public float stunTime = 4.0f;					//how long every other player is stunned
	public GameObject[] otherPlayers;				//a list of every other player

	private GameObject mainCamera;					//the main camera of the scene
	private Player tempPlayer;						//the Player script of the other player being affected
	private Player thisPlayer;						//the shooter's Player script
	private bool stop = false;								//hopefully fixes fast shield recharge bug

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		thisPlayer = shootingPlayer.GetComponent<Player> ();

		//affect other players
		Player.useShieldRecharge = false;									//disable all shield's from recharging
		otherPlayers = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < otherPlayers.Length; i++) {
			if (otherPlayers [i] != shootingPlayer) {
				tempPlayer = otherPlayers [i].GetComponent<Player> ();
				if (tempPlayer.shield > 0) {
					tempPlayer.Hurt (tempPlayer.shield);					//remove all other shields
				}
				tempPlayer.poweredOn = false;
				tempPlayer.StartCoroutine ("Stunned", stunTime);
			}
		}
		StartCoroutine ("EMPTime");
		StartCoroutine ("SelfCharge");
	}

	/// <summary>
	/// Adjusts the sound, waits for the sfx to finish before allowing shield's to recharge again
	/// </summary>
	/// <returns>The time.</returns>
	IEnumerator EMPTime () {
		mainCamera.GetComponent<AudioSource> ().mute = true;
		yield return new WaitForSeconds (NoShieldTime);
		Player.useShieldRecharge = true;
		mainCamera.GetComponent<AudioSource> ().mute = false;
		stop = true;
		Destroy (this.gameObject);
	}

	/// <summary>
	/// Manually charge the shooter's shield after turning off all shield recharging.
	/// </summary>
	/// <returns>The charge.</returns>
	IEnumerator SelfCharge () {
		while (this.gameObject != null) {
			thisPlayer.StartCoroutine ("ShieldRecharge", thisPlayer.shieldChargeRate);
			yield return new WaitForSeconds (thisPlayer.shieldChargeRate + 0.1f);
			if (stop) {
				break;
			}
		}
	}
}
