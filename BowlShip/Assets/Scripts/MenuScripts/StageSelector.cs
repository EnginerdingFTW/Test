using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageSelector : MonoBehaviour {

	public int initialTimeToStart = 100;				//How long it takes for a stage to be selected (num / 10 = seconds)
	public Image timer;									//A timer circle to show progress of gamestarting
	public int stage;									//0 for asteroids, 1 for walls, 2 for rotation, 3 for warpholes
	public GameObject prevMenu;							//used to fix glitch where you can only go back once

	private MenuHandler mh;								//The script that will load the stage
	private SpriteRenderer spr;							//The stage sprite to slowly fade in
	private Color color;								//The color of the stage sprite
	public int numPlayers;								//The number of players hovering over the stage sprite
	public int timeTillPlay;							//How much longer until the stage is selected
	private bool ready = true;							//Don't count infinitely

	// Use this for initialization
	void Start () {
		mh = GameObject.FindGameObjectWithTag ("Collectable").GetComponent<MenuHandler>();
		spr = GetComponent<SpriteRenderer> ();
		color = spr.color;
		numPlayers = 0;
		timeTillPlay = initialTimeToStart;
	}

	/// <summary>
	/// Reset every time the player switches back a menu.
	/// </summary>
	void OnDisable() {
		numPlayers = 0;
		ready = true;
		timeTillPlay = initialTimeToStart;
	}

	/// <summary>
	/// Allows prev menu to activate
	/// </summary>
	void OnEnable() {
		prevMenu.SetActive (true);
	}

	/// <summary>
	/// Checks to see how many players are hovering over the stage.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			numPlayers++;
		}
	}

	/// <summary>
	/// Checks to see how many players are hovering over the stage.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerExit2D (Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			numPlayers--;
			if (numPlayers < 0) {
				numPlayers = 0;
			}
		}
	}

	// Update is called once per frame. Increase time till player while players are on it, decrease otherwise.
	void Update () {
		if (ready) {
			ready = false;
			StartCoroutine ("Countdown");
		}
		if (timeTillPlay <= 0) {
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
			for (int i = 0; i < players.Length; i++) {
				Destroy(players[i]);
			}
			players = GameObject.FindGameObjectsWithTag ("Nuke");
			for (int i = 0; i < players.Length; i++) {
				Destroy(players[i]);
			}
			switch (stage) {
			case 0:
				mh.Play ();
				break;
			case 1:
				mh.PlayWalls ();
				break;
			case 2:
				mh.PlayRotation ();
				break;
			case 3:
				mh.PlayWarp ();
				break;
			}
		}
	}

	/// <summary>
	/// Countdown the timer while there are players over the stage.
	/// </summary>
	IEnumerator Countdown () {
		if (numPlayers == 0 && timeTillPlay <= initialTimeToStart) {
			timeTillPlay += 2;
		} else {
			timeTillPlay -= 1 * numPlayers;																		//more players = faster countdown
		}

		color.a = 0.5f + (((float)(initialTimeToStart - timeTillPlay) / (float)initialTimeToStart) / 2);		//slowly fade in the stage sprite as it gets closer to selection
		spr.color = color;
		timer.fillAmount = ((float)(initialTimeToStart - timeTillPlay) / (float)initialTimeToStart);			//add a circular timer to show even more obvious stage selection

		yield return new WaitForSeconds (0.1f);
		ready = true;
	}
}
