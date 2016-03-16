using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject[] powerups;
	public GameObject[] players;
	public GameObject[] spawnPoints;
	public GameObject outerBoundary;
	public GameObject innerBoundary;
	public GameObject bigAsteroid;
	public GameObject smallAsteroid;
	public int[] scores;
	public float asteroidSpawnRate = 3.0f;
	public float big_small_asteroidProb = 0.8f;
	public float AsteroidSpawnSpeed = 5.0f;
	//public float AsteroidSpawnSpeedRatio = 0.33f;
	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(spawnAsteroids());
	}

	IEnumerator spawnAsteroids()
	{
		while (true)
		{
			float value = Random.Range(0.0f, 1.0f);
			Vector3 pos = GetPointWithinSpawnRange();
			GameObject temp = null;
			if (value >= big_small_asteroidProb)
			{
				temp = Instantiate(bigAsteroid, pos, Quaternion.identity) as GameObject;
			} 
			else 
			{
				temp = Instantiate(smallAsteroid, pos, Quaternion.identity) as GameObject;
			}
			SetVelocityOfAsteroid(temp);
			yield return new WaitForSeconds(asteroidSpawnRate);
		}
	}

	Vector3 GetPointWithinSpawnRange()
	{
		float radius = outerBoundary.GetComponent<CircleCollider2D>().radius;
		while (true)
		{
			float angle = Random.Range(0.0f, 2*Mathf.PI);
			Vector3 pos = outerBoundary.transform.position + radius * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(0.5f, 1.0f);
			if (!innerBoundary.GetComponent<BoxCollider2D>().bounds.Contains(pos))
			{
				return pos;
			}
			Debug.Log("in1");
		}
	}

	void SetVelocityOfAsteroid(GameObject temp)
	{
		Vector2 outer = innerBoundary.GetComponent<BoxCollider2D>().size;
		float xboundlow = innerBoundary.transform.position.x - outer.x / 2;
		float xboundhigh = innerBoundary.transform.position.x + outer.x / 2;
		float yboundlow = innerBoundary.transform.position.y - outer.y / 2;
		float yboundhigh = innerBoundary.transform.position.y + outer.y / 2;
		Vector2 dir = new Vector3(1.0f, 1.0f);
		do 
		{
			dir = new Vector2(innerBoundary.transform.position.x, innerBoundary.transform.position.y) + new Vector2(Random.Range(xboundlow, xboundhigh), Random.Range(yboundlow, yboundhigh));			
			dir -= new Vector2(temp.transform.position.x, temp.transform.position.y);
		} while (dir.magnitude == 0);
		dir = dir / dir.magnitude;
		temp.GetComponent<Rigidbody2D>().velocity = dir * AsteroidSpawnSpeed;	
	}
}
