using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	private EnemySpawner enemySpawner;
	private int spawnedEnemies;
	private GameObject boss;

	public int enemiesTillBoss = 10;

	// Use this for initialization
	void Start () {
		enemySpawner = FindObjectOfType<EnemySpawner> ();
		spawnedEnemies = 0;
	}

	// Keep track of spawned enemies, summon boss after so many have spawned
	public void SpawnCount () {
		spawnedEnemies++;
		if (spawnedEnemies  == enemiesTillBoss) {
			enemySpawner.Invoke("StopSpawning", 0.1f);
			enemySpawner.Invoke("SpawnBoss", 3.0f);
			boss = GameObject.FindGameObjectWithTag("Boss");
			StartCoroutine("BossAlive");
			spawnedEnemies = 0;
		}
	}

	// New thread to keep track of boss being alive or not, checks every 4 seconds
	IEnumerator BossAlive () {
		yield return new WaitForSeconds (4f);
		if (boss == null) {
			enemySpawner.Invoke("StartSpawning", 0.1f);
		} else {
			StartCoroutine("BossAlive");
		}
	}
}
