using UnityEngine;
using System.Collections;

public class DangerLaser : WeaponFire {

	public GameObject epicLaser;
	public float timeStunned = 3.7f;
	public float timeTillLaserFire = 2.0f;
	public float timeBetweenInstantiations = 0.01f;
	public int totalInstantiations = 120;
	public AudioClip epicLaserSoundfx;

	private Player playerScript;
	private GameObject currentLaser;

	// Use this for initialization
	void Start () {
		playerScript = shootingPlayer.GetComponent<Player> ();
		playerScript.poweredOn = false;
		//playerScript.Stunned (timeStunned);
		GetComponent<AudioSource> ().PlayOneShot (epicLaserSoundfx);
		StartCoroutine ("FireAtWill");
	}

	/// <summary>
	/// Checks to see if the player is still there, if not don't fire the laser.
	/// </summary>
	void Update () {
		if (playerScript.defeated == true) {
			Destroy (this.gameObject);
		}
	}

	/// <summary>
	/// Takes into accound the angle the ship is facing before giving back a point sufficiently in front of the ship.
	/// </summary>
	/// <returns>The to point in front.</returns>
	/// <param name="oldPos">Old position.</param>
	Vector3 MoveToPointInFront () {
		float z = currentLaser.transform.rotation.eulerAngles.z;
		Vector3 returnable = new Vector3 (Mathf.Sin (Mathf.Deg2Rad * z), -Mathf.Cos (Mathf.Deg2Rad * z), 0).normalized;
		return returnable;
	}

	/// <summary>
	/// When the player shoots this massive beam, it instantiates a ton of epic lasers in rapid succession, after finished, it allows the player to move again.
	/// </summary>
	/// <returns>The at will.</returns>
	IEnumerator FireAtWill () {
		yield return new WaitForSeconds (timeTillLaserFire);
		int numLasers = 0;
		while (numLasers < totalInstantiations) {
			currentLaser = (GameObject) Instantiate (epicLaser, new Vector3 (shootingPlayer.transform.position.x, shootingPlayer.transform.position.y, -1.3f), shootingPlayer.transform.rotation);
			currentLaser.transform.position += MoveToPointInFront ();
			currentLaser.GetComponent<LaserMover> ().hasShot = true;
			currentLaser.GetComponent<LaserMover> ().shootingPlayer = shootingPlayer;
			currentLaser.GetComponent<LaserMover> ().epic = true;
			yield return new WaitForSeconds (timeBetweenInstantiations);
			numLasers++;
		}
		playerScript.poweredOn = true;
		Destroy (this.gameObject);
	}
}
