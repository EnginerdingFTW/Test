using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	//This script is almost entirely small public functions used in the Menu 
	//in order to do specific lines of code at runtime, functions implementations are fairly obvious

	public Slider scoreToWin;				//used in options to set max score
	public Text scoreToWinText;				//used in options to show max score
	public GameObject fadeIn;				//fadein/out between scenes
	private Animator fadeAnim;				//animator to call fadeout

	public Slider gameModeSlider;			//used in options to set game mode
	public Text currentGameMode;			//used in options to show current game mode

	//for player instantiation
	public Slider health;
	public Slider shield;
	public GameObject chargin;
	public GameObject weapon;
	public GameObject circle;

	private SceneController sc;				//used to set scene controller values in menu

	void Awake () {
		sc = GameObject.FindGameObjectWithTag ("SceneController").GetComponent<SceneController> ();
		fadeAnim = fadeIn.GetComponent<Animator> ();
	}

	public void PeoplePerController (bool two) {
		if (two) {
			Player.fireButton = "Fire";
		} else {
			Player.fireButton = "FireAlt";
		}
	}

	void OnSubmit() {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "Parr");
	}

	public void Test () {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "Parr");
	}

	public void Play () {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "Asteroids");
	}

	public void PlayWalls () {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "Walls");
	}

	public void PlayRotation () {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "Rotation");
	}

	public void PlayWarp () {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "WarpHoles");
	}

	public void Quit () {
		Application.Quit();
	}

	IEnumerator Load (string level) {
		yield return new WaitForSeconds (0.3f);
		SceneManager.LoadScene (level);
	}

	public void SetScoreToWin () {
		sc.setScore ((int) scoreToWin.value);
		scoreToWinText.text = "" + ((int)scoreToWin.value);
	}

	public void ToggleShieldRecharge () {
		Player.toggleShieldRecharge();
	}

	public void InstantiatePlayers() {
		for (int i = 0; i < sc.numPlayers; i++) {
			GameObject temp = (GameObject) Instantiate (sc.playerShips[i], sc.transform);
			temp.GetComponent<Player> ().playerNum = sc.playerNumArray [i];
			temp.GetComponent<Player>().AssignHUD (health, shield, chargin, weapon, circle);
			if (sc.playerShips[i].GetComponent<EnemyAI>() != null) {
				temp.tag = "Nuke";
				Destroy (temp.GetComponent<EnemyAI>());
			}
		}
	}

	public void SetGameMode () {
		sc.gameMode = (int) gameModeSlider.value;
		switch ((int) gameModeSlider.value) {
		case 0:
			currentGameMode.text = "Last Ship Standing";
			break;
		case 1:
			currentGameMode.text = "Score Attack";
			break;
		case 2:
			currentGameMode.text = "Time Attack";
			break;
		case 3:
			currentGameMode.text = "Stock Match";
			break;
		default:
			currentGameMode.text = "Error";
			break;
		}
	}
}
