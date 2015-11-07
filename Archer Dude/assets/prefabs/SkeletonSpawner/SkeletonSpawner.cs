using UnityEngine;
using System.Collections;

/// <summary>
/// this class chooses from a list of prefabs and spawns 1 after a random time between the spawn time range. withing a spawn range with a spawn speed
/// </summary>
public class SkeletonSpawner : MonoBehaviour 
{
	public GameObject[] skeletonPrefabs;		//list of skeleton prefabs 
	public float[] spawnTime = new float[2];	//lower bound and upper bound of time between spawn (increased over time)
	public float[] spawnRange = new float[2]; 	//lower and upper bounds of distance from player the spawner can be to spawn
	public float[] spawnSpeed = new float[2];	//lower and upper bounds on the speed the skeleton can spawn with
	public float spawnTimerIncrease;			//incremenet by which the spawn timer is increased
	
	private float nextSpawnTime = 1;			//timer the next skeleton spawn
	private float timer = 0;					//timer
	private GameObject player;					//player gameobject

	void Start () 
	{
			//getting components
		player = GameObject.Find("Player");
	}
	
		//spawns skeletons after the spawn time has passed, gets the time before the next spawn, and increments the spawnTimers
	void FixedUpdate () 
	{
		timer += Time.deltaTime;
		
		if (timer / nextSpawnTime > 1 && Vector3.Distance(this.transform.position, player.transform.position) > spawnRange[0] && Vector3.Distance(this.transform.position, player.transform.position) < spawnRange[1])
		{
			timer = 0;
			nextSpawnTime = GetRandomValue(spawnTime);
			SpawnSkeleton();
		}


			//Increasing time between skeleton spawns
		spawnTime[0] += spawnTimerIncrease * Time.deltaTime;
		spawnTime[1] += spawnTimerIncrease * Time.deltaTime;
	}

	float GetRandomValue(float[] range)
	{
		return Random.Range(range[0], range[1]);
	}
		
		//Sets the velocity of the new skeleton
	void SetVelocity(GameObject skeletonInstantiation)
	{
		Rigidbody2D rb2d = skeletonInstantiation.GetComponent<Rigidbody2D>();
		int factor = -1;
		if (player.transform.position.x > skeletonInstantiation.transform.position.x)
		{
			factor = 1;
		}	

		rb2d.velocity = new Vector2(factor * GetRandomValue(spawnSpeed), 0);
	}
		
		//does everything that spawning a skeleton would require (velocity, position, rotation, which prefab)
	void SpawnSkeleton()
	{
		int index = (int) Random.Range(0, Random.Range(0, skeletonPrefabs.Length));
		GameObject skeletonInstantiation = Instantiate(skeletonPrefabs[index], this.transform.position, this.transform.rotation) as GameObject;
		skeletonInstantiation.transform.parent = this.transform;
		SetVelocity(skeletonInstantiation);
	}
}
