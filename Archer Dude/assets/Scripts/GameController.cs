using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	private EnemySpawner enemySpawner;
	private int spawnedEnemies;

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
			spawnedEnemies = 0;
		}
	}

	public void BossDefeated () {
		enemySpawner.Invoke ("StartSpawning", 0.1f);
	}
}
