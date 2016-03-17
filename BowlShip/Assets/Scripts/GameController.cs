using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject[] powerups;										//the total list of spawnable powerups
	public GameObject[] spawnPoints;									//a list of gameObjects with positions to spawn players at
	public GameObject outerBoundary;									//the boundary to destroy faraway objects
	public GameObject innerBoundary;									//the boundary wrapped around the screen
	public GameObject bigAsteroid;										//the big Asteroid to be instantiated
	public GameObject smallAsteroid;									//the small Asteroid to be instantiated
	public int numPlayers; 												//The number of players remaining
	public int[] scores;												//a list of each scores corresponding to each player
	public float asteroidSpawnRate = 3.0f;								//how often the asteroids are spawned
	public float big_small_asteroidProb = 0.8f;							//the probability to spawn either a big or small asteroid
	public float AsteroidSpawnSpeed = 5.0f;								//How fast the asteroids move on spawn
	//public float AsteroidSpawnSpeedRatio = 0.33f;

	private GameObject[] players;										//a list of the prefabs each player is controlling
	private SceneController sceneController;							//The script to pass values between scenes
	
	/// <summary>
	/// Start this instance, i.e. Start the playerList to keep track of. Commence Asteroid bombardment.
	/// </summary>
	void Start () 
	{
		sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();      
		numPlayers = sceneController.numPlayers;
		StartCoroutine(spawnAsteroids());
		players = new GameObject[numPlayers];
		for (int i = 0; i < numPlayers; i++) {
			players [i] = sceneController.playerShips [i];
		}
	}

	/// <summary>
	/// Spawns the big and small asteroids.
	/// </summary>
	/// <returns>The asteroids.</returns>
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

	/// <summary>
	/// Gets the point within spawn range.
	/// </summary>
	/// <returns>The point within spawn range.</returns>
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
			//Debug.Log("in1");
		}
	}

	/// <summary>
	/// Sets the velocity of asteroid.
	/// </summary>
	/// <param name="temp">Temp.</param>
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

	public void CheckEnd (int playerNum) {
		numPlayers--;
		if (numPlayers < 2) {
			SceneManager.LoadScene ("Parr");
		}
	}
}
