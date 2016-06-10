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
	private int numPlayers = 0;								//How many players are going to be in the game
	private int playersSelected = 0;						//the total number of players that have selected their ship

	//Generic public variables
	public GameObject characterSelection;					//the current menu
	public GameObject mainMenu;								//the previous menu
	public GameObject stageSelection;						//the next menu (Stage Selection)
	public GameObject[] spaceShips;							//A list of all PlayerSpaceships
	public Quaternion defaultRotation;						//The default rotation that all ships are turned when instantiated
	public Button defaultSelection;							//Upon entering stage selection, select a default button
	public float timeBetweenSelection = 0.5f;				//how much time a player has to wait in between selection
	public float selectionTolerance = 0.8f;					//how far the joystick must be moved to make a selection

	//UI elements
	public GameObject[] spawnPoints;						//a list of gameObjects with positions to spawn players at
	public Slider[] healthSliders;							//a list of all shieldSliders
	public Slider[] shieldSliders;							//a list of all healthSliders
	public GameObject[] charginStuff;						//A list of all charging icons
	public GameObject[] playerCircle;						//a list of all playercircles
	public GameObject[] weaponIcons;						//a list of all weapon icons
	public Image[] textBackgrounds;							//a list of all textBackgrounds to set false when playerEntered
	public GameObject[] arrows;								//a list of all the arrows to set true when playerEntered

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
		for (int i = 0; i < TOTALPLAYERS; i++) {
			playerEntered [i] = false;
			playerSelected [i] = false;
			playerWaiting [i] = false;
			playerChoice [i] = 0;
			healthSliders [i].gameObject.SetActive(false);
			shieldSliders [i].gameObject.SetActive (false);
			charginStuff [i].SetActive (false);
			playerCircle [i].SetActive (false);
			weaponIcons [i].SetActive (false);
			textBackgrounds [i].gameObject.SetActive (true);
			arrows [i].gameObject.SetActive (false);
		}
		numPlayers = 0;
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
	/// Every frame checks for input by a player to enter in the match. 
	/// </summary>
	void CheckForPlayerEntering () {
		for (int i = 0; i < TOTALPLAYERS; i++) {
			if (!playerEntered[i] && (Input.GetAxis(string.Concat("Fire", (i + 1).ToString())) > selectionTolerance)) {
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
			StartCoroutine ("PlayerWait", player);
			playerChoice [player]++;
			if (playerChoice [player] >= spaceShips.Length) {
				playerChoice [player] = 0;
			}
			SwitchShip (player);
		}
		if (!playerWaiting[player] && Input.GetAxis (string.Concat("Horizontal" + (player + 1).ToString())) < -selectionTolerance) {
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
		players [player] = (GameObject) Instantiate (spaceShips [playerChoice[player]], spawnPoints [player].transform.position, defaultRotation);
		playerScript [player] = players [player].GetComponent<Player> ();
		playerScript [player].poweredOn = false;
		playerScript [player].AssignHUD (healthSliders [player], shieldSliders [player], charginStuff [player], weaponIcons [player], playerCircle [player]);
	}

	/// <summary>
	/// Every frame runs through the players and checks for selection input, only if they have already entered
	/// </summary>
	void PlayerConfirmation () {
		for (int i = 0; i < TOTALPLAYERS; i++) {
			if (playerEntered [i] && !playerSelected [i] && !playerWaiting [i]) {
				if (Input.GetAxis(string.Concat("Fire" + (i + 1).ToString())) > selectionTolerance) {
					playerSelected [i] = true;
					arrows [i].gameObject.SetActive (false);
					healthSliders [i].gameObject.SetActive (true);
					shieldSliders [i].gameObject.SetActive (true);
					charginStuff [i].SetActive (true);
					weaponIcons [i].SetActive (true);
					playerCircle [i].SetActive (true);
					playerScript [i].playerNum = i + 1;
					playerScript [i].poweredOn = true;
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
			if (playersSelected == numPlayers) {
				playersSelected = 0;
				sceneController.numPlayers = numPlayers;
				for (int i = 0; i < TOTALPLAYERS; i++) {
					if (playerSelected [i]) {
						sceneController.playerShips [playersSelected] = spaceShips[playerChoice[i]];
						sceneController.playerNumArray [playersSelected] = i + 1;
						playersSelected++;
					}
				}
				if (playersSelected == numPlayers) {
					stageSelection.SetActive (true);
					defaultSelection.Select();
					characterSelection.SetActive (false);
				} else {
					Debug.Log ("ERROR: The number of players assigned to scene controller does not match the number of players that have selected");
					Reset ();
				}
			}
		}
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
