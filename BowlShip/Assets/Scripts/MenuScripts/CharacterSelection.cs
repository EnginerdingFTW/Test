using UnityEngine;
using System.Collections;

public class CharacterSelection : MonoBehaviour {

	//This class currently doesn't account for only player 1 and player 3 wanting to play. i.e. The game won't begin if Player 1 and Player 3 join but Player 2 never does.
	//It was also done at 4:30am and very poorly written

	private GameObject sceneControllerObject;				//The sceneController GameObject
	private SceneController sceneController;				//The sceneController to assign player ships to

	private bool player1Selected = false;					//Did this player join?
	public int player1Choosing;								//The ship selection of the player, determined by horizontal movement
	private bool player1Waiting = false;					//time in between selection
	private bool player1Ready = false;						//Did player1 select their ship?
	private bool player2Selected = false;					//repeat for every other player
	private int player2Choosing;
	private bool player2Waiting = false;
	private bool player2Ready = false;
	private bool player3Selected = false;
	private int player3Choosing;
	private bool player3Waiting = false;
	private bool player3Ready = false;
	private bool player4Selected = false;
	private int player4Choosing;
	private bool player4Waiting = false;
	private bool player4Ready = false;
	private bool player5Selected = false;
	private int player5Choosing;
	private bool player5Waiting = false;
	private bool player5Ready = false;
	private bool player6Selected = false;
	private int player6Choosing;
	private bool player6Waiting = false;
	private bool player6Ready = false;
	private bool player7Selected = false;
	private int player7Choosing;
	private bool player7Waiting = false;
	private bool player7Ready = false;
	private bool player8Selected = false;
	private int player8Choosing;
	private bool player8Waiting = false;
	private bool player8Ready = false;

	public GameObject parrShip1;							//show Player 1 that parrShip is currently Selected
	public GameObject borrosShip1;							//show Player 1 that borrosShip is currently Selected
	public GameObject tayShip1;								//show Player 1 that tayShip is currently Selected
	public GameObject parrShip2;							//etc.
	public GameObject borrosShip2;
	public GameObject tayShip2;
	public GameObject parrShip3;
	public GameObject borrosShip3;
	public GameObject tayShip3;
	public GameObject parrShip4;
	public GameObject borrosShip4;
	public GameObject tayShip4;
	public GameObject parrShip5;
	public GameObject borrosShip5;
	public GameObject tayShip5;
	public GameObject parrShip6;
	public GameObject borrosShip6;
	public GameObject tayShip6;
	public GameObject parrShip7;
	public GameObject borrosShip7;
	public GameObject tayShip7;
	public GameObject parrShip8;
	public GameObject borrosShip8;
	public GameObject tayShip8;

	public GameObject characterSelection;					//the current menu
	public GameObject mainMenu;								//the previous menu
	public GameObject stageSelection;						//the next menu (Stage Selection)
	public GameObject parrShip;								//green spaceship
	public GameObject borrosShip;							//blue spaceship
	public GameObject tayShip;								//red spaceship
	public float timeBetweenSelection = 0.1f;				//how much time a player has to wait in between selection
	public float selectionTolerance = 0.8f;					//how far the joystick must be moved to make a selection


	// Use this for initialization
	void Start () {
		sceneControllerObject = GameObject.Find ("SceneController");
		sceneController = sceneControllerObject.GetComponent<SceneController> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Press 'Fire' to enter
		if (!player1Selected && (Input.GetAxis("Fire1") > selectionTolerance)) {
			player1Selected = true;
			sceneController.playerShips [0] = parrShip;
			parrShip1.SetActive (true);
			sceneController.numPlayers++;
			StartCoroutine ("Player1Wait");
		}
		if (!player2Selected && (Input.GetAxis("Fire2") > selectionTolerance)) {
			player2Selected = true;
			sceneController.playerShips [1] = parrShip;
			parrShip2.SetActive (true);
			sceneController.numPlayers++;
			StartCoroutine ("Player2Wait");
		}
		if (!player3Selected && (Input.GetAxis("Fire3") > selectionTolerance)) {
			player3Selected = true;
			sceneController.playerShips [2] = parrShip;
			parrShip3.SetActive (true);
			sceneController.numPlayers++;
			StartCoroutine ("Player3Wait");
		}
		if (!player4Selected && (Input.GetAxis("Fire4") > selectionTolerance)) {
			player4Selected = true;
			sceneController.playerShips [3] = parrShip;
			parrShip4.SetActive (true);
			sceneController.numPlayers++;
			StartCoroutine ("Player4Wait");
		}
		if (!player5Selected && (Input.GetAxis("Fire5") > selectionTolerance)) {
			player5Selected = true;
			sceneController.playerShips [4] = parrShip;
			parrShip5.SetActive (true);
			sceneController.numPlayers++;
			StartCoroutine ("Player5Wait");
		}
		if (!player6Selected && (Input.GetAxis("Fire6") > selectionTolerance)) {
			player6Selected = true;
			sceneController.playerShips [5] = parrShip;
			parrShip6.SetActive (true);
			sceneController.numPlayers++;
			StartCoroutine ("Player6Wait");
		}
		if (!player7Selected && (Input.GetAxis("Fire7") > selectionTolerance)) {
			player7Selected = true;
			sceneController.playerShips [6] = parrShip;
			parrShip7.SetActive (true);
			sceneController.numPlayers++;
			StartCoroutine ("Player7Wait");
		}
		if (!player8Selected && (Input.GetAxis("Fire8") > selectionTolerance)) {
			player8Selected = true;
			sceneController.playerShips [7] = parrShip;
			parrShip8.SetActive (true);
			sceneController.numPlayers++;
			StartCoroutine ("Player8Wait");
		}

		//Player 1's options upon entering
		if (player1Selected) {
			if (!player1Waiting && Input.GetAxis ("Horizontal1") > selectionTolerance) {
				player1Choosing++;
				StartCoroutine ("Player1Wait");
			}
			if (!player1Waiting && Input.GetAxis ("Horizontal1") < -selectionTolerance) {
				player1Choosing--;
				StartCoroutine ("Player1Wait");
			}
			if (player1Choosing > 2) {
				player1Choosing = 0;
			}
			if (player1Choosing < 0) {
				player1Choosing = 2;
			}

			//Pick out a ship
			if (player1Choosing == 0) {
				sceneController.playerShips [0] = parrShip;
				tayShip1.SetActive (false);
				parrShip1.SetActive (true);
				borrosShip1.SetActive (false);
			}
			if (player1Choosing == 1) {
				sceneController.playerShips [0] = tayShip;
				tayShip1.SetActive (true);
				parrShip1.SetActive (false);
				borrosShip1.SetActive (false);
			}
			if (player1Choosing == 2) {
				sceneController.playerShips [0] = borrosShip;
				tayShip1.SetActive (false);
				parrShip1.SetActive (false);
				borrosShip1.SetActive (true);
			}
				
			//Confirm your ship
			if (!player1Waiting && Input.GetAxis ("Fire1") == 1.0) {
				player1Ready = true;
				StartCoroutine ("Player1Wait");
			}
		}

		//Player 2's options upon entering
		if (player2Selected) {
			if (!player2Waiting && Input.GetAxis ("Horizontal2") > selectionTolerance) {
				player2Choosing++;
				StartCoroutine ("Player2Wait");
			}
			if (!player2Waiting && Input.GetAxis ("Horizontal2") < -selectionTolerance) {
				player2Choosing--;
				StartCoroutine ("Player2Wait");
			}
			if (player2Choosing > 2) {
				player2Choosing = 0;
			}
			if (player2Choosing < 0) {
				player2Choosing = 2;
			}

			//Pick out a ship
			if (player2Choosing == 0) {
				sceneController.playerShips [1] = parrShip;
				tayShip2.SetActive (false);
				parrShip2.SetActive (true);
				borrosShip2.SetActive (false);
			}
			if (player2Choosing == 1) {
				sceneController.playerShips [1] = tayShip;
				tayShip2.SetActive (true);
				parrShip2.SetActive (false);
				borrosShip2.SetActive (false);
			}
			if (player2Choosing == 2) {
				sceneController.playerShips [1] = borrosShip;
				tayShip2.SetActive (false);
				parrShip2.SetActive (false);
				borrosShip2.SetActive (true);
			}

			//Confirm your ship
			if (!player2Waiting && Input.GetAxis ("Fire2") == 1.0) {
				player2Ready = true;
				StartCoroutine ("Player2Wait");
			}
		}

		//Player 3's options upon entering
		if (player3Selected) {
			if (!player3Waiting && Input.GetAxis ("Horizontal3") > selectionTolerance) {
				player3Choosing++;
				StartCoroutine ("Player3Wait");
			}
			if (!player3Waiting && Input.GetAxis ("Horizontal3") < -selectionTolerance) {
				player3Choosing--;
				StartCoroutine ("Player3Wait");
			}
			if (player3Choosing > 2) {
				player3Choosing = 0;
			}
			if (player3Choosing < 0) {
				player3Choosing = 2;
			}

			//Pick out a ship
			if (player3Choosing == 0) {
				sceneController.playerShips [2] = parrShip;
				tayShip3.SetActive (false);
				parrShip3.SetActive (true);
				borrosShip3.SetActive (false);
			}
			if (player3Choosing == 1) {
				sceneController.playerShips [2] = tayShip;
				tayShip3.SetActive (true);
				parrShip3.SetActive (false);
				borrosShip3.SetActive (false);
			}
			if (player3Choosing == 2) {
				sceneController.playerShips [2] = borrosShip;
				tayShip3.SetActive (false);
				parrShip3.SetActive (false);
				borrosShip3.SetActive (true);
			}

			//Confirm your ship
			if (!player3Waiting && Input.GetAxis ("Fire3") == 1.0) {
				player3Ready = true;
				StartCoroutine ("Player3Wait");
			}
		}

		//Player 4's options upon entering
		if (player4Selected) {
			//Pick out a ship
			if (!player4Waiting && Input.GetAxis ("Horizontal4") > selectionTolerance) {
				player4Choosing++;
				StartCoroutine ("Player4Wait");
			}
			if (!player4Waiting && Input.GetAxis ("Horizontal4") < -selectionTolerance) {
				player4Choosing--;
				StartCoroutine ("Player4Wait");
			}
			if (player4Choosing > 2) {
				player4Choosing = 0;
			}
			if (player4Choosing < 0) {
				player4Choosing = 2;
			}
			if (player4Choosing == 0) {
				sceneController.playerShips [3] = parrShip;
				tayShip4.SetActive (false);
				parrShip4.SetActive (true);
				borrosShip4.SetActive (false);
			}
			if (player4Choosing == 1) {
				sceneController.playerShips [3] = tayShip;
				tayShip4.SetActive (true);
				parrShip4.SetActive (false);
				borrosShip4.SetActive (false);
			}
			if (player4Choosing == 2) {
				sceneController.playerShips [3] = borrosShip;
				tayShip4.SetActive (false);
				parrShip4.SetActive (false);
				borrosShip4.SetActive (true);
			}

			//Confirm your ship
			if (!player4Waiting && Input.GetAxis ("Fire4") == 1.0) {
				player4Ready = true;
				StartCoroutine ("Player4Wait");
			}
		}

		//Player 5's options upon entering
		if (player5Selected) {
			if (!player5Waiting && Input.GetAxis ("Horizontal5") > selectionTolerance) {
				player5Choosing++;
				StartCoroutine ("Player5Wait");
			}
			if (!player5Waiting && Input.GetAxis ("Horizontal5") < -selectionTolerance) {
				player5Choosing--;
				StartCoroutine ("Player5Wait");
			}
			if (player5Choosing > 2) {
				player5Choosing = 0;
			}
			if (player5Choosing < 0) {
				player5Choosing = 2;
			}

			//Pick out a ship
			if (player5Choosing == 0) {
				sceneController.playerShips [4] = parrShip;
				tayShip5.SetActive (false);
				parrShip5.SetActive (true);
				borrosShip5.SetActive (false);
			}
			if (player5Choosing == 1) {
				sceneController.playerShips [4] = tayShip;
				tayShip5.SetActive (true);
				parrShip5.SetActive (false);
				borrosShip5.SetActive (false);
			}
			if (player5Choosing == 2) {
				sceneController.playerShips [4] = borrosShip;
				tayShip5.SetActive (false);
				parrShip5.SetActive (false);
				borrosShip5.SetActive (true);
			}

			//Confirm your ship
			if (!player5Waiting && Input.GetAxis ("Fire5") == 1.0) {
				player5Ready = true;
				StartCoroutine ("Player5Wait");
			}
		}

		//Player 6's options upon entering
		if (player6Selected) {
			if (!player6Waiting && Input.GetAxis ("Horizontal6") > selectionTolerance) {
				player6Choosing++;
				StartCoroutine ("Player6Wait");
			}
			if (!player6Waiting && Input.GetAxis ("Horizontal6") < -selectionTolerance) {
				player6Choosing--;
				StartCoroutine ("Player6Wait");
			}
			if (player6Choosing > 2) {
				player6Choosing = 0;
			}
			if (player6Choosing < 0) {
				player6Choosing = 2;
			}

			//Pick out a ship
			if (player6Choosing == 0) {
				sceneController.playerShips [5] = parrShip;
				tayShip6.SetActive (false);
				parrShip6.SetActive (true);
				borrosShip6.SetActive (false);
			}
			if (player6Choosing == 1) {
				sceneController.playerShips [5] = tayShip;
				tayShip6.SetActive (true);
				parrShip6.SetActive (false);
				borrosShip6.SetActive (false);
			}
			if (player6Choosing == 2) {
				sceneController.playerShips [5] = borrosShip;
				tayShip6.SetActive (false);
				parrShip6.SetActive (false);
				borrosShip6.SetActive (true);
			}

			//Confirm your ship
			if (!player6Waiting && Input.GetAxis ("Fire6") == 1.0) {
				player6Ready = true;
				StartCoroutine ("Player6Wait");
			}
		}

		//Player 7's options upon entering
		if (player7Selected) {
			if (!player7Waiting && Input.GetAxis ("Horizontal7") > selectionTolerance) {
				player7Choosing++;
				StartCoroutine ("Player7Wait");
			}
			if (!player7Waiting && Input.GetAxis ("Horizontal7") < -selectionTolerance) {
				player7Choosing--;
				StartCoroutine ("Player7Wait");
			}
			if (player7Choosing > 2) {
				player7Choosing = 0;
			}
			if (player7Choosing < 0) {
				player7Choosing = 2;
			}

			//Pick out a ship
			if (player7Choosing == 0) {
				sceneController.playerShips [6] = parrShip;
				tayShip7.SetActive (false);
				parrShip7.SetActive (true);
				borrosShip7.SetActive (false);
			}
			if (player7Choosing == 1) {
				sceneController.playerShips [6] = tayShip;
				tayShip7.SetActive (true);
				parrShip7.SetActive (false);
				borrosShip7.SetActive (false);
			}
			if (player7Choosing == 2) {
				sceneController.playerShips [6] = borrosShip;
				tayShip7.SetActive (false);
				parrShip7.SetActive (false);
				borrosShip7.SetActive (true);
			}


			//Confirm your ship
			if (!player7Waiting && Input.GetAxis ("Fire7") == 1.0) {
				player7Ready = true;
				StartCoroutine ("Player7Wait");
			}
		}

		//Player 8's options upon entering
		if (player8Selected) {
			if (!player8Waiting && Input.GetAxis ("Horizontal8") > selectionTolerance) {
				player8Choosing++;
				StartCoroutine ("Player8Wait");
			}
			if (!player8Waiting && Input.GetAxis ("Horizontal8") < -selectionTolerance) {
				player8Choosing--;
				StartCoroutine ("Player8Wait");
			}
			if (player8Choosing > 2) {
				player8Choosing = 0;
			}
			if (player8Choosing < 0) {
				player8Choosing = 2;
			}

			//Pick out a ship
			if (player8Choosing == 0) {
				sceneController.playerShips [7] = parrShip;
				tayShip8.SetActive (false);
				parrShip8.SetActive (true);
				borrosShip8.SetActive (false);
			}
			if (player8Choosing == 1) {
				sceneController.playerShips [7] = tayShip;
				tayShip8.SetActive (true);
				parrShip8.SetActive (false);
				borrosShip8.SetActive (false);
			}
			if (player8Choosing == 2) {
				sceneController.playerShips [7] = borrosShip;
				tayShip8.SetActive (false);
				parrShip8.SetActive (false);
				borrosShip8.SetActive (true);
			}

			//Confirm your ship
			if (!player8Waiting && Input.GetAxis ("Fire8") == 1.0) {
				player8Ready = true;
				StartCoroutine ("Player8Wait");
			}
		}

		//Check to see if everyone is ready in order to move on
		if (sceneController.numPlayers == 1) {
			if (player1Ready) {
				stageSelection.SetActive (true);
				characterSelection.SetActive (false);
			}
		}
		if (sceneController.numPlayers == 2) {
			if (player1Ready && player2Ready) {
				stageSelection.SetActive (true);
				characterSelection.SetActive (false);
			}
		}
		if (sceneController.numPlayers == 3) {
			if (player1Ready && player2Ready && player3Ready) {
				stageSelection.SetActive (true);
				characterSelection.SetActive (false);
			}
		}
		if (sceneController.numPlayers == 4) {
			if (player1Ready && player2Ready && player3Ready && player4Ready) {
				stageSelection.SetActive (true);
				characterSelection.SetActive (false);
			}
		}
		if (sceneController.numPlayers == 5) {
			if (player1Ready && player2Ready && player3Ready && player4Ready && player5Ready) {
				stageSelection.SetActive (true);
				characterSelection.SetActive (false);
			}
		}
		if (sceneController.numPlayers == 6) {
			if (player1Ready && player2Ready && player3Ready && player4Ready && player5Ready && player6Ready) {
				stageSelection.SetActive (true);
				characterSelection.SetActive (false);
			}
		}
		if (sceneController.numPlayers == 7) {
			if (player1Ready && player2Ready && player3Ready && player4Ready && player5Ready && player6Ready && player7Ready) {
				stageSelection.SetActive (true);
				characterSelection.SetActive (false);
			}
		}
		if (sceneController.numPlayers == 8) {
			if (player1Ready && player2Ready && player3Ready && player4Ready && player5Ready && player6Ready && player7Ready && player8Ready) {
				stageSelection.SetActive (true);
				characterSelection.SetActive (false);
			}
		}

		//Reset if the main menu is opened up
		if (mainMenu.gameObject.activeSelf) {
			sceneController.numPlayers = 0;
			player1Ready = false;
			player1Selected = false;
			sceneController.playerShips [1] = null;
			player2Ready = false;
			player2Selected = false;
			sceneController.playerShips [2] = null;
			player3Ready = false;
			player3Selected = false;
			sceneController.playerShips [3] = null;
			player4Ready = false;
			player4Selected = false;
			sceneController.playerShips [4] = null;
			player5Ready = false;
			player5Selected = false;
			sceneController.playerShips [5] = null;
			player6Ready = false;
			player6Selected = false;
			sceneController.playerShips [6] = null;
			player7Ready = false;
			player7Selected = false;
			sceneController.playerShips [7] = null;
			player8Ready = false;
			player8Selected = false;
			sceneController.playerShips [0] = null;
		}
	}

	IEnumerator Player1Wait () {
		player1Waiting = true;
		yield return new WaitForSeconds (timeBetweenSelection);
		player1Waiting = false;
	}

	IEnumerator Player2Wait () {
		player2Waiting = true;
		yield return new WaitForSeconds (timeBetweenSelection);
		player2Waiting = false;
	}

	IEnumerator Player3Wait () {
		player3Waiting = true;
		yield return new WaitForSeconds (timeBetweenSelection);
		player3Waiting = false;
	}

	IEnumerator Player4Wait () {
		player4Waiting = true;
		yield return new WaitForSeconds (timeBetweenSelection);
		player4Waiting = false;
	}

	IEnumerator Player5Wait () {
		player5Waiting = true;
		yield return new WaitForSeconds (timeBetweenSelection);
		player5Waiting = false;
	}

	IEnumerator Player6Wait () {
		player6Waiting = true;
		yield return new WaitForSeconds (timeBetweenSelection);
		player6Waiting = false;
	}

	IEnumerator Player7Wait () {
		player7Waiting = true;
		yield return new WaitForSeconds (timeBetweenSelection);
		player7Waiting = false;
	}

	IEnumerator Player8Wait () {
		player8Waiting = true;
		yield return new WaitForSeconds (timeBetweenSelection);
		player8Waiting = false;
	}
}
