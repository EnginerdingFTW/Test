using UnityEngine;
using System.Collections;

public class CreditsPowerUps : MonoBehaviour {

	public GameObject[] powerUps;						//One of the powerUps that can be spawned
	public float min = 2.0f;							//min time till spawn
	public float max = 5.0f;							//max time till spawn
	public float speed = 1.0f;							//speed of powerup
	public bool zoomRight = true;						//do the powerups fly left or right
	private int powerUpToSpawn;							//The number associated with the spawned powerup
	private GameObject newPowerup;						//the powerup just spawned

	// Use this for initialization
	void Start () {
		StartCoroutine ("SpawnPowerUp");
	}

	/// <summary>
	/// Spawns the power up.
	/// </summary>
	/// <returns>The time until powerup is spawned.</returns>
	IEnumerator SpawnPowerUp () {
		yield return new WaitForSeconds (Random.Range(min, max));
		powerUpToSpawn = Random.Range(0, powerUps.Length);
		newPowerup = (GameObject) Instantiate (powerUps [powerUpToSpawn], new Vector3(transform.position.x, transform.position.y, -1.0f), Quaternion.identity);
		if (zoomRight) {
			newPowerup.GetComponent<Rigidbody2D>().velocity = new Vector3(speed, 0, 0);
		} else {
			newPowerup.GetComponent<Rigidbody2D>().velocity = new Vector3(-speed, 0, 0);
		}
		StartCoroutine ("SpawnPowerUp");
	}
}
