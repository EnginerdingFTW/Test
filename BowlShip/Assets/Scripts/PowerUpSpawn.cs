using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerUpSpawn : MonoBehaviour {

	public GameObject circleImage;						//The timer gameObject attached to be radially filled
	public float timeTillSpawn = 320.0f;				//How much time until a powerup is spawned
	public float oneSecond = 0.1f;						//How fast to make each "second" in order to smooth the timer
	public GameObject[] powerUps;						//One of the powerUps that can be spawned
	private int powerUpToSpawn;							//The number associated with the spawned powerup
	private Image circleFill;							//The UI component that allows the fill to be radial

	// Use this for initialization
	void Start () {
		circleFill = circleImage.GetComponent<Image> ();
		StartCoroutine ("SpawnPowerUp");
		GameObject.Find("GameController").GetComponent<GameController>().SpawnerList.Add(this.gameObject);
	}

	/// <summary>
	/// Spawns the power up.
	/// </summary>
	/// <returns>The time until powerup is spawned.</returns>
	IEnumerator SpawnPowerUp () {
		circleFill.fillAmount = 0;
		for (int i = 0; i < (int) timeTillSpawn; i++) {
			yield return new WaitForSeconds (oneSecond);
			circleFill.fillAmount += (1.0f / timeTillSpawn);
		}
		powerUpToSpawn = Random.Range(0, powerUps.Length);
		Instantiate (powerUps [powerUpToSpawn], new Vector3(transform.position.x, transform.position.y, -1.0f), Quaternion.identity);
		StartCoroutine ("SpawnPowerUp");
	}

	void OnDestroy()
	{
		if (GameObject.Find("GameController").GetComponent<GameController>() != null)
		{
			GameObject.Find("GameController").GetComponent<GameController>().SpawnerList.Remove(this.gameObject);
		}
	}

}
