using UnityEngine;
using System.Collections;

/// <summary>
/// this class chooses from a list of prefabs and spawns 1 after a set time.
/// </summary>
public class EnemySpawner : MonoBehaviour {

	private Vector3 spawnerPosition;							//the position of the spawner
	private Vector3 playerPosition;								//the position of the player
	private GameObject player;									//the player
	private Animator playerAnimator;							//the player's animator
	private bool timerDone = false;								//This bool makes sure enemies don't breed like rabbits
	private int selectedEnemy;									//This int is used to keep track of what enemy to spawn
	private float enemyChance;									//This float is used to select enemies based on a percentage
	private int numEnemies;										//the amount of enemies to choose from

	public float spawnTime = 4;									//The amount of time between enemy spawns
	public float spawnDistance = 25;							//The distance between the enemy spawn point and the player
	public bool spawnable = true;								//This bool allows enemies to spawn
	public GameObject[] enemies;								//A list of all available enemies to spawn
	public int easy = 0;										//easy enemy int to be changed based on wave
	public int normal = 1;										//normal enemy int to be changed based on wave
	public int hard = 2;										//hard enemy int to be changed based on wave

	// Initialization
	void Start () {
		//getting components
		player = GameObject.Find("Player");
		playerAnimator = player.GetComponent<Animator> ();
		//give the player time to get ready
		StartCoroutine ("spawnTimer");
		numEnemies = enemies.Length;
	}
	
	// Update is called once per frame
	void Update () {
		//This keeps the spawner at a set distance in front of the player (just off screen)
		playerPosition = player.transform.position;
		spawnerPosition = new Vector3 (playerPosition.x + spawnDistance, playerPosition.y, playerPosition.z);

		//This stops enemies from spawning while the player is standing idle
		if (!playerAnimator.GetBool ("run")) 
		{
			spawnable = false;
			StartCoroutine("waitForRun");
		}

		if (spawnable && timerDone) //only spawn if the timer is done and enemies are allowed to spawn
		{
			timerDone = false;
			StartCoroutine("spawnTimer");

			//50% chance of easy enemy, 35% chance of normal enemy, 14% chance of hard enemy, 1% chance of ANY random enemy
			enemyChance = Random.value;
			if (enemyChance < 0.5)
				selectedEnemy = easy;
			else if (enemyChance >= 0.5 && enemyChance < 0.85)
				selectedEnemy = normal;
			else if (enemyChance >= 0.85)
				selectedEnemy = hard;
			else if (enemyChance >= 0.99)
				selectedEnemy = (int) Random.Range(0, numEnemies); //the int acts as a - 1

			//spawn the selected enemy
			Instantiate(enemies[selectedEnemy], spawnerPosition, Quaternion.identity);
		}
	}

	//A simple timer to reset a boolean
	IEnumerator spawnTimer () {
		yield return new WaitForSeconds (spawnTime);
		timerDone = true;
	}

	//Start a thread that waits for the player to start running to allow enemies to spawn
	IEnumerator waitForRun () {
		while (!playerAnimator.GetBool ("run"))
		{
			yield return new WaitForFixedUpdate();
		}
		spawnable = true;
	}
}
