using UnityEngine;
using System.Collections;

public class SkeletonSpawner : MonoBehaviour 
{
	public GameObject[] skeletonPrefabs;
	public float[] spawnTime = new float[2];
	public float[] spawnRange = new float[2]; 
	public float[] spawnSpeed = new float[2];
	public float spawnTimerIncrease;
	
	private float nextSpawnTime = 1;
	private float timer = 0;
	private GameObject player;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find("Player");
	}
	
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

	void SpawnSkeleton()
	{
		int index = (int) Random.Range(0, Random.Range(0, skeletonPrefabs.Length));
		GameObject skeletonInstantiation = Instantiate(skeletonPrefabs[index], this.transform.position, this.transform.rotation) as GameObject;
		skeletonInstantiation.transform.parent = this.transform;
		SetVelocity(skeletonInstantiation);
	}
}
