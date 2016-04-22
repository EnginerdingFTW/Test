using UnityEngine;
using System.Collections;

public class WeaponFire : MonoBehaviour {

	[HideInInspector] public GameObject shootingPlayer;				//the player that shot the weapon
	[HideInInspector] public bool hasShot = false;					//was the shootingPlayer assigned yet

	/// <summary>
	/// Used in every weapon shot to make sure it can't hurt the player that shot it.
	/// </summary>
	/// <param name="shootingPlayer">Shooting player.</param>
	public void AttachPlayer (GameObject shootingPlayer) {
		this.shootingPlayer = shootingPlayer;
		hasShot = true;
	}
}
