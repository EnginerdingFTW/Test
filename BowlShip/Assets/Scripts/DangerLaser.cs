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
	/// When the player shoots this massive beam, it instantiates a ton of epic lasers in rapid succession, after finished, it allows the player to move again.
	/// </summary>
	/// <returns>The at will.</returns>
	IEnumerator FireAtWill () {
		yield return new WaitForSeconds (timeTillLaserFire);
		int numLasers = 0;
		while (numLasers < totalInstantiations) {
			currentLaser = (GameObject) Instantiate (epicLaser, new Vector3 (shootingPlayer.transform.position.x, shootingPlayer.transform.position.y, -1.3f), shootingPlayer.transform.rotation);
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
