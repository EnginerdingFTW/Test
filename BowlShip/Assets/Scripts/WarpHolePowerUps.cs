using UnityEngine;
using System.Collections;

public class WarpHolePowerUps : MonoBehaviour {

	public int portalLoc = 0;							//0 for right, 1 for topLeft, 2 for bottomLeft
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
		switch (portalLoc) {
		case 0:
			Instantiate (powerUps [powerUpToSpawn], new Vector3 (transform.position.x - 0.1f, transform.position.y, -1.0f), Quaternion.identity);
			break;
		case 1:
			Instantiate (powerUps [powerUpToSpawn], new Vector3 (transform.position.x + 0.08f, transform.position.y - 0.1f, -1.0f), Quaternion.identity);
			break;
		case 2:
			Instantiate (powerUps [powerUpToSpawn], new Vector3 (transform.position.x + 0.08f, transform.position.y + 0.1f, -1.0f), Quaternion.identity);
			break;
		}
		StartCoroutine ("SpawnPowerUp");
	}
}
