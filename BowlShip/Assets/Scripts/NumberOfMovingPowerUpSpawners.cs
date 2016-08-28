using UnityEngine;
using System.Collections;

public class NumberOfMovingPowerUpSpawners : MonoBehaviour {

	//Global Variables
	public GameObject[] spawners;
	public float threePlayerSpawnTime = 10;
	public float fivePlayerSpawnTime = 12;
	public float sevenPlayerSpawnTime = 14;
	private SceneController sc;

	// Use this for initialization
	void Start () {
		sc = GameObject.FindGameObjectWithTag ("SceneController").GetComponent<SceneController> ();
		switch (sc.numPlayers) {
		case 2:
			spawners [0].SetActive (true);
			break;
		case 3:
		case 4:
			spawners [0].SetActive (true);
			spawners [1].SetActive (true);
			spawners [0].GetComponent<MovingPowerUpSpawner>().timeTillSpawn = threePlayerSpawnTime;
			spawners [1].GetComponent<MovingPowerUpSpawner>().timeTillSpawn = threePlayerSpawnTime;
			break;
		case 5:
		case 6:
			spawners [0].SetActive (true);
			spawners [1].SetActive (true);
			spawners [2].SetActive (true);
			spawners [0].GetComponent<MovingPowerUpSpawner> ().timeTillSpawn = fivePlayerSpawnTime;
			spawners [1].GetComponent<MovingPowerUpSpawner> ().timeTillSpawn = fivePlayerSpawnTime;
			spawners [2].GetComponent<MovingPowerUpSpawner> ().timeTillSpawn = fivePlayerSpawnTime;
			break;
		case 7:
		case 8:
			spawners [0].SetActive (true);
			spawners [1].SetActive (true);
			spawners [2].SetActive (true);
			spawners [3].SetActive (true);
			spawners [0].GetComponent<MovingPowerUpSpawner>().timeTillSpawn = sevenPlayerSpawnTime;
			spawners [1].GetComponent<MovingPowerUpSpawner>().timeTillSpawn = sevenPlayerSpawnTime;
			spawners [2].GetComponent<MovingPowerUpSpawner>().timeTillSpawn = sevenPlayerSpawnTime;
			spawners [3].GetComponent<MovingPowerUpSpawner>().timeTillSpawn = sevenPlayerSpawnTime;
			break;
		}
	}
}
