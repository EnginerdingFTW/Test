using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public List<GameObject>	SpawnerList = new List<GameObject>();		//List of all the current spawners on the map
	public List<GameObject> CollectableList = new List<GameObject>();	//List of all the Collectables tht have spawned
	public GameObject sceneCamera;										//The camera of the scene, used to set the final audio
	public GameObject trophy;											//The trophy to spawn for the winner to play with
	public GameObject[] powerups;										//the total list of spawnable powerups
	public GameObject[] spawnPoints;									//a list of gameObjects with positions to spawn players at
	public GameObject outerBoundary;									//the boundary to destroy faraway objects
	public GameObject innerBoundary;									//the boundary wrapped around the screen
	public GameObject bigAsteroid;										//the big Asteroid to be instantiated
	public GameObject smallAsteroid;									//the small Asteroid to be instantiated
	public int gameMode;												//The int number corresponding to each gameMode
	public float timeOfTimedMatch = 185.0f;								//The time used in a Time Attack match
	public float timeForRespawn = 3.0f;									//How much time until a player comes back to life
	public bool paused = false;											//If the game is paused, stop time
	public int numPlayers; 												//The number of players remaining
	public bool useAsteroids = true;									//Should Asteroids be spawned?
	public float asteroidSpawnRate = 3.0f;								//how often the asteroids are spawned, smaller spawn rate means more asteroids in a set amount of time
	public float asteroidSpawnRateFaster = 2.0f;						//how often the asteroids are spawned in a game with 5 or more players.
	public float big_small_asteroidProb = 0.8f;							//the probability to spawn either a big or small asteroid
	public float AsteroidSpawnSpeed = 5.0f;								//How fast the asteroids move on spawn
	//public float AsteroidSpawnSpeedRatio = 0.33f;

	private bool[] shipsToStop;											//A list of booleans for each player that show whether or not to power down/up during pause menu
	private bool canPause = true;										//Can the game be paused

	//Audio Elements
	public AudioClip destroyed;											//The sound clip to be played when a player is destroyed
	public AudioClip victoryJingle;										//The sound clip to be played when a player has won
	public AudioClip victoryOver;										//The sound clip to be played when the game is over
	private AudioSource audioSource;									//The audioSource used to play our soundclips

	//UI Elements
	public GameObject[] circleImage;									//The spawn gameObject attached to be radially filled
	public GameObject pauseScreen;										//The menu to pull up for pause screens
	public Button pauseDefault;											//The default button to select when showing the pause menu
	public GameObject gameOverScreen;									//The menu to show asking for a rematch or return to menu
	public Button gameOverDefault;										//The default button to select when showing the gameOverScreen
	public Color fullHealthC;											//The default color of health for a player
	public Slider[] healthSliders;										//The HUD sliders that must be assigned to players
	public Slider[] shieldSliders;										//^^^same but for shields^^^
	public GameObject[] playerIcons;									//A list of all player icons to be displayed in the HUD
	public GameObject[] weaponIcons;									//A list of all player's weapon icons to be displayed in the HUD
	public GameObject[] scoreBoxs;										//A list of all player's score icons to be displayed in the HUD
	public GameObject[] chargeIndicators;								//A list of all player's charge/stun icons to be displayed in the HUD
	public GameObject[] barIcons;										//All the icons that make health/shield look nice
	public GameObject victoryIcon;										//The Icon that shows up when a player won a round
	public Vector3 victoryPosition;										//The position to instantiate a copied winner for a victorious round.
	public float victoryWaitTime = 1.5f;								//The time to wait to show round winner
	public GameObject roundStarter;										//The UI Message that displays the round countdown between rounds
	public Sprite roundReady;											//The UI Sprite to start a round
	public Sprite roundGo;												//The UI Sprite to initialize combat
	public Text roundWaitText;											//The text saying to wait or go
	public Text roundNumberText;										//The text displaying how much time is left till the next round
	public GameObject roundFire;										//The fire prefab with the Commence logo
	public string roundWaitString = "Next Round\nBegins In...";			//The text to be displayed when a round begins
	public string roundBeginString = "Commence\nBOWLSHIP";				//The text to be displayed when a round begins
	public float roundMoveAwaySpeed = 10.0f;							//The speed at which the GO icon moves off the screen
	public float roundDestroyTime = 3.0f;								//How soon after a match begins does the icon disappear
	public Text timeLeft;												//Used in a timebased game to show the remaining time in a round
	private Rigidbody2D roundStarterRB;									//The rigidbody for the roundStarter Icon
	private Text[] scoreBoxTexts;										//A list of all the text fields for the scoreboxs

	public int defaultPlayerHealth = 100;								//The default health and
	public int defaultPlayerShields = 100;								//shields to reset a player with

	private Image[] circleFill;											//The UI component by the spawn that shows when a player will be respawned
	private bool roundOver = false;										//Is the round finished? Used to stop nuke glitch of 2 wins for one
	private GameObject[] walls;											//All the walls in the scene
	private bool gameOver = false;										//Is the game over? 
	private bool asteroidTime = true;									//spawn Asteroids
	public int maxScore;												//the score needed to win the game
	public GameObject[] players;										//a list of the prefabs each player is controlling
	public int[] scores;												//a list of each scores corresponding to each player
	private SceneController sceneController;							//The script to pass values between scenes
	
	/// <summary>
	/// Start this instance, i.e. Start the playerList to keep track of. Instantiate each player. Assign and show each player's health HUD. Commence Asteroid bombardment after beginning next round.
	/// </summary>
	void Start () 
	{
		sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
		gameMode = sceneController.gameMode;
		audioSource = GetComponent<AudioSource> ();
		numPlayers = sceneController.numPlayers;
		if (numPlayers > 4) {
			asteroidSpawnRate = asteroidSpawnRateFaster;			//bigger games on Asteroids spawn even more asteroids.
		}
		players = new GameObject[numPlayers];
		maxScore = sceneController.score;
		roundStarterRB = roundStarter.GetComponent<Rigidbody2D> ();
		scores = new int[numPlayers];
		scoreBoxTexts = new Text[numPlayers];
		walls = GameObject.FindGameObjectsWithTag ("Wall");
		canPause = true;
		circleFill = new Image[numPlayers];
		shipsToStop = new bool[numPlayers];
		for (int i = 0; i < numPlayers; i++) {
			circleFill[i] = circleImage[i].GetComponent<Image> ();
			players [i] = Instantiate (sceneController.playerShips [i]);
			players [i].GetComponent<Player> ().AssignHUD (healthSliders [i], shieldSliders [i], chargeIndicators[i], weaponIcons[i], playerIcons[i]);  				//assigns the player their HUD
			players [i].GetComponent<Player> ().playerNum = sceneController.playerNumArray[i];
			ActivatePlayerHUD (i);
			healthSliders [i].value = defaultPlayerHealth;
			shieldSliders [i].value = defaultPlayerShields;
			players [i].SetActive (false);
			scoreBoxTexts [i] = scoreBoxs [i].GetComponentInChildren<Text> ();
		}
		if (gameMode == 2) {
			timeOfTimedMatch = maxScore * 60 + 5;
			timeLeft.gameObject.SetActive (true);
			StartCoroutine ("TimeBasedGame", timeOfTimedMatch);
		}
		if (gameMode == 3) {
			for (int i = 0; i < numPlayers; i++) {
				scores [i] = maxScore;
				scoreBoxTexts [i].text = scores [i].ToString ();
			}
		}
		StartCoroutine ("BeginNextRound");
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
	/// Destroy any leftover objects from previous round, like asteroids or weaponFire.
	/// A -1 player that shot value indicates self destruction.
	/// </summary>
	/// <param name="playerNum">Defeated Player's number.</param>
	public void CheckEnd (int playerNum, int playerThatShot) {
		int playerThatWasDefeated;						//the player that just died
		playerThatShot = FindPlayerJustDefeated (playerThatShot);
		GameObject[] asteroids;							//list of all leftover asteroids to destroy

		//Last Ship Standing
		if (gameMode == 0) {
			//Instance Variables to save memory
			bool isDraw = true;								//was this round a draw?

			//Debug.Log ("Player " + playerNum.ToString () + " Died");
			playerThatWasDefeated = FindPlayerJustDefeated (playerNum);
			audioSource.PlayOneShot (destroyed);
			DeactivatePlayerHUD (playerThatWasDefeated);

			numPlayers--;
			Debug.Log ("Player " + playerThatWasDefeated.ToString () + " Defeated!");
			if (!roundOver && numPlayers < 2) {
				roundOver = true;
				asteroidTime = false;

				asteroids = GameObject.FindGameObjectsWithTag ("Asteroid");
				for (int i = 0; i < asteroids.Length; i++) {
					Destroy (asteroids [i]);
				}

				asteroids = GameObject.FindGameObjectsWithTag ("WeaponFire");		//piggy back off asteroids variable to also destroy all weaponfire
				for (int i = 0; i < asteroids.Length; i++) {
					Destroy (asteroids [i]);
				}

				//walls = GameObject.FindGameObjectsWithTag ("Wall");					//disable all walls
				for (int i = 0; i < walls.Length; i++) {
					walls [i].SetActive (false);
				}

				for (int i = 0; i < players.Length; i++) {
					if (!players [i].GetComponent<Player> ().defeated) {
						scores [i]++;
						scoreBoxTexts [i].text = scores [i].ToString ();
						isDraw = false;						//Should never come to a draw, unless both players are defeated in the EXACT same frame
						Debug.Log ("The winner of the round is: Player " + (i + 1).ToString () + " Score: " + scores [i].ToString ());

						//Victory Screen
						sceneCamera.GetComponent<AudioSource> ().volume = 0;
						audioSource.PlayOneShot (victoryJingle);
						victoryIcon.SetActive (true);
						GameObject playerIcon = (GameObject)Instantiate (players [i], victoryPosition, Quaternion.identity);
						playerIcon.GetComponent<Player> ().canFire = false;
						players [i].SetActive (false);
						StartCoroutine ("ShowVictoryScreens", playerIcon);
					}
				}
				if (isDraw) {
					Debug.Log ("It's a draw!");
					roundWaitText.text = "DRAW!";
					StartCoroutine ("BeginNextRound");				//if no one has won the game, begin the next round
				}
				for (int i = 0; i < scores.Length; i++) {
					if (scores [i] >= maxScore) {
						Debug.Log ("The winner of the GAME is: Player " + (i + 1).ToString () + " Score: " + scores [i].ToString ());
						gameOver = true;
					}
				}
			}
		}

		//Score Attack Logic
		if (gameMode == 1) {
			playerThatWasDefeated = FindPlayerJustDefeated (playerNum);
			audioSource.PlayOneShot (destroyed);
			Debug.Log ("Player " + (playerThatWasDefeated + 1).ToString () + " Defeated!");
			DeactivatePlayerHUD (playerThatWasDefeated);
			if (playerThatShot != -1 && playerThatShot != playerThatWasDefeated) {
				scores [playerThatShot]++;
				scoreBoxTexts [playerThatShot].text = scores [playerThatShot].ToString ();
			}

			//CheckEnd of game
			for (int i = 0; i < scores.Length; i++) {
				if (scores [i] >= maxScore) {
					Debug.Log ("The winner of the GAME is: Player " + (i + 1).ToString () + " Score: " + scores [i].ToString ());
					gameOver = true;
					roundOver = true;
					asteroidTime = false;

					sceneCamera.GetComponent<AudioSource> ().volume = 0;
					audioSource.PlayOneShot (victoryJingle);
					victoryIcon.SetActive (true);
					GameObject playerIcon = (GameObject) Instantiate (players [i], victoryPosition, Quaternion.identity);
					playerIcon.GetComponent<Player> ().canFire = false;
					StartCoroutine ("ShowVictoryScreens", playerIcon);

					for (int j = 0; j < players.Length; j++) {							//disable all players
						players [j].SetActive (false);
					}

					asteroids = GameObject.FindGameObjectsWithTag ("Asteroid");
					for (int j = 0; j < asteroids.Length; j++) {
						Destroy (asteroids [j]);
					}

					asteroids = GameObject.FindGameObjectsWithTag ("WeaponFire");		//piggy back off asteroids variable to also destroy all weaponfire
					for (int j = 0; j < asteroids.Length; j++) {
						Destroy (asteroids [j]);
					}

					//walls = GameObject.FindGameObjectsWithTag ("Wall");					//disable all walls
					for (int j = 0; j < walls.Length; j++) {
						walls [j].SetActive (false);
					}
					break;
				}
			}
			if (!gameOver) {
				StartCoroutine ("TimedRespawn", playerThatWasDefeated);
			}
		}

		//Time Attack Logic
		if (gameMode == 2) {
			playerThatWasDefeated = FindPlayerJustDefeated (playerNum);
			audioSource.PlayOneShot (destroyed);
			Debug.Log ("Player " + (playerThatWasDefeated + 1).ToString () + " Defeated!");
			DeactivatePlayerHUD (playerThatWasDefeated);
			if (playerThatShot != -1 && playerThatShot != playerThatWasDefeated) {
				scores [playerThatShot]++;
				scoreBoxTexts [playerThatShot].text = scores [playerThatShot].ToString ();
			}
			StartCoroutine ("TimedRespawn", playerThatWasDefeated);
		}

		//Stock Match Logic
		if (gameMode == 3) {
			playerThatWasDefeated = FindPlayerJustDefeated (playerNum);
			audioSource.PlayOneShot (destroyed);
			Debug.Log ("Player " + (playerThatWasDefeated + 1).ToString () + " Defeated!");
			DeactivatePlayerHUD (playerThatWasDefeated);
			scores [playerThatWasDefeated]--;
			scoreBoxTexts [playerThatWasDefeated].text = scores [playerThatWasDefeated].ToString ();
			if (scores [playerThatWasDefeated] <= 0) {
				numPlayers--;
			} else {
				StartCoroutine ("TimedRespawn", playerThatWasDefeated);
			}
			if (!roundOver && numPlayers < 2) {
				roundOver = true;
				asteroidTime = false;

				asteroids = GameObject.FindGameObjectsWithTag ("Asteroid");
				for (int i = 0; i < asteroids.Length; i++) {
					Destroy (asteroids [i]);
				}

				asteroids = GameObject.FindGameObjectsWithTag ("WeaponFire");		//piggy back off asteroids variable to also destroy all weaponfire
				for (int i = 0; i < asteroids.Length; i++) {
					Destroy (asteroids [i]);
				}

				//walls = GameObject.FindGameObjectsWithTag ("Wall");					//disable all walls
				for (int i = 0; i < walls.Length; i++) {
					walls [i].SetActive (false);
				}

				for (int i = 0; i < players.Length; i++) {
					if (!players [i].GetComponent<Player> ().defeated) {
						Debug.Log ("The winner of the round is: Player " + (i + 1).ToString () + " Score: " + scores [i].ToString ());

						//Victory Screen
						sceneCamera.GetComponent<AudioSource> ().volume = 0;
						audioSource.PlayOneShot (victoryJingle);
						victoryIcon.SetActive (true);
						GameObject playerIcon = (GameObject)Instantiate (players [i], victoryPosition, Quaternion.identity);
						playerIcon.GetComponent<Player> ().canFire = false;
						players [i].SetActive (false);
						StartCoroutine ("ShowVictoryScreens", playerIcon);
						Debug.Log ("The winner of the GAME is: Player " + (i + 1).ToString () + " Score: " + scores [i].ToString ());
						gameOver = true;
					}
				}
			}
		}
	}

	/// <summary>
	/// Runs a timer that will eventually end the game.
	/// </summary>
	/// <returns>The based game.</returns>
	/// <param name="timeOfGame">Time of game.</param>
	IEnumerator TimeBasedGame (float timeOfGame) {
		int minutes = (int)timeOfGame / 60;
		int seconds = (int)timeOfGame % 60;

		while (true) {
			seconds--;
			if (seconds < 0) {
				minutes--;
				if (minutes < 0) {
					CheckWinner ();
					break;
				}
				seconds = 59;
			}
			if (seconds < 10) {
				timeLeft.text = minutes.ToString () + ":0" + seconds.ToString ();
			} else {
				timeLeft.text = minutes.ToString () + ":" + seconds.ToString ();
			}
			yield return new WaitForSeconds (1.0f);
		}
	}

	/// <summary>
	/// Used by TimeBasedGame to check who the winner of the game is, based on who has the highest score.
	/// </summary>
	void CheckWinner() {
		int highestScore = 0;
		int winners = 0;

		for (int i = 0; i < numPlayers; i++) {
			if (scores [i] > highestScore) {
				highestScore = scores [i];
			}
		}
		for (int i = 0; i < numPlayers; i++) {
			if (scores [i] == highestScore) {
				winners++;
			} else {
				players [i].SetActive (false);
				players [i].GetComponent<Player> ().defeated = true;
			}
		}
		if (winners == 1) {
			int i = 1;									//used to copy partial code from earlier
			for (int j = 0; j < numPlayers; j++) {
				if (scores [j] == highestScore) {
					i = j;
				}
			}
				
			Debug.Log ("The winner of the GAME is: Player " + (i + 1).ToString () + " Score: " + scores [i].ToString ());
			gameOver = true;
			roundOver = true;
			asteroidTime = false;

			sceneCamera.GetComponent<AudioSource> ().volume = 0;
			audioSource.PlayOneShot (victoryJingle);
			victoryIcon.SetActive (true);
			GameObject playerIcon = (GameObject)Instantiate (players [i], victoryPosition, Quaternion.identity);
			playerIcon.GetComponent<Player> ().canFire = false;
			StartCoroutine ("ShowVictoryScreens", playerIcon);

			GameObject[] asteroids = GameObject.FindGameObjectsWithTag ("Asteroid");
			for (int j = 0; j < asteroids.Length; j++) {
				Destroy (asteroids [j]);
			}

			asteroids = GameObject.FindGameObjectsWithTag ("WeaponFire");		//piggy back off asteroids variable to also destroy all weaponfire
			for (int j = 0; j < asteroids.Length; j++) {
				Destroy (asteroids [j]);
			}

			//walls = GameObject.FindGameObjectsWithTag ("Wall");					//disable all walls
			for (int j = 0; j < walls.Length; j++) {
				walls [j].SetActive (false);
			}
			players [i].SetActive (false);
		} else {
			gameMode = 0;
			maxScore = highestScore + 1;
			numPlayers = winners;
		}
	}

	/// <summary>
	/// Begins the next round. Gives each player a random starting location based on given list, instantiates them, and releases control to them after a timer.
	/// </summary>
	/// <returns>The next round.</returns>
	IEnumerator BeginNextRound () {
//		Player tempPlayer;										//temporary instance variable to save memory

//		yield return new WaitForSeconds (1.0f);					//moment to breathe and make sure all ships have gone through their destroy sequence
		Debug.Log ("Next Round Begins in");
		roundWaitText.text = roundWaitString;
		roundFire.SetActive (false);
		roundStarterRB.velocity = new Vector3 (0, 0, 0);
		roundStarter.transform.position = new Vector3(0, 0, 0);
		roundStarter.GetComponent<Image>().sprite = roundReady;
		roundStarter.SetActive(true);
	
		numPlayers = 0;
		for (int i = 0; i < players.Length; i++) {
			numPlayers++;
			RespawnPlayer (i);
			players [i].SetActive (true);
			ActivatePlayerHUD (i);
		}
			
		Debug.Log ("3");
		roundNumberText.text = "3";
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("2");
		roundNumberText.text = "2";
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("1");
		roundNumberText.text = "1";
		yield return new WaitForSeconds (1.0f);
		victoryIcon.SetActive (false);
		Debug.Log ("GO!");
		roundStarter.GetComponent<Image> ().sprite = roundGo;
		roundWaitText.text = roundBeginString;
		roundNumberText.text = "!!!";
		roundFire.SetActive (true);
		roundStarterRB.velocity = new Vector3 (roundMoveAwaySpeed, 0, 0);

		for (int i = 0; i < walls.Length; i++) {				//Activate all the walls for the round
			walls [i].SetActive(true);
		}

		for (int i = 0; i < players.Length; i++) {				//Activate players for next Round!
			players[i].GetComponent<Player>().poweredOn = true;
		}
		asteroidTime = true;
		if (useAsteroids) {
			StartCoroutine ("spawnAsteroids");
		}

		yield return new WaitForSeconds (roundDestroyTime);
		roundStarter.SetActive (false);
	}

	/// <summary>
	/// Called every time a round ends, first shows the victory screen, then proceeds to either begin the next round or display the gameOverScreen.
	/// </summary>
	/// <returns>The time to wait in order to appreciate the winner.</returns>
	/// <param name="winner">The winning player inside the win circle.</param>
	IEnumerator ShowVictoryScreens (GameObject winner) {
		yield return new WaitForSeconds (victoryWaitTime);
		roundOver = false;
		sceneCamera.GetComponent<AudioSource>().volume = sceneController.musicLevel;
		if (!gameOver) {
			Destroy (winner, 3.0f);
			StartCoroutine ("BeginNextRound");				//if no one has won the game, begin the next round
		} else {
			canPause = false;
			sceneCamera.GetComponent<AudioSource> ().clip = victoryOver;
			sceneCamera.GetComponent<AudioSource> ().Play();
			Instantiate (trophy, new Vector3 (0, 0, 0), Quaternion.identity);
			victoryIcon.SetActive (false);
			winner.GetComponent<Player> ().canFire = true;
			winner.GetComponent<Player> ().poweredOn = true;
			gameOverScreen.SetActive (true);
			gameOverDefault.Select ();
		}
	}

	/// <summary>
	/// Respawns the player.
	/// </summary>
	/// <param name="player">Player.</param>
	void RespawnPlayer (int player) {
		players [player].transform.position = spawnPoints [player].transform.position;
		players [player].transform.rotation = spawnPoints [player].transform.rotation;
		Player tempPlayer = players [player].GetComponent<Player> ();
		tempPlayer.poweredOn = false;
		tempPlayer.defeated = false;
		tempPlayer.health = 100;
		tempPlayer.shield = 20;
		tempPlayer.canFire = true;
		tempPlayer.canRecharge = true;
		tempPlayer.man = 0;
		tempPlayer.weapons.Clear ();
		players [player].GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		healthSliders [player].value = 100;
		healthSliders [player].GetComponentsInChildren<Image> () [1].color = fullHealthC;
		shieldSliders [player].value = 20;
	}

	/// <summary>
	/// Uses the Respawn Player function after a set amount of time, shows to the user how much time is left till respawn.
	/// </summary>
	/// <returns>The respawn.</returns>
	/// <param name="player">Player.</param>
	IEnumerator TimedRespawn (int player) {
		circleImage [player].SetActive (true);
		circleFill[player].fillAmount = 0;
		for (int i = 0; i < (int) (timeForRespawn * 10); i++) {
			yield return new WaitForSeconds (0.1f);
			circleFill[player].fillAmount += (0.1f / timeForRespawn);
		}
		if (!gameOver) {
			RespawnPlayer (player);
			players [player].SetActive (true);
			ActivatePlayerHUD (player);
			circleImage [player].SetActive (false);
			players [player].GetComponent<Player> ().poweredOn = true;
		}
	}

	/// <summary>
	/// Used by Start and BeginRound functions to set true the proper UI elements
	/// </summary>
	/// <param name="player">Player.</param>
	void ActivatePlayerHUD (int player) {
		healthSliders [player].gameObject.SetActive (true);
		shieldSliders [player].gameObject.SetActive (true);
		playerIcons [player].SetActive (true);
		weaponIcons [player].SetActive (true);
		scoreBoxs [player].SetActive (true);
		chargeIndicators [player].SetActive (true);
		barIcons [player].SetActive (true);
	}

	/// <summary>
	/// Used by the CheckEnd function to set false the proper UI elements
	/// </summary>
	/// <param name="playerDefeated">Player defeated.</param>
	void DeactivatePlayerHUD (int playerDefeated) {
		healthSliders [playerDefeated].gameObject.SetActive (false);
		shieldSliders [playerDefeated].gameObject.SetActive (false);
		playerIcons [playerDefeated].SetActive (false);
		weaponIcons [playerDefeated].SetActive (false);
		scoreBoxs [playerDefeated].SetActive (false);
		chargeIndicators [playerDefeated].SetActive (false);
		barIcons [playerDefeated].SetActive (false);
	}

	/// <summary>
	/// Used by the CheckEnd function to find which player actually was defeated, from what controller was used.
	/// </summary>
	/// <param name="playerNum">Player number.</param>
	int FindPlayerJustDefeated (int playerNum) {
		for (int i = 0; i < sceneController.numPlayers; i++) {
			if (playerNum == sceneController.playerNumArray [i]) {
				return i;
			}
		}
		return -1;	//error, should never get here
	}

	/// <summary>
	/// Pause the game and pull up a pause menu
	/// </summary>
	public void Pause () {
		if (canPause) {
			for (int i = 0; i < players.Length; i++) {
				if (players [i].GetComponent<Player> ().poweredOn) {
					shipsToStop [i] = true;
					players [i].GetComponent<Player> ().poweredOn = false;
				} else {
					shipsToStop [i] = false;
				}
			}
			pauseScreen.SetActive (true);
			canPause = false;
			paused = true;
			Time.timeScale = 0.0f;
			pauseDefault.Select ();
		}
	}

	/// <summary>
	/// Unpause the game and take down the pause menu
	/// </summary>
	public void UnPause () {
		for (int i = 0; i < players.Length; i++) {
			if (shipsToStop[i]) {
				shipsToStop [i] = false;
				players [i].GetComponent<Player> ().poweredOn = true;
			}
		}
		pauseScreen.SetActive (false);
		canPause = true;
		paused = false;
		Time.timeScale = 1.0f;
		//Input.ResetInputAxes ();
	}

	/// <summary>
	/// Used by the Game Over Screen to start the same scene again.
	/// </summary>
	public void Rematch () {
		UnPause ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	/// <summary>
	/// Used by the Game Over Screen to start boot up the main menu.
	/// </summary>
	public void MainMenu () {
		sceneController.numPlayers = 0;
		UnPause ();
		sceneController.menuLevel = 1;
		SceneManager.LoadScene ("Menu");
	}

	/// <summary>
	/// Used by the Game Over Screen to start boot up the main menu on gamemode selection screen.
	/// </summary>
	public void MenuGamemode() {
		UnPause ();
		sceneController.menuLevel = 2;
		SceneManager.LoadScene ("Menu");
	}

	/// <summary>
	/// Used by the Game Over Screen to start boot up the main menu on stage selection screen.
	/// </summary>
	public void MenuStage() {
		UnPause ();
		sceneController.menuLevel = 3;
		SceneManager.LoadScene ("Menu");
	}

	/// <summary>
	/// Removes the collectable from list whenever a player picks it up to keep scene clean.
	/// </summary>
	/// <param name="collectable">Collectable.</param>
	public void RemoveCollectableFromList(GameObject collectable)
	{
		int list_index = 0;
		bool index_found = false;
		foreach (GameObject obj in this.CollectableList)
		{
			if (obj == collectable)
			{
				index_found = true;
				break;
			}
			list_index++;
		}
		if (index_found)
		{
			this.CollectableList.RemoveAt(list_index);
		}
		
	}
}
