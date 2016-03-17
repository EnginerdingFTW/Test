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
	public int gameMode;												//The int number corresponding to each gameMode
	public int numPlayers; 												//The number of players remaining
	public float asteroidSpawnRate = 3.0f;								//how often the asteroids are spawned
	public float big_small_asteroidProb = 0.8f;							//the probability to spawn either a big or small asteroid
	public float AsteroidSpawnSpeed = 5.0f;								//How fast the asteroids move on spawn
	//public float AsteroidSpawnSpeedRatio = 0.33f;

	public int defaultPlayerHealth = 100;								//The default health and
	public int defaultPlayerShields = 100;								//shields to reset a player with

	private bool asteroidTime = true;									//spawn Asteroids
	public int maxScore;												//the score needed to win the game
	private GameObject[] players;										//a list of the prefabs each player is controlling
	public int[] scores;												//a list of each scores corresponding to each player
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
		maxScore = sceneController.score;
		scores = new int[numPlayers];
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
		while (asteroidTime)
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

	/// <summary>
	/// Checks if the round has ended each time a player is defeated. If a player has reached the maxScore, Show Victory Screen.
	/// </summary>
	/// <param name="playerNum">Defeated Player's number.</param>
	public void CheckEnd (int playerNum) {
		//Instance Variables to save memory
		//int playerThatWon;							//the winning player of the round, used for Victory Screen?
		bool isDraw = true;								//was this round a draw?
		GameObject[] asteroids;							//list of all leftover asteroids to destroy

		numPlayers--;
		Debug.Log ("Player " + playerNum.ToString() + " Defeated!");
		if (numPlayers < 2) {
			asteroidTime = false;

			asteroids = GameObject.FindGameObjectsWithTag ("Asteroid");
			for (int i = 0; i < asteroids.Length; i++) {
				Destroy (asteroids [i]);
			}

			for (int i = 0; i < players.Length; i++) {
				if (!players [i].GetComponent<Player>().defeated) {
					scores [i]++;
					isDraw = false;						//Should never come to a draw, unless both players are defeated in the EXACT same frame
					Debug.Log ("The winner of the round is: Player " + (i + 1).ToString() + " Score: " + scores[i].ToString());
				}
			}
			if (isDraw) {
				Debug.Log ("It's a draw!");
			}
			for (int i = 0; i < scores.Length; i++) {
				if (scores[i] >= maxScore) {
					Debug.Log ("The winner of the GAME is: Player " + (i + 1).ToString() + " Score: " + scores[i].ToString());
					SceneManager.LoadScene ("Parr");		//To be replaced with Victory Screen
				}
			}
			StartCoroutine ("BeginNextRound");				//if no one has won the game, begin the next round
		}
	}

	/// <summary>
	/// Begins the next round. Gives each player a random starting location based on given list, instantiates them, and releases control to them after a timer.
	/// </summary>
	/// <returns>The next round.</returns>
	IEnumerator BeginNextRound () {
		Player tempPlayer;										//temporary instance variable to save memory

		yield return new WaitForSeconds (1.0f);					//Moment to breathe and check who won
		Debug.Log ("Next Round Begins in");
		for (int i = 0; i < players.Length; i++) {
			players [i].transform.position = spawnPoints [i].transform.position;
			players [i].transform.rotation = spawnPoints [i].transform.rotation;
			tempPlayer = players [i].GetComponent<Player> ();
			tempPlayer.poweredOn = false;
			tempPlayer.health = 100;
			tempPlayer.shield = 100;
			tempPlayer.canFire = true;
			tempPlayer.weapons.Clear ();
			players [i].SetActive (true);
			players [i].GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		}

		yield return new WaitForSeconds (1.0f);
		Debug.Log ("5");
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("4");
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("3");
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("2");
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("1");
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("GO!");

		for (int i = 0; i < players.Length; i++) {				//Activate players for next Round!
			players[i].GetComponent<Player>().poweredOn = true;
		}
		asteroidTime = true;
		StartCoroutine ("spawnAsteroids");
	}
}
