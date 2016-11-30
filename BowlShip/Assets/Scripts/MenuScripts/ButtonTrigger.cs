using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonTrigger : MonoBehaviour {

	public bool isNumerator = false;	//is this button used to change the score value
	public bool isIncrementor = true;	//does the button increase or decrease the score
	public int gamemode;				//which gamemode this button is reffering to
	public GameObject checkmark;		//the checkicon to further show selection
	public GameObject[] otherbuttons;	//the other buttons to uncheck upon new selection
	public GameObject[] switchMenus;	//allow the player to transition to a new menu
	public Text text;					//what does the score value mean?
	private SceneController sc;			//the scenecontroller to update the gamemode


	//Initializes everything
	void Start () {
		sc = GameObject.Find("SceneController").GetComponent<SceneController> ();
		text.text = sc.score.ToString ();
	}

	/// <summary>
	/// Check to see if the cursor is over it.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "WeaponFire") {
			if (isNumerator) {
				if (isIncrementor) {
					sc.score++;
				} else {
					if (sc.score > 1) {
						sc.score--;
					}
				}
				text.text = sc.score.ToString ();
			} else {
				sc.gameMode = gamemode;
				checkmark.SetActive (true);
				ChangeText ();
				for (int i = 0; i < otherbuttons.Length; i++) {
					otherbuttons [i].GetComponent<ButtonTrigger> ().UnCheck ();
				}
				for (int i = 0; i < switchMenus.Length; i++) {
					switchMenus [i].SetActive (true);
				}
			}
			Destroy (other.gameObject);
		}
	}

	/// <summary>
	/// Changes the text depending on what the game mode is.
	/// </summary>
	void ChangeText () {
		switch (gamemode) {
		case 0:
			text.text = "Rounds to Win";
			break;
		case 1:
			text.text = "Score to Win";
			break;
		case 2:
			text.text = "Time in Minutes";
			break;
		case 3:
			text.text = "Number of Lives";
			break;
		}
	}

	//When coming back make the player choose again
	void OnDisable() {
		if (text != null) {
			text.text = "";
		}
		if (isNumerator) {
			text.gameObject.SetActive (false);
			this.gameObject.SetActive (false);
		} else {
			UnCheck ();
		}
	}

	/// <summary>
	/// Called by other buttons to show new selection.
	/// </summary>
	public void UnCheck () {
		checkmark.SetActive (false);
	}
}
