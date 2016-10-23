using UnityEngine;
using System.Collections;

public class CreditsRoll : MonoBehaviour {

	public GameObject[] credits;						//The next credits prefab to spawn
	public float spacing = 4.0f;						//the spacing between credits roll
	public float speed = 2.0f;							//speed of credit
	private int creditToSpawn;							//The number associated with the spawned credit
	private GameObject newCredit;						//the credit just spawned

	// Use this for initialization
	void Start () {
		StartCoroutine ("SpawnCredit");
	}

	/// <summary>
	/// Spawns the power up.
	/// </summary>
	/// <returns>The time until powerup is spawned.</returns>
	IEnumerator SpawnCredit () {
		creditToSpawn = 0;
		for (int i = 0; i < credits.Length; i++) {
			yield return new WaitForSeconds (spacing);
			newCredit = (GameObject) Instantiate (credits [creditToSpawn], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
			newCredit.GetComponent<Rigidbody2D>().velocity = new Vector3(-speed, 0, 0);
			creditToSpawn++;
		}
		StartCoroutine ("SpawnCredit");
	}
}
