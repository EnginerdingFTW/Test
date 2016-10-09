using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BetterCharacterSelection : MonoBehaviour {

	//Constants
	private int TOTALPLAYERS = 8;							//The maximum number of players

	// Generic private variables
	private GameObject sceneControllerObject;				//The sceneController GameObject
	private SceneController sceneController;				//The sceneController to assign player ships to
	private bool[] playerEntered;							//A list of booleans to see if the player has entered
	private bool[] playerSelected;							//A list of booleans to see if the player has selected their ship
	private bool[] playerWaiting;							//A list of booleans to make sure the player doesn't flip through choices too quickly
	private int[] playerChoice;								//A numerical representation of which ship the player has selected
	private GameObject[] players;							//The current ship that the player has active
	private Player[] playerScript;							//The script attached to each player
	private bool[] shipsTaken;								//An array to show which ships have been selected already
	private int numPlayers = 0;								//How many players are going to be in the game
	private int numAI = 0;									//How many CPUs are going to be in the game
	private int playersSelected = 0;						//the total number of players that have selected their ship
	private int[] aiDifficulty;								//The difficulty of each AI ship
	private bool[] isAI;									//which players are AI

	//Generic public variables
	public GameObject nextMenu;								//The prefab to allow ships to fly into the next menu
	public GameObject prevMenu;								//The prefab to allow ships to fly back to the main menu
	public GameObject[] spaceShips;							//A list of all PlayerSpaceships
	public GameObject[] spaceShipsAI;						//A list of all PlayerAISpaceships
	public GameObject[] returnTriggers;						//A list of all the locations that a player can return
	public Quaternion defaultRotation;						//The default rotation that all ships are turned when instantiated
	public Button defaultSelection;							//Upon entering stage selection, select a default button
	public float timeBetweenSelection = 0.5f;				//how much time a player has to wait in between selection
	public float selectionTolerance = 0.8f;					//how far the joystick must be moved to make a selection

	//UI elements
	public GameObject[] spawnPoints;						//a list of gameObjects with positions to spawn players at
	public GameObject[] spawnPointsAI;						//a list of gameObjects with positions to spawn playerAI at
	public Slider[] healthSliders;							//a list of all shieldSliders
	public Slider[] shieldSliders;							//a list of all healthSliders
	public GameObject[] charginStuff;						//A list of all charging icons
	public GameObject[] playerCircle;						//a list of all playercircles
	public GameObject[] weaponIcons;						//a list of all weapon icons
	public GameObject[] textBackgrounds;					//a list of all textBackgrounds to set false when playerEntered
	public GameObject[] arrows;								//a list of all the arrows to set true when playerEntered
	public GameObject[] brackets;							//a list of all the brackets to set false when playerSelected
	public GameObject[] aiSelection;						//a list of all selection "menus" for each possible AI player
	public GameObject[] destroyAI;							//a list of all selection cursors for the destroy AI option

	private Animator[] arrowAnims;							//a list of each of the arrows animators

	// Used for initialization of arrays and SceneController, default values, etc.
	void Start () {
		sceneControllerObject = GameObject.Find ("SceneController");
		sceneController = sceneControllerObject.GetComponent<SceneController> ();
		players = new GameObject[TOTALPLAYERS];
		playerScript = new Player[TOTALPLAYERS];
		playerEntered = new bool[TOTALPLAYERS];
		playerSelected = new bool[TOTALPLAYERS];
		playerWaiting = new bool[TOTALPLAYERS];
		playerChoice = new int[TOTALPLAYERS];
		isAI = new bool[TOTALPLAYERS];
		aiDifficulty = new int[TOTALPLAYERS];
		shipsTaken = new bool[TOTALPLAYERS];
		arrowAnims = new Animator[TOTALPLAYERS];
		for (int i = 0; i < TOTALPLAYERS; i++) {
			playerEntered [i] = false;
			playerSelected [i] = false;
			playerWaiting [i] = false;
			playerChoice [i] = 0;
			aiDifficulty [i] = 0;
			shipsTaken [i] = false;
			isAI [i] = false;
			healthSliders [i].gameObject.SetActive(false);
			shieldSliders [i].gameObject.SetActive (false);
			charginStuff [i].SetActive (false);
			playerCircle [i].SetActive (false);
			weaponIcons [i].SetActive (false);
			aiSelection [i].SetActive (false);
			destroyAI [i].SetActive (false);
			textBackgrounds [i].gameObject.SetActive (true);
			arrows [i].gameObject.SetActive (false);
			brackets [i].gameObject.SetActive (true);
			returnTriggers [i].gameObject.SetActive (false);
			arrowAnims [i] = arrows [i].GetComponent<Animator> ();
		}
		numPlayers = 0;
		numAI = 0;
		playersSelected = 0;
	}

	
	// Update is called once per frame (the main of this script) 
	void Update () {
		CheckForPlayerEntering ();
		PlayerOptions ();
		PlayerConfirmation ();
		MoveOn ();
	}

	/// <summary>
	/// Every frame checks for input by a player to enter in the match. Only allows 4 players if controls set to one person per controller.
	/// </summary>
	void CheckForPlayerEntering () {
		for (int i = 0; i < TOTALPLAYERS; i++) {
			if (!playerEntered[i] && !isAI[i] && (Input.GetAxis(string.Concat(Player.fireButton, (i + 1).ToString())) > selectionTolerance)) {
				if (!Player.fireButton.Equals ("Fire")) {
					if (i == 1 || i == 3 || i == 5 || i == 7) {
						continue;
					}
				}
				PlayerEnterInstantiation (i);
			}
		}
	}

	/// <summary>
	/// Used by CheckForPlayerEntering in order to show to the user that he has entered and is now selecting a ship. Updates playerNumber count.
	/// Assigns the player array and the playerScript array.
	/// </summary>
	/// <param name="player">Player.</param>
	void PlayerEnterInstantiation (int player) {
		playerEntered [player] = true;
		textBackgrounds [player].gameObject.SetActive (false);
		arrows [player].gameObject.SetActive (true);
		StartCoroutine ("PlayerWait", player);
		players[player] = (GameObject) Instantiate (spaceShips [0], spawnPoints [player].transform.position, defaultRotation);
		playerScript [player] = players [player].GetComponent<Player> ();
		playerScript [player].poweredOn = false;
		playerScript [player].AssignHUD (healthSliders [player], shieldSliders [player], charginStuff [player], weaponIcons [player], playerCircle [player]);
		numPlayers++;
	}

	/// <summary>
	/// Every frame runs through the array of possible players, if they are selecting a ship, call the PlayerChoices function
	/// </summary>
	void PlayerOptions () {
		for (int i = 0; i < playerEntered.Length; i++) {
			if (playerEntered [i] && !playerSelected[i]) {
				PlayerChoices (i);
			}
		}
	}

	/// <summary>
	/// Whenever a player enters right or left, change their ship to a new one by calling SwitchShip
	/// </summary>
	/// <param name="player">Player.</param>
	void PlayerChoices (int player) {
		if (!playerWaiting[player] && Input.GetAxis (string.Concat("Horizontal" + (player + 1).ToString())) > selectionTolerance) {
			arrowAnims [player].SetTrigger ("Right");
			StartCoroutine ("PlayerWait", player);
			playerChoice [player]++;
			if (playerChoice [player] >= spaceShips.Length) {
				playerChoice [player] = 0;
			}
			SwitchShip (player);
		}
		if (!playerWaiting[player] && Input.GetAxis (string.Concat("Horizontal" + (player + 1).ToString())) < -selectionTolerance) {
			arrowAnims [player].SetTrigger ("Left");
			StartCoroutine ("PlayerWait", player);
			playerChoice [player]--;
			if (playerChoice [player] < 0) {
				playerChoice [player] = spaceShips.Length - 1;
			}
			SwitchShip (player);
		}
	}

	/// <summary>
	/// Destroy the old ship, replace it with their current choice
	/// </summary>
	/// <param name="player">Player.</param>
	void SwitchShip (int player) {
		Destroy (players[player]);
		if (isAI [player]) {
			players [player] = (GameObject)Instantiate (spaceShips [playerChoice [player]], spawnPointsAI [player].transform.position, defaultRotation);
		} else {
			players [player] = (GameObject)Instantiate (spaceShips [playerChoice [player]], spawnPoints [player].transform.position, defaultRotation);
		}
		if (shipsTaken[playerChoice[player]]) {
			SpriteRenderer spr = players [player].GetComponent<SpriteRenderer> ();
			Color tmp = spr.color;
			tmp.a = 0.4f;
			spr.color = tmp;
		}
		playerScript [player] = players [player].GetComponent<Player> ();
		playerScript [player].poweredOn = false;
		playerScript [player].AssignHUD (healthSliders [player], shieldSliders [player], charginStuff [player], weaponIcons [player], playerCircle [player]);
	}

	/// <summary>
	/// Every frame runs through the players and checks for selection input, only if they have already entered (Also checks to make sure the ship isn't taken)
	/// </summary>
	void PlayerConfirmation () {
		for (int i = 0; i < TOTALPLAYERS; i++) {
			if (playerEntered [i] && !playerSelected [i] && !playerWaiting [i]) {
				if (Input.GetAxis(string.Concat(Player.fireButton + (i + 1).ToString())) > selectionTolerance) {
					if (!shipsTaken [playerChoice [i]]) {
						shipsTaken [playerChoice [i]] = true;
						playerSelected [i] = true;
						arrows [i].gameObject.SetActive (false);
						brackets [i].gameObject.SetActive (false);
						returnTriggers [i].gameObject.SetActive (true);
						healthSliders [i].gameObject.SetActive (true);
						shieldSliders [i].gameObject.SetActive (true);
						charginStuff [i].SetActive (true);
						weaponIcons [i].SetActive (true);
						playerCircle [i].SetActive (true);
						playerScript [i].playerNum = i + 1;
						playerScript [i].poweredOn = true;
						SpriteRenderer spr = players [i].GetComponent<SpriteRenderer> ();
						Color tmp = spr.color;
						tmp.a = 1f;
						spr.color = tmp;
					} //else {
						//error sound?
					//}
				}
			}
		}
	}

	/// <summary>
	/// Every frame checks to see if every player has selected their ship. Must have at least 2 players.
	/// If so then update sceneController and move on to Stage Selection.
	/// </summary>
	void MoveOn () {
		if (numPlayers > 1) {
			playersSelected = 0;
			for (int i = 0; i < TOTALPLAYERS; i++) {
				if (playerSelected [i]) {
					playersSelected++;
				}
			}
			if (playersSelected == numPlayers - numAI) {
				playersSelected = 0;
				sceneController.numPlayers = numPlayers;
				sceneController.numAI = numAI;
				for (int i = 0; i < TOTALPLAYERS; i++) {
					if (playerSelected [i]) {
						sceneController.playerShips [playersSelected] = spaceShips[playerChoice[i]];
						sceneController.playerNumArray [playersSelected] = i + 1;
						playersSelected++;
					}
					if (isAI[i]) {
						spaceShipsAI [playerChoice [i]].GetComponent<EnemyAI> ().difficulty = aiDifficulty [i];
						sceneController.playerShips [playersSelected] = spaceShipsAI [playerChoice [i]];
						sceneController.playerNumArray [playersSelected] = i + 1;
						playersSelected++;
					}
				}
				if (playersSelected == (sceneController.numPlayers)) {
					nextMenu.SetActive (true);
					prevMenu.SetActive (true);
					for (int i = 0; i < TOTALPLAYERS; i++) {
						brackets [i].SetActive (false);
						textBackgrounds [i].SetActive (false);
						destroyAI [i].SetActive (false);
						returnTriggers [i].SetActive (false);
					}
				} else {
					Debug.Log ("ERROR: The number of players assigned to scene controller does not match the number of players that have selected");
					Reset ();
				}
			}
		}
	}

	/// <summary>
	/// Call this when the Add CPU cursor is shot.
	/// </summary>
	/// <param name="player">Player.</param>
	public void AddAI (int player) {
		Debug.Log ("Adding AI for Player " + player);
		textBackgrounds [player].SetActive (false);
		aiSelection [player].SetActive (true);
		isAI [player] = true;
		players[player] = (GameObject) Instantiate (spaceShips [0], spawnPointsAI [player].transform.position, defaultRotation);
		if (shipsTaken[playerChoice[player]]) {
			SpriteRenderer spr = players [player].GetComponent<SpriteRenderer> ();
			Color tmp = spr.color;
			tmp.a = 0.4f;
			spr.color = tmp;
		}
		playerScript [player] = players [player].GetComponent<Player> ();
		playerScript [player].poweredOn = false;
		playerScript [player].AssignHUD (healthSliders [player], shieldSliders [player], charginStuff [player], weaponIcons [player], playerCircle [player]);
		numPlayers++;
	}

	/// <summary>
	/// Removes the AI player and allows player selection
	/// </summary>
	/// <param name="player">Player.</param>
	public void RemoveAI (int player) {
		Destroy (players[player]);
		destroyAI [player].SetActive (false);
		textBackgrounds [player].SetActive (true);
		brackets [player].SetActive (true);
		shipsTaken [playerChoice [player]] = false;
		isAI [player] = false;
		numAI--;
		numPlayers--;
	}

	/// <summary>
	/// Call this when the AI character and difficulty is confirmed.
	/// </summary>
	/// <param name="player">Player.</param>
	/// <param name="difficulty">Difficulty.</param>
	public void SetAI (int player, int difficulty) {
		if (!shipsTaken[playerChoice[player]]) {
			aiSelection [player].SetActive (false);
			shipsTaken [playerChoice [player]] = true;
			brackets [player].SetActive (false);
			destroyAI [player].SetActive (true);
			aiDifficulty [player] = difficulty;
//			Destroy (players[player]);
//			players[player] = (GameObject) Instantiate (spaceShipsAI [playerChoice[player]], spawnPointsAI [player].transform.position, defaultRotation);
			numAI++;
		}
	}

	/// <summary>
	/// Call this when the AI character is switching ships
	/// </summary>
	/// <param name="player">Player.</param>
	/// <param name="direction">Direction.</param>
	public void SwitchAI (int player, int direction) {
		if (direction == 0) {
			playerChoice [player]--;
			if (playerChoice [player] < 0) {
				playerChoice [player] = spaceShips.Length - 1;
			}
			SwitchShip (player);
		} else {
			playerChoice [player]++;
			if (playerChoice [player] >= spaceShips.Length) {
				playerChoice [player] = 0;
			}
			SwitchShip (player);
		}
	}

	/// <summary>
	/// Call this when an individual player wants to go back to change their ship.
	/// </summary>
	/// <param name="player">Player.</param>
	public void Return (int player) {
		Destroy (players [player]);
		sceneController.playerShips [player] = null;
		playerSelected [player] = false;
		shipsTaken [playerChoice [player]] = false;
		returnTriggers [player].gameObject.SetActive (false);
		arrows [player].gameObject.SetActive (true);
		brackets [player].gameObject.SetActive (true);
		healthSliders [player].gameObject.SetActive (false);
		shieldSliders [player].gameObject.SetActive (false);
		charginStuff [player].SetActive (false);
		weaponIcons [player].SetActive (false);
		playerCircle [player].SetActive (false);
		StartCoroutine ("PlayerWait", player);
		players[player] = (GameObject) Instantiate (spaceShips [0], spawnPoints [player].transform.position, defaultRotation);
		playerScript [player] = players [player].GetComponent<Player> ();
		playerScript [player].poweredOn = false;
		playerScript [player].AssignHUD (healthSliders [player], shieldSliders [player], charginStuff [player], weaponIcons [player], playerCircle [player]);
		playerChoice [player] = 0;
	}

	/// <summary>
	/// Call this when the player returns to the main screen. 
	/// Destroy any active Player objects and reset the CharacterSelection and SceneController values.
	/// </summary>
	public void Reset () {
		GameObject[] destroyable = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < destroyable.Length; i++) {
			Destroy (destroyable [i]);
		}
		for (int i = 0; i < sceneController.playerShips.Length; i++) {
			sceneController.playerShips [i] = null;
		}
		sceneController.numPlayers = 0;
		Start ();
	}

	/// <summary>
	/// This function is called to set a short timer that stops player input. This way selection is managable.
	/// </summary>
	/// <returns>The wait.</returns>
	/// <param name="player">Which player is waiting, 0-7.</param>
	IEnumerator PlayerWait (int player) {
		playerWaiting[player] = true;
		yield return new WaitForSeconds (timeBetweenSelection);
		playerWaiting[player] = false;
	}
}
