using UnityEngine;
using System.Collections;

public class SwitchMenu : MonoBehaviour {

	public GameObject nextMenu;						//the next menu to transition to
	public GameObject currentMenu;					//the menu currently active to deactivate
	public GameObject characterSelection;			//used to reset character selection
	private int howManyPlayers = 0;					//the number of players ready to move on
	private SceneController sc;						//the scenecontroller


	// Use this for initialization
	void Start () {
		sc = GameObject.Find("SceneController").GetComponent<SceneController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (howManyPlayers >= (sc.numPlayers - sc.numAI)) {
			if (characterSelection != null) {
				if (sc.menuLevel < 2) {
					sc.menuLevel = 0;
					characterSelection.GetComponent<BetterCharacterSelection> ().Reset ();
				} else {
					sc.menuLevel = 0;
					GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
					for (int i = 0; i < players.Length; i++) {
						Destroy(players[i]);
					}
					players = GameObject.FindGameObjectsWithTag ("Nuke");
					for (int i = 0; i < players.Length; i++) {
						Destroy(players[i]);
					}
				}
			}
			nextMenu.SetActive (true);
			currentMenu.SetActive (false);
		}
	}

	/// <summary>
	/// Used to reset count in case players come back to this menu
	/// </summary>
	void OnDisable () {
		howManyPlayers = 0;
		this.gameObject.SetActive (false);
	}

	/// <summary>
	/// Check to see if all players are ready to move on.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			howManyPlayers++;
		}
	}

	/// <summary>
	/// Check to see if all players are ready to move on.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			howManyPlayers--;
		}
	}
}
