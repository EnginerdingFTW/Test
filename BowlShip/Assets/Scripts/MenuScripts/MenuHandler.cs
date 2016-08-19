using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	//This script is almost entirely small public functions used in the Menu 
	//in order to do specific lines of code at runtime, functions implementations are fairly obvious

	public Slider scoreToWin;				//used in options to set max score
	public Text scoreToWinText;				//used in options to show max score

	public Slider gameModeSlider;			//used in options to set game mode
	public Text currentGameMode;			//used in options to show current game mode

	private SceneController sc;				//used to set scene controller values in menu

	void Start () {
		sc = GameObject.FindGameObjectWithTag ("SceneController").GetComponent<SceneController> ();
	}

	public void PeoplePerController (bool two) {
		if (two) {
			Player.fireButton = "Fire";
		} else {
			Player.fireButton = "FireAlt";
		}
	}

	void OnSubmit() {
		SceneManager.LoadScene ("Parr");
	}

	public void Test () {
		SceneManager.LoadScene ("Parr");
	}

	public void Play () {
		SceneManager.LoadScene ("Asteroids");
	}

	public void PlayWalls () {
		SceneManager.LoadScene ("Walls");
	}

	public void PlayRotation () {
		SceneManager.LoadScene ("Rotation");
	}

	public void PlayWarp () {
		SceneManager.LoadScene ("WarpHoles");
	}

	public void Quit () {
		Application.Quit();
	}

	public void SetScoreToWin () {
		sc.setScore ((int) scoreToWin.value);
		scoreToWinText.text = "" + ((int)scoreToWin.value);
	}

	public void ToggleShieldRecharge () {
		Player.toggleShieldRecharge();
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
