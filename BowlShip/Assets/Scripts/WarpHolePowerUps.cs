using UnityEngine;
using System.Collections;

public class WarpHolePowerUps : MonoBehaviour {

	public float timeTillSpawn = 15.0f;					//How much time until a powerup is spawned
	public GameObject[] powerUps;						//One of the powerUps that can be spawned
	private int powerUpToSpawn;							//The number associated with the spawned powerup

	// Use this for initialization
	void Start () {
		StartCoroutine ("SpawnPowerUp");
	}
	
	/// <summary>
	/// Spawns the power up.
	/// </summary>
	/// <returns>The time until powerup is spawned.</returns>
	IEnumerator SpawnPowerUp () {
		yield return new WaitForSeconds (timeTillSpawn);
		powerUpToSpawn = Random.Range(0, powerUps.Length);
		Instantiate (powerUps [powerUpToSpawn], new Vector3(transform.position.x - 0.1f, transform.position.y, -1.0f), Quaternion.identity);
		StartCoroutine ("SpawnPowerUp");
	}
}
